using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpaceCraftScriptableObject", menuName = "ScriptableObjects/EnemySpaceCraftScriptableObject")]
public class EnemySpaceCraftScriptable : ScriptableObject
{
    public EnemySpaceCraftType enemySpaceCraftType; // Type of the enemy spacecraft
    public EnemySpaceCraftView enemySpaceCraftView;
    public MissileType missileType; // Type of the missile used by the enemy spacecraft
    public float health = 100f; // Health of the enemy spacecraft
    public float moveSpeed = 10f;
    public float fireInterval = 5f;
}

[System.Serializable]
public class EnemySpaceCraftData
{
    public EnemySpaceCraftType enemySpaceCraftType;
    public EnemySpaceCraftScriptable enemySpaceCraftScriptable;
}

public enum EnemySpaceCraftType
{
    Destroyer_1,
    Destroyer_2,
    Destroyer_3,
    Destroyer_4,
    Destroyer_5,
    LightCruiser_1,
    LightCruiser_2,
    LightCruiser_3,
    LightCruiser_4,
    LightCruiser_5,

}

