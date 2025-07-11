using System.Collections.Generic;
using System;
using UnityEngine;

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
        if (typeof(T) == typeof(AGM65))
            return new AGM65(missileScriptable);
        else if (typeof(T) == typeof(AGM114))
            return new AGM114(missileScriptable);
        else if (typeof(T) == typeof(AIM7))
            return new AIM7(missileScriptable);
        else if (typeof(T) == typeof(AIM9))
            return new AIM9(missileScriptable);
        else if (typeof(T) == typeof(GBU12b))
            return new GBU12b(missileScriptable);
        else if (typeof(T) == typeof(HJ10))
            return new HJ10(missileScriptable);
        else if (typeof(T) == typeof(JDAM))
            return new JDAM(missileScriptable);
        else if (typeof(T) == typeof(JDAM2))
            return new JDAM2(missileScriptable);
        else if (typeof(T) == typeof(KAB500L))
            return new KAB500L(missileScriptable);
        else if (typeof(T) == typeof(Kh29))
            return new Kh29(missileScriptable);
        else if (typeof(T) == typeof(PL11))
            return new PL11(missileScriptable);
        else if (typeof(T) == typeof(PL112))
            return new PL112(missileScriptable);
        else if (typeof(T) == typeof(R27))
            return new R27(missileScriptable);
        else if (typeof(T) == typeof(R272))
            return new R272(missileScriptable);
        else if (typeof(T) == typeof(R77))
            return new R77(missileScriptable);
        else if (typeof(T) == typeof(TY90))
            return new TY90(missileScriptable);

        else
            throw new NotSupportedException($"Power-up type '{typeof(T)}' is not supported.");
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
