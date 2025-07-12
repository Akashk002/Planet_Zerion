using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<Building> TargetBuildings = new List<Building>();
    [SerializeField] private List<BuildingData> BuildingDatas = new List<BuildingData>();
    private int buildingsDamage = 0;
    private int BuildingMaxDamage = 2000;

    public void AddDamage(int val)
    {
        buildingsDamage += val;

        int percentage = buildingsDamage / 10;
        UIManager.Instance.ShowWarning(percentage);

        if (buildingsDamage >= BuildingMaxDamage)
        {
            UIManager.Instance.ShowMissionFailedPanelWithDelay();
        }
    }

    public Vector3 GetRandomBuildingPos()
    {
        int randomIndex = Random.Range(0, TargetBuildings.Count);
        return TargetBuildings[randomIndex].transform.position;
    }

    public List<BuildingData> GetBuildingDatas()
    {
        return BuildingDatas;
    }

    public bool CheckedBuildingUnlocked(BuildingType entranceBuildingType)
    {
        return BuildingDatas.Exists(data => data.buildingType == entranceBuildingType && data.buildingScriptable.buildingState == BuildingState.Unlocked);
    }

    public void SetBuildingAvilable(BuildingType buildingType)
    {
        BuildingData data = BuildingDatas.Find(d => d.buildingType == buildingType);
        if (data != null)
        {
            data.buildingScriptable.buildingState = BuildingState.Locked;
        }
    }
}
