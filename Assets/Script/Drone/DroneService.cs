using System.Collections.Generic;
using UnityEngine;

public class DroneService
{
    private Dictionary<DroneType, DroneController> droneControllers = new Dictionary<DroneType, DroneController>();
    private List<DroneData> droneDatas = new List<DroneData>();
    public DroneController currentDroneController;

    public DroneService(List<DroneData> droneDatas)
    {
        this.droneDatas = droneDatas;
        CreateAllDrone();
    }

    public void CreateAllDrone()
    {
        foreach (DroneData droneData in droneDatas)
        {
            DroneModel droneModel = new DroneModel(droneData.droneScriptable);
            DroneController droneController = new DroneController(droneModel);
            droneController.Configure();
            droneControllers.Add(droneData.droneType, droneController);
        }
    }

    public void StartDrone(DroneType droneType)
    {
        currentDroneController = droneControllers[droneType];
        currentDroneController.Activate();
    }

    public void SwitchDrone()
    {
        currentDroneController.Deactivate();
        DroneType otherDroneType = (currentDroneController.GetDronetype() == DroneType.CarrierDrone) ? DroneType.SecurityDrone : DroneType.CarrierDrone;
        StartDrone(otherDroneType);
    }

    public DroneController GetDroneControllerByType(DroneType droneType)
    {
        if (droneControllers.ContainsKey(droneType))
        {
            return droneControllers[droneType];
        }
        return null;
    }

    public void StopDrone()
    {
        if (currentDroneController != null)
        {
            currentDroneController.Deactivate();
            currentDroneController = null;
        }
    }

    public DroneController GetCurrentDroneController()
    {
        return currentDroneController;
    }
}
