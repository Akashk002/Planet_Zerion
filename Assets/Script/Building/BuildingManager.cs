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

        //Debug.Log($"Building damage added: {val}. Total damage: {buildingsDamage}");

        if (buildingsDamage >= 1000)
        {
            Debug.LogWarning("Buildings are damaged beyond repair!");
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
}
