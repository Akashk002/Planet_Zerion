using System.Collections.Generic;
using System;
using UnityEngine;

public class MissilePool : GenericObjectPool<MissileController>
{
    private List<MissileData> missileDatas;
    private MissileType missileType;

    public MissileController GetMissile<T>(List<MissileData> missileDatas, MissileType missileType) where T : MissileController
    {
        this.missileDatas = missileDatas;
        this.missileType = missileType;
        var item = pooledItems.Find(p => !p.isUsed && p.Item.missileType == missileType);

        if (item != null)
        {
            item.isUsed = true;
            return item.Item;
        }

        return GetItem<T>();
    }

    public List<MissileController> GetAllMissiles()
    {
        List<MissileController> activeMissiles = new List<MissileController>();
        foreach (var item in pooledItems)
        {
            if (item.isUsed)
            {
                activeMissiles.Add(item.Item);
            }
        }
        return activeMissiles;
    }


    protected override MissileController CreateItem<T>()
    {
        MissileScriptable missileScriptable = missileDatas.Find(m => m.missileType == missileType).missileScriptable;

        if (typeof(T) == typeof(MissileController))
            return new MissileController(missileScriptable);
        else
            throw new NotSupportedException($"Power-up type '{typeof(T)}' is not supported.");
    }
}
