using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneScriptableObject", menuName = "ScriptableObjects/DroneScriptableObject")]
public class DroneScriptable : ScriptableObject
{
    public DroneType droneType;
    public DroneView droneView;
    public float droneBatteryDecRate;
    public float droneBatteryChargingTime;

    [Header("Movement")]
    public float maxSpeed;
    public float verticalSpeed;
    public float AccelerationSpeed;

    [Header("Rotation")]
    public float rotationSpeed;
    public float mouseSensitivity;
    public float maxPitch;

    [Header("Rock Detail")]
    public List<RockData> rockDatas = new List<RockData>();
    public float RockStorageCapacity;
}

[System.Serializable]
public class DroneData
{
    public DroneType droneType;
    public DroneScriptable droneScriptable;
}

[System.Serializable]
public enum DroneType
{
    CarrierDrone,
    SecurityDrone,
}