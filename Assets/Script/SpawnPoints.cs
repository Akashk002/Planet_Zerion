using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField] private List<SpawnPointData> spawnPoints = new List<SpawnPointData>();
    private List<SpawnPointData> tempSpawnPoints = new List<SpawnPointData>();
    private bool DetectEnemySpawnSignal;

    private void Start()
    {
        SetTempSpawnPointsList();
    }

    private void SetTempSpawnPointsList()
    {
        tempSpawnPoints = spawnPoints;
    }

    public SpawnPointData GetspawnPoint()
    {
        if (tempSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points available. Resetting the temporary list.");
            SetTempSpawnPointsList();
        }

        int randomIndex = Random.Range(0, tempSpawnPoints.Count);
        SpawnPointData spawnPointData = tempSpawnPoints[randomIndex];
        tempSpawnPoints.RemoveAt(randomIndex);

        if (GameService.Instance.droneService.GetDroneControllerByType(DroneType.SecurityDrone).GetDroneState() == DroneState.Surveillance)
        {
            UIManager.Instance.minimapIconPanel.StartBlink(randomIndex);
        }

        return spawnPointData;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnPointData spawnPoint = GetspawnPoint();
            GameService.Instance.enemySpaceCraftService.CreateEnemySpaceCraft(EnemySpaceCraftType.Destroyer_1, spawnPoint.initialTransform.position, spawnPoint.targetTransform.position);
        }
    }
}

[System.Serializable]
public class SpawnPointData
{
    public Transform initialTransform;
    public Transform targetTransform;
}