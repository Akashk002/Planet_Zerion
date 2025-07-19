using System;
using System.Collections.Generic;

public class EnemySpaceCraftPool : GenericObjectPool<EnemySpaceCraftController>
{
    private EnemySpaceCraftScriptable enemySpaceCraftScriptable;

    public EnemySpaceCraftController GetEnemySpaceCraft<T>(EnemySpaceCraftScriptable enemySpaceCraftScriptable) where T : EnemySpaceCraftController
    {
        this.enemySpaceCraftScriptable = enemySpaceCraftScriptable;
        return GetItem<T>();
    }

    protected override EnemySpaceCraftController CreateItem<T>()
    {
        switch (typeof(T))
        {
            case Type t when t == typeof(Destroyer_1):
                return new Destroyer_1(enemySpaceCraftScriptable);
            case Type t when t == typeof(Destroyer_2):
                return new Destroyer_2(enemySpaceCraftScriptable);
            case Type t when t == typeof(Destroyer_3):
                return new Destroyer_3(enemySpaceCraftScriptable);
            case Type t when t == typeof(Destroyer_4):
                return new Destroyer_4(enemySpaceCraftScriptable);
            case Type t when t == typeof(Destroyer_5):
                return new Destroyer_5(enemySpaceCraftScriptable);
            case Type t when t == typeof(Lightcruiser_1):
                return new Lightcruiser_1(enemySpaceCraftScriptable);
            case Type t when t == typeof(Lightcruiser_2):
                return new Lightcruiser_2(enemySpaceCraftScriptable);
            case Type t when t == typeof(Lightcruiser_3):
                return new Lightcruiser_3(enemySpaceCraftScriptable);
            case Type t when t == typeof(Lightcruiser_4):
                return new Lightcruiser_4(enemySpaceCraftScriptable);
            case Type t when t == typeof(Lightcruiser_5):
                return new Lightcruiser_5(enemySpaceCraftScriptable);
            default:
                throw new NotSupportedException($"This type '{typeof(T)}' is not supported.");
                break;
        }
    }
}


