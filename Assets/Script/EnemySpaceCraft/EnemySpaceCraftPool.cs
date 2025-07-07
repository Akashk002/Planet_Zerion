using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceCraftPool : GenericObjectPool<EnemySpaceCraftController>
{
    private List<EnemySpaceCraftData> enemySpaceCraftDatas;
    private EnemySpaceCraftType enemySpaceCraftType;

    public EnemySpaceCraftController GetEnemySpaceCraft<T>(List<EnemySpaceCraftData> enemySpaceCraftDatas, EnemySpaceCraftType enemySpaceCraftType) where T : EnemySpaceCraftController
    {
        this.enemySpaceCraftDatas = enemySpaceCraftDatas;
        this.enemySpaceCraftType = enemySpaceCraftType;

        var item = pooledItems.Find(p => !p.isUsed && p.Item.enemySpaceCraftType == enemySpaceCraftType);

        if (item != null)
        {
            item.isUsed = true;
            return item.Item;
        }

        return GetItem<T>();
    }

    protected override EnemySpaceCraftController CreateItem<T>()
    {
        EnemySpaceCraftScriptable enemySpaceCraftScriptable = enemySpaceCraftDatas.Find(m => m.enemySpaceCraftType == enemySpaceCraftType).enemySpaceCraftScriptable;

        if (typeof(T) == typeof(EnemySpaceCraftController))
            return new EnemySpaceCraftController(enemySpaceCraftScriptable);
        else
            throw new NotSupportedException($"This type '{typeof(T)}' is not supported.");
    }
}


