using System.Collections.Generic;
using UnityEngine;

public class GameService : GenericMonoSingleton<GameService>
{
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerScriptable PlayerScriptable;
    [SerializeField] private List<DroneData> droneDatas = new List<DroneData>();
    [SerializeField] private List<SpacecraftData> spacecraftDatas = new List<SpacecraftData>();
    [SerializeField] private List<EnemySpaceCraftData> enemySpaceCraftDatas = new List<EnemySpaceCraftData>();
    [SerializeField] private List<MissileData> missileDatas = new List<MissileData>();
    [SerializeField] private List<MissionData> missionDatas = new List<MissionData>();
    [SerializeField] private VFXView VFXPrefab;

    public BuildingManager buildingManager;
    public AudioManager audioManager;
    public SpawnPoints spawnPoints;
    public PlayerController playerController { get; private set; }
    public DroneService droneService { get; private set; }
    public SpacecraftService spacecraftService { get; private set; }
    public EnemySpaceCraftService enemySpaceCraftService { get; private set; }
    public MissileService missileService { get; private set; }
    public MissionService missionService { get; private set; }
    public VFXService VFXService { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        missionService = new MissionService(missionDatas);

        PlayerModel playerModel = new PlayerModel(PlayerScriptable);
        playerController = new PlayerController(playerView, playerModel);

        droneService = new DroneService(droneDatas);
        spacecraftService = new SpacecraftService();
        enemySpaceCraftService = new EnemySpaceCraftService(enemySpaceCraftDatas);
        missileService = new MissileService(missileDatas);
        VFXService = new VFXService(VFXPrefab);
    }

    private void Update()
    {
        missionService?.Update();
    }

    public List<SpacecraftData> GetSpacecraftDatas()
    {
        return spacecraftDatas;
    }
    public List<MissileData> GetMissileDatas()
    {
        return missileDatas;
    }
}

