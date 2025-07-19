using System.Collections.Generic;
using System;

public class MissilePool : GenericObjectPool<MissileController>
{
    private MissileScriptable missileScriptable;

    public MissileController GetMissile<T>(MissileScriptable missileScriptable) where T : MissileController
    {
        this.missileScriptable = missileScriptable;
        return GetItem<T>();
    }

    protected override MissileController CreateItem<T>()
    {
        switch (typeof(T))
        {
            case Type t when t == typeof(AGM65):
                return new AGM65(missileScriptable);
            case Type t when t == typeof(AGM114):
                return new AGM114(missileScriptable);
            case Type t when t == typeof(AIM7):
                return new AIM7(missileScriptable);
            case Type t when t == typeof(AIM9):
                return new AIM9(missileScriptable);
            case Type t when t == typeof(GBU12b):
                return new GBU12b(missileScriptable);
            case Type t when t == typeof(HJ10):
                return new HJ10(missileScriptable);
            case Type t when t == typeof(JDAM):
                return new JDAM(missileScriptable);
            case Type t when t == typeof(JDAM2):
                return new JDAM2(missileScriptable);
            case Type t when t == typeof(KAB500L):
                return new KAB500L(missileScriptable);
            case Type t when t == typeof(Kh29):
                return new Kh29(missileScriptable);
            case Type t when t == typeof(PL11):
                return new PL11(missileScriptable);
            case Type t when t == typeof(PL112):
                return new PL112(missileScriptable);
            case Type t when t == typeof(R27):
                return new R27(missileScriptable);
            case Type t when t == typeof(R272):
                return new R272(missileScriptable);
            case Type t when t == typeof(R77):
                return new R77(missileScriptable);
            case Type t when t == typeof(TY90):
                return new TY90(missileScriptable);
            default:
                throw new NotSupportedException($"Power-up type '{typeof(T)}' is not supported.");
                break;
        }
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
}
