using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionService
{
    private List<MissionData> missionDatas;
    private int currentMissionIndex = 0;
    private MissionScriptable currentMission;
    private float startTimer;
    private float spawnTimer;
    private int currentEnemyIndex = 0;

    private bool missionStarted = false;
    private bool spawningStarted = false;
    private bool missionCompleted = false;
    private List<EnemySpaceCraftController> enemySpaceCraftControllers = new List<EnemySpaceCraftController>();

    public MissionService(List<MissionData> missionDatas)
    {
        this.missionDatas = missionDatas;
        Debug.Log("MissionService initialized with " + missionDatas.Count + " missions.");
        LoadMission(currentMissionIndex);
    }

    private void LoadMission(int index)
    {
        currentMission = missionDatas[index].missionScriptable;
        currentEnemyIndex = 0;
        spawnTimer = 0f;
        missionStarted = true;
        spawningStarted = false;
        missionCompleted = false;
        startTimer = currentMission.waveStartDelay;

        Debug.Log($"Loading Mission: {currentMission.missionName} (Index: {index})");

        UnlockBuildings();
        UnlockSpacecrafts();

        // Show panel
        UIManager.Instance.ShowMissionStartPanel(currentMission.missionName, currentMission.missionDescription);
        Debug.Log($"Mission panel shown: {currentMission.missionName} - {currentMission.missionDescription}");
    }

    public void Update()
    {
        Debug.Log("MissionService Update called.");

        if (!missionStarted || missionCompleted)
            return;

        if (!spawningStarted)
        {
            startTimer -= Time.deltaTime;
            Debug.Log($"Start Timer: {startTimer}");
            if (startTimer <= 0f)
            {
                spawningStarted = true;
                Debug.Log($"Spawning started for mission: {currentMission.missionName}");
            }
        }
        else
        {
            Debug.Log($"Spawning enemies for mission: {currentMission.missionName}, Current Enemy Index: {currentEnemyIndex}");
            HandleEnemySpawning();
            CheckMissionCompletion();
        }
    }

    private void HandleEnemySpawning()
    {
        spawnTimer += Time.deltaTime;

        Debug.Log($"Spawn Timer: {spawnTimer}, Current Enemy Index: {currentEnemyIndex}, Total Enemies: {currentMission.enemySpaceCraftSpawnList.Count}");

        if (currentEnemyIndex < currentMission.enemySpaceCraftSpawnList.Count &&
            spawnTimer >= currentMission.spawnRate)
        {
            Debug.Log($"Spawning enemy: {currentMission.enemySpaceCraftSpawnList[currentEnemyIndex]}");
            var enemyType = currentMission.enemySpaceCraftSpawnList[currentEnemyIndex];

            SpawnPointData spawnPoint = GameService.Instance.spawnPoints.GetspawnPoint();

            EnemySpaceCraftController enemySpaceCraftController = GameService.Instance.enemySpaceCraftService.CreateEnemySpaceCraft(enemyType, spawnPoint.initialTransform.position, spawnPoint.targetTransform.position);
            if (enemySpaceCraftController != null)
            {
                enemySpaceCraftControllers.Add(enemySpaceCraftController);
                Debug.Log($"Enemy spawned: {enemyType} at {spawnPoint.initialTransform.position}");
            }
            else
            {
                Debug.LogWarning($"Failed to spawn enemy: {enemyType}");
            }

            currentEnemyIndex++;
            spawnTimer = 0f;
        }
    }

    private void CheckMissionCompletion()
    {
        if (currentEnemyIndex >= currentMission.enemySpaceCraftSpawnList.Count && CheckAllEnemyDied())
        {
            missionCompleted = true;
            Debug.Log($"Mission completed: {currentMission.missionName}");

            if (currentMissionIndex >= missionDatas.Count - 1)
            {
                Debug.Log("All missions completed. Showing game complete panel.");
                UIManager.Instance.ShowGameCompletePanelWithDelay(3);
                return;
            }
            Debug.Log("Showing mission complete panel.");
            UIManager.Instance.ShowMissionCompletePanelWithDelay(3);
        }
    }

    private bool CheckAllEnemyDied()
    {
        for (int i = enemySpaceCraftControllers.Count - 1; i >= 0; i--)
        {
            if (enemySpaceCraftControllers[i].IsDead())
            {
                Debug.Log($"Enemy destroyed: {enemySpaceCraftControllers[i].enemySpaceCraftType}");
                enemySpaceCraftControllers.RemoveAt(i);
            }
        }
        return enemySpaceCraftControllers.Count == 0;
    }

    public void StartNextMission()
    {
        currentMissionIndex++;
        Debug.Log($"Starting next mission. Index: {currentMissionIndex}");
        LoadMission(currentMissionIndex);
    }

    private void UnlockBuildings()
    {
        foreach (var buildingTypes in currentMission.buildingUnlockList)
        {
            GameService.Instance.buildingManager.SetBuildingAvilable(buildingTypes);
            Debug.Log($"Building unlocked: {buildingTypes}");
        }
    }

    private void UnlockSpacecrafts()
    {
        foreach (var spacecraftType in currentMission.spacecraftUnlockList)
        {
            SpacecraftScriptable spacecraftScriptable = GameService.Instance.GetSpacecraftDatas().Find(x => x.spacecraftScriptable.spacecraftType == spacecraftType).spacecraftScriptable;
            spacecraftScriptable.spacecraftStatus = SpacecraftStatus.Locked;
            Debug.Log($"Spacecraft unlocked: {spacecraftType}");
        }
    }
}
