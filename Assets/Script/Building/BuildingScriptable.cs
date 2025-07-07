using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingScriptableObject", menuName = "ScriptableObjects/BuildingScriptableObject")]
public class BuildingScriptable : ScriptableObject
{
    public BuildingType buildingType;
    public string description;
    public Sprite buildingIcon;
    public BuildingState buildingState;
    public int rockRequire;
}

public enum BuildingState
{
    Locked,
    Unlocked,
    unavailable,
}

[System.Serializable]
public class BuildingData
{
    public BuildingType buildingType;
    public BuildingScriptable buildingScriptable;
}


public enum BuildingType
{
    House,
    MainOffice,
    ReserchCenter,
    SpacecraftTerminal,
    DroneControlRoom,
    DroneChargingStation,
    Reactor,
    Farm,
    MachineBuildingPlant,
    ResourceWareHouse,
    SignalCommandCenter,
    GeothermalGenerator,
    SolarGenerator,
    SolarPanel,
    MyHome,
}