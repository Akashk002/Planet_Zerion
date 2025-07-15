using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileService
{
    private List<MissileData> missileDatas;
    private MissilePool missilePool;

    public MissileService(List<MissileData> missileDatas)
    {
        this.missileDatas = missileDatas;
        missilePool = new MissilePool();
    }

    public void CreateMissile(MissileType missileType, Transform InitialTransform, Vector3 targetPos, bool turningEnable = true)
    {
        MissileScriptable missileScriptable = missileDatas.Find(m => m.missileType == missileType).missileScriptable;
        MissileController missileController = GetMissileController(missileType, missileScriptable);
        missileController.Configure(InitialTransform, targetPos, turningEnable);
    }

    private MissileController GetMissileController(MissileType missileType, MissileScriptable missileScriptable)
    {
        MissileController missileController = null;
        switch (missileType)
        {
            case MissileType.AGM65:
                missileController = missilePool.GetMissile<AGM65>(missileScriptable);
                break;
            case MissileType.AGM114:
                missileController = missilePool.GetMissile<AGM114>(missileScriptable);
                break;
            case MissileType.AIM7:
                missileController = missilePool.GetMissile<AIM7>(missileScriptable);
                break;
            case MissileType.AIM9:
                missileController = missilePool.GetMissile<AIM9>(missileScriptable);
                break;
            case MissileType.GBU12b:
                missileController = missilePool.GetMissile<GBU12b>(missileScriptable);
                break;
            case MissileType.HJ10:
                missileController = missilePool.GetMissile<HJ10>(missileScriptable);
                break;
            case MissileType.JDAM:
                missileController = missilePool.GetMissile<JDAM>(missileScriptable);
                break;
            case MissileType.JDAM2:
                missileController = missilePool.GetMissile<JDAM2>(missileScriptable);
                break;
            case MissileType.KAB500L:
                missileController = missilePool.GetMissile<KAB500L>(missileScriptable);
                break;
            case MissileType.Kh29:
                missileController = missilePool.GetMissile<Kh29>(missileScriptable);
                break;
            case MissileType.PL11:
                missileController = missilePool.GetMissile<PL11>(missileScriptable);
                break;
            case MissileType.PL112:
                missileController = missilePool.GetMissile<PL112>(missileScriptable);
                break;
            case MissileType.R27:
                missileController = missilePool.GetMissile<R27>(missileScriptable);
                break;
            case MissileType.R272:
                missileController = missilePool.GetMissile<R272>(missileScriptable);
                break;
            case MissileType.R77:
                missileController = missilePool.GetMissile<R77>(missileScriptable);
                break;
            case MissileType.TY90:
                missileController = missilePool.GetMissile<TY90>(missileScriptable);
                break;
            default:
                Debug.LogError($"Missile type {missileType} is not supported.");
                return null;

        }
        return missileController;
    }

    public void DeactivateAllMissile()
    {
        foreach (var missile in missilePool.GetAllMissiles())
        {
            missile.Destroy();
        }
    }

    public void ReturnDefenderPool(MissileController missileController) => missilePool.ReturnItem(missileController);
}


