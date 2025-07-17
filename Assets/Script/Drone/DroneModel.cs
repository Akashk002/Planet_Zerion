using System.Collections.Generic;
using UnityEngine;

public class DroneModel
{
    private DroneScriptable droneScriptable;
    private LTDescr batteryLeenTween;
    private DroneUIManager droneUIManager;
    public float AccelerationSpeed;
    public float maxSpeed;
    public float verticalSpeed;
    public float rotationSpeed;
    public float mouseSensitivity;
    public float maxPitch;
    public float droneBattery;
    public DroneType droneType;
    public DroneView dronePrefab;
    public List<RockData> rockDatas;


    public DroneModel(DroneScriptable droneScriptable)
    {
        this.droneScriptable = droneScriptable;
        AccelerationSpeed = droneScriptable.AccelerationSpeed;
        maxSpeed = droneScriptable.maxSpeed;
        verticalSpeed = droneScriptable.verticalSpeed;
        rotationSpeed = droneScriptable.rotationSpeed;
        mouseSensitivity = droneScriptable.mouseSensitivity;
        maxPitch = droneScriptable.maxPitch;
        droneType = droneScriptable.droneType;
        dronePrefab = droneScriptable.droneView;
        rockDatas = droneScriptable.rockDatas;
        droneBattery = 100f; // Initialize battery to full
        droneUIManager = UIManager.Instance.droneUIManager;
    }

    public void AddRock(RockType rockType, Vector3 pos)
    {
        if (GetTotalRock() < droneScriptable.RockStorageCapacity)
        {
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.CollectRock, pos);
            RockData rockData = droneScriptable.rockDatas.Find(r => r.RockType == rockType);
            rockData?.AddRock();
            droneUIManager.UpdateRockCount();
        }
        else
        {
            UIManager.Instance.GetInfoHandler().ShowInstruction(InstructionType.StorageFull);
        }
    }

    private int GetTotalRock()
    {
        int total = 0;
        foreach (var r in droneScriptable.rockDatas)
            total += r.rockCount;
        return total;
    }

    public void SetBattery(float usage)
    {
        droneBattery -= usage * droneScriptable.droneBatteryDecRate * .001f;
        droneBattery = Mathf.Clamp(droneBattery, 0f, 100f);
        droneUIManager.SetDroneBattery(droneBattery);
    }

    public void ChargeBattery()
    {
        float chargeTime = droneScriptable.droneBatteryChargingTime * (droneBattery / 100f);
        batteryLeenTween = LeanTween.value(droneBattery, 100f, chargeTime)
            .setOnUpdate(val => droneBattery = val)
            .setOnComplete(() => batteryLeenTween = null);
    }
}
