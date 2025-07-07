using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a Generic Object Pool Class with basic functionality, which can be inherited to implement object pools for any type of objects.
/// </summary>
/// <typeparam object Type to be pooled = "T"></typeparam>
public class GenericObjectPool<T> where T : class
{
    public List<PooledItem<T>> pooledItems = new List<PooledItem<T>>();

    public virtual T GetItem<U>() where U : T
    {
        //Debug.Log($"[ObjectPool] Requesting item of type {typeof(U).Name}. Pool size: {pooledItems.Count}");
        if (pooledItems.Count > 0)
        {
            PooledItem<T> item = pooledItems.Find(item => !item.isUsed && item.Item is U);
            if (item != null)
            {
                item.isUsed = true;
                //Debug.Log($"[ObjectPool] Reusing existing item of type {typeof(U).Name}.");
                //Debug.Log($"[ObjectPool] Item details: {item.Item.GetType().Name}, Used: {item.isUsed}");
                return item.Item;
            }
        }
        //Debug.Log($"[ObjectPool] No available item of type {typeof(U).Name}, creating new.");
        return CreateNewPooledItem<U>();
    }

    private T CreateNewPooledItem<U>() where U : T
    {
        PooledItem<T> newItem = new PooledItem<T>();
        newItem.Item = CreateItem<U>();
        newItem.isUsed = true;
        pooledItems.Add(newItem);
        //Debug.Log($"[ObjectPool] Created new item of type {typeof(U).Name}. New pool size: {pooledItems.Count}");
        return newItem.Item;
    }

    protected virtual T CreateItem<U>() where U : T
    {
        throw new NotImplementedException("CreateItem() method not implemented in derived class");
    }

    public virtual void ReturnItem(T item)
    {
        PooledItem<T> pooledItem = pooledItems.Find(i => ReferenceEquals(i.Item, item));
        if (pooledItem != null)
        {
            pooledItem.isUsed = false;
            //Debug.Log($"[ObjectPool] Returned item of type {item.GetType().Name} to pool.");
        }
        else
        {
            // Debug.LogWarning($"[ObjectPool] Tried to return item of type {item.GetType().Name}, but it was not found in the pool.");
        }
    }

    public class PooledItem<T>
    {
        public T Item;
        public bool isUsed;
    }
}
