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
        EnemySpaceCraftScriptable enemySpaceCraftScriptable = enemySpaceCraftDatas.Find(m => m.enemySpaceCraftType == enemySpaceCraftType).enemySpaceCraftScriptable;
        EnemySpaceCraftController enemySpaceCraftController = GetEnemySpaceCraft(enemySpaceCraftType, enemySpaceCraftScriptable);
        enemySpaceCraftController.Configure(initialPos, targetPos);
        return enemySpaceCraftController;
    }

    private EnemySpaceCraftController GetEnemySpaceCraft(EnemySpaceCraftType enemySpaceCraftType, EnemySpaceCraftScriptable enemySpaceCraftScriptable)
    {
        EnemySpaceCraftController enemySpaceCraftController = null;
        switch (enemySpaceCraftType)
        {
            case EnemySpaceCraftType.Destroyer_1:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Destroyer_1>(enemySpaceCraftScriptable);
                break;
            case EnemySpaceCraftType.Destroyer_2:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Destroyer_2>(enemySpaceCraftScriptable);
                break;
            case EnemySpaceCraftType.Destroyer_3:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Destroyer_3>(enemySpaceCraftScriptable);
                break;
            case EnemySpaceCraftType.Destroyer_4:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Destroyer_4>(enemySpaceCraftScriptable);
                break;
            case EnemySpaceCraftType.Destroyer_5:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Destroyer_5>(enemySpaceCraftScriptable);
                break;
            case EnemySpaceCraftType.LightCruiser_1:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Lightcruiser_1>(enemySpaceCraftScriptable);
                break;
            case EnemySpaceCraftType.LightCruiser_2:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Lightcruiser_2>(enemySpaceCraftScriptable);
                break;
            case EnemySpaceCraftType.LightCruiser_3:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Lightcruiser_3>(enemySpaceCraftScriptable);
                break;
            case EnemySpaceCraftType.LightCruiser_4:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Lightcruiser_4>(enemySpaceCraftScriptable);
                break;
            case EnemySpaceCraftType.LightCruiser_5:
                enemySpaceCraftController = enemySpaceCraftPool.GetEnemySpaceCraft<Lightcruiser_5>(enemySpaceCraftScriptable);
                break;
            default:
                break;
        }
        return enemySpaceCraftController;
    }

    public void ReturnEnemySpaceCraftPool(EnemySpaceCraftController enemySpaceCraftController) => enemySpaceCraftPool.ReturnItem(enemySpaceCraftController);
}