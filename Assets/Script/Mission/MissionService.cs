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

        UnlockBuildings();
        UnlockSpacecrafts();

        // Show panel
        UIManager.Instance.ShowMissionStartPanel(currentMission.missionName, currentMission.missionDescription);
    }

    public void Update()
    {
        if (!missionStarted || missionCompleted)
            return;

        if (!spawningStarted)
        {
            startTimer -= Time.deltaTime;
            if (startTimer <= 0f)
            {
                spawningStarted = true;
            }
        }
        else
        {
            HandleEnemySpawning();
            CheckMissionCompletion();
        }
    }

    private void HandleEnemySpawning()
    {
        spawnTimer += Time.deltaTime;
        if (currentEnemyIndex < currentMission.enemySpaceCraftSpawnList.Count &&
            spawnTimer >= currentMission.spawnRate)
        {
            var enemyType = currentMission.enemySpaceCraftSpawnList[currentEnemyIndex];

            SpawnPointData spawnPoint = GameService.Instance.spawnPoints.GetspawnPoint();

            EnemySpaceCraftController enemySpaceCraftController = GameService.Instance.enemySpaceCraftService.CreateEnemySpaceCraft(enemyType, spawnPoint.initialTransform.position, spawnPoint.targetTransform.position);
            if (enemySpaceCraftController != null)
            {
                enemySpaceCraftControllers.Add(enemySpaceCraftController);
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

            if (currentMissionIndex >= missionDatas.Count - 1)
            {
                UIManager.Instance.ShowGameCompletePanelWithDelay();
                return;
            }
            UIManager.Instance.ShowMissionCompletePanelWithDelay();
        }
    }

    private bool CheckAllEnemyDied()
    {
        for (int i = enemySpaceCraftControllers.Count - 1; i >= 0; i--)
        {
            if (enemySpaceCraftControllers[i].IsDead())
            {
                enemySpaceCraftControllers.RemoveAt(i);
            }
        }
        return enemySpaceCraftControllers.Count == 0;
    }

    public void StartNextMission()
    {
        GameService.Instance.missileService.DeactivateAllMissile();
        currentMissionIndex++;
        LoadMission(currentMissionIndex);
    }

    private void UnlockBuildings()
    {
        foreach (var buildingTypes in currentMission.buildingUnlockList)
        {
            GameService.Instance.buildingManager.SetBuildingAvilable(buildingTypes);
        }
    }

    private void UnlockSpacecrafts()
    {
        foreach (var spacecraftType in currentMission.spacecraftUnlockList)
        {
            SpacecraftScriptable spacecraftScriptable = GameService.Instance.GetSpacecraftDatas().Find(x => x.spacecraftScriptable.spacecraftType == spacecraftType).spacecraftScriptable;
            spacecraftScriptable.spacecraftStatus = SpacecraftStatus.Locked;
        }
    }
}
