using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<Building> TargetBuildings = new List<Building>();
    [SerializeField] private List<BuildingData> BuildingDatas = new List<BuildingData>();
    private int buildingsDamage = 0;

    public void AddDamage(int val)
    {
        buildingsDamage += val;

        int percentage = buildingsDamage / 10;
        UIManager.Instance.ShowWarning(percentage);

        if (buildingsDamage >= 1000)
        {
            UIManager.Instance.ShowMissionFailedPanel();
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            Debug.Log($"Building {buildingType} unlocked.");
        }
    }
}
