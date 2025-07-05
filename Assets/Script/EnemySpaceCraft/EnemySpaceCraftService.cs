using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceCraftService
{
    private List<EnemySpaceCraftScriptable> EnemySpaceCraftScriptables;
    private EnemySpaceCraftPool enemySpaceCraftPool;

    public EnemySpaceCraftService(List<EnemySpaceCraftScriptable> enemySpaceCraftScriptables)
    {
        this.EnemySpaceCraftScriptables = enemySpaceCraftScriptables;
        enemySpaceCraftPool = new EnemySpaceCraftPool();

        foreach (EnemySpaceCraftScriptable enemySpaceCraftScriptable in enemySpaceCraftScriptables)
        {
            enemySpaceCraftPool.Initialize(enemySpaceCraftScriptable); // Initialize with the first data and a pool size of 10
        }
    }

    public void CreateEnemySpaceCraft(Vector3 initialPos, Vector3 targetPos)
    {
        EnemySpaceCraftScriptable enemySpaceCraftScriptable = EnemySpaceCraftScriptables[Random.Range(0, EnemySpaceCraftScriptables.Count)];
        EnemySpaceCraftController enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<EnemySpaceCraftController>(enemySpaceCraftScriptable);

        enemySpaceCraftController.Configure(initialPos, targetPos);
    }

    public void ReturnEnemySpaceCraftPool(EnemySpaceCraftController enemySpaceCraftController) => enemySpaceCraftPool.ReturnItem(enemySpaceCraftController);
}