using UnityEngine;

public class SpacecraftModel
{
    private SpacecraftScriptable spacecraftScriptable;
    public SpacecraftView spacecraftPrefab;
    public SpaceCraftUIManager spaceCraftUIManager;
    private float range;
    public float maxSpeed;
    public float accelerationSpeed;
    public float brakeSpeed;
    public MissileType missileType;
    public float verticalSpeed;
    public float rotationSpeed;
    public float mouseSensitivity;
    public float maxPitch;
    public int missileCapacity;
    public int missileCount;
    public int maxRange;

    public SpacecraftModel(SpacecraftScriptable spacecraftScriptable)
    {
        this.spacecraftScriptable = spacecraftScriptable;
        verticalSpeed = spacecraftScriptable.verticalSpeed;
        rotationSpeed = spacecraftScriptable.rotationSpeed;
        mouseSensitivity = spacecraftScriptable.mouseSensitivity;
        maxPitch = spacecraftScriptable.maxPitch;
        maxRange = spacecraftScriptable.maxRange;
        missileType = spacecraftScriptable.missileType;
        spacecraftPrefab = spacecraftScriptable.spacecraftView;
        missileCapacity = spacecraftScriptable.missileCapacity;
        accelerationSpeed = spacecraftScriptable.accelerationSpeed;
        brakeSpeed = spacecraftScriptable.brakeSpeed;
        maxSpeed = spacecraftScriptable.maxSpeed;
        range = maxRange;
        missileCount = missileCapacity; // Initialize with full capacity
        spaceCraftUIManager = UIManager.Instance.spacecraftPanel;
    }

    public void SetRange(float value)
    {
        range -= Time.deltaTime * value * 0.01f;
        Mathf.Clamp(range, 0f, spacecraftScriptable.maxRange);
        spaceCraftUIManager.SetRangeRemaining((int)range);

        if (range <= 0)
        {
            UIManager.Instance.ShowMissionFailedPanelWithDelay();
        }
    }

    public void Refuel()
    {
        range = spacecraftScriptable.maxRange;
        spaceCraftUIManager.SetRangeRemaining((int)range);
    }

    public void ReloadMissile()
    {
        missileCount = spacecraftScriptable.missileCapacity;
        SetMissileCount(missileCount);
    }

    public void SetMissileCount(int missileCount)
    {
        spaceCraftUIManager.SetMissileCount(missileCount);
    }

    public void SetAltitude(int y)
    {
        spaceCraftUIManager.SetAltitude(y);
    }

    public void SetSpeed(int moveSpeed)
    {
        spaceCraftUIManager.SetSpeed(moveSpeed);
    }
}
