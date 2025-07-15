using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissileScriptableObject", menuName = "ScriptableObjects/MissileScriptableObject")]
public class MissileScriptable : ScriptableObject
{
    public MissileType missileType; // Type of the missile
    public MissileView missileView;
    public float moveSpeed = 10f;
    public float damage = 50f; // Damage value for the missile
    public float boostSpeed = 3f; // For launch phase

    [Header("Sprite")]
    public Sprite spacecraftSprite;
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