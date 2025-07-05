using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpacecraftScriptableObject", menuName = "ScriptableObjects/SpacecraftScriptableObject")]
public class SpacecraftScriptable : ScriptableObject
{
    public SpacecraftType spacecraftType;
    public SpacecraftView spacecraftView;
    public int health;
    public MissileType missileType;
    public int missileCapacity = 15;
    public int maxRange = 100;

    [Header("Movement")]
    public float maxSpeed;
    public float verticalSpeed;
    public float brakeSpeed;
    public float accelerationSpeed;

    [Header("Rotation")]
    public float rotationSpeed;
    public float mouseSensitivity;
    public float maxPitch;

    [Header("Sprite")]
    public Sprite spacecraftSprite;

    [Header("Purchase Data")]
    public SpacecraftStatus spacecraftStatus = SpacecraftStatus.Locked;
    public int rocksRequire = 2;
}

[System.Serializable]
public class SpacecraftData
{
    public SpacecraftType spacecraftType;
    public SpacecraftScriptable spacecraftScriptable;
}

public enum SpacecraftType
{
    Corvette_01,
    Corvette_02,
    Corvette_03,
    Corvette_04,
    Corvette_05,
    Frigate_01,
    Frigate_02,
    Frigate_03,
    Frigate_04,
    Frigate_05,
}

public enum SpacecraftStatus
{
    Locked,
    Selected,
    Unlocked
}