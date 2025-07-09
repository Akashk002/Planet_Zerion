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
        MissileController missileController = missilePool.GetMissile<MissileController>(missileDatas, missileType);
        missileController.Configure(InitialTransform, targetPos, turningEnable);
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

[System.Serializable]
public class MissileData
{
    public MissileType missileType;
    public MissileScriptable missileScriptable;
}

public enum MissileType
{
    AGM65,
    AGM114,
    AIM7,
    AIM9,
    GBU12b,
    HJ10,
    JDAM,
    JDAM2,
    KAB500L,
    Kh29,
    PL11,
    PL112,
    R27,
    R272,
    R77,
    TY90
}
