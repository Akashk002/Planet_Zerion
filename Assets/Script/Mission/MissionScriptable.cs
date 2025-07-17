using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionScriptableObject", menuName = "ScriptableObjects/MissionScriptableObject")]
public class MissionScriptable : ScriptableObject
{
    public MissionType missionType;
    public string missionName;
    public string missionDescription;
    public int waveStartDelay;
    public int spawnRate;
    public List<EnemySpaceCraftType> enemySpaceCraftSpawnList;
    public List<SpacecraftType> spacecraftUnlockList;
    public List<BuildingType> buildingUnlockList;
}
[System.Serializable]
public class MissionData
{
    public MissionType missionType;
    public MissionScriptable missionScriptable;
}

public enum MissionType
{
    Mission_1,
    Mission_2,
    Mission_3,
    Mission_4,
    Mission_5,
}
