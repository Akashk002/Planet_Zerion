using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceCraftService
{
    private List<EnemySpaceCraftData> enemySpaceCraftDatas;
    private EnemySpaceCraftPool enemySpaceCraftPool;

    public EnemySpaceCraftService(List<EnemySpaceCraftData> enemySpaceCraftDatas)
    {
        this.enemySpaceCraftDatas = enemySpaceCraftDatas;
        enemySpaceCraftPool = new EnemySpaceCraftPool();
    }

    public EnemySpaceCraftController CreateEnemySpaceCraft(EnemySpaceCraftType enemySpaceCraftType, Vector3 initialPos, Vector3 targetPos)
    {
        EnemySpaceCraftController enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<EnemySpaceCraftController>(enemySpaceCraftDatas, enemySpaceCraftType);
        enemySpaceCraftController.Configure(initialPos, targetPos);
        return enemySpaceCraftController;
    }

    public void ReturnEnemySpaceCraftPool(EnemySpaceCraftController enemySpaceCraftController) => enemySpaceCraftPool.ReturnItem(enemySpaceCraftController);
}