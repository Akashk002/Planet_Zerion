using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    private PlayerScriptable playerScriptable;
    private PlayerUIManager playerUIManager;
    public LTDescr tireNessleenTween;
    public List<RockData> rockDatas;
    public float walkSpeed;
    public float runSpeed;
    public float rotationSmoothTime;
    public float tiredness;
    public float gravity;


    public PlayerModel(PlayerScriptable playerScriptable)
    {
        this.playerScriptable = playerScriptable;
        playerUIManager = UIManager.Instance.playerPanel;
        walkSpeed = playerScriptable.walkSpeed;
        runSpeed = playerScriptable.runSpeed;
        rotationSmoothTime = playerScriptable.rotationSmoothTime;
        tiredness = playerScriptable.tiredness;
        gravity = playerScriptable.gravity;
        rockDatas = playerScriptable.rockDatas;
        ResetData();
    }

    public void AddRock(RockType rockType, Vector3 position)
    {
        if (GetTotalRock() < playerScriptable.RockStorageCapacity)
        {
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.CollectRock, position);
            RockData rockData = playerScriptable.rockDatas.Find(r => r.RockType == rockType);
            rockData.AddRock();
            playerUIManager.UpdateRockCount();
        }
        else
        {
            UIManager.Instance.GetInfoHandler().ShowInstruction(InstructionType.StorageFull);
        }
    }

    public void SetTiredness(float val)
    {
        playerScriptable.tiredness += val * playerScriptable.tirednessIncRate;
        Mathf.Clamp(playerScriptable.tiredness, 0, playerScriptable.maxTiredness);
        playerUIManager.SetTiredness(playerScriptable.tiredness, playerScriptable.maxTiredness);
    }

    public void TakeRest()
    {
        float tirednessRecoverTime = playerScriptable.tirednessRecoverTime * (playerScriptable.maxTiredness / playerScriptable.maxTiredness);
        tireNessleenTween = LeanTween.value(playerScriptable.tiredness, 0, tirednessRecoverTime).setOnUpdate((float val) =>
        {
            playerScriptable.tiredness = val;
            playerUIManager.SetTiredness(playerScriptable.tiredness, playerScriptable.maxTiredness);
        }).setOnComplete(() => tireNessleenTween = null);
    }

    public void SpendRock(int rockRequire)
    {
        int remaining = rockRequire;

        foreach (var rockData in playerScriptable.rockDatas)
        {
            if (remaining <= 0) break;

            int spendAmount = Mathf.Min(rockData.rockCount, remaining);
            rockData.SpendRock(spendAmount);
            remaining -= spendAmount;

            playerUIManager.UpdateRockCount();
        }

        if (remaining > 0)
        {
            Debug.LogWarning("Not enough total rocks to spend the required amount!");
            // Optionally: rollback if partial spending is not allowed
        }
    }

    public int GetTotalRock()
    {
        int totalCount = 0;
        foreach (var rockData in playerScriptable.rockDatas)
        {
            totalCount += rockData.rockCount;
        }
        return totalCount;
    }

    public void TakeRocks(List<RockData> droneRocks)
    {
        int currentTotal = GetTotalRock();

        foreach (var droneRock in droneRocks)
        {
            var playerRock = playerScriptable.rockDatas.Find(r => r.RockType == droneRock.RockType);
            if (playerRock == null) continue;

            int spaceLeft = playerScriptable.RockStorageCapacity - currentTotal;
            if (spaceLeft <= 0) return; // Player inventory full

            int transferableAmount = Mathf.Min(droneRock.rockCount, spaceLeft);

            playerRock.AddRock(transferableAmount);
            droneRock.SpendRock(transferableAmount);
            currentTotal += transferableAmount;
        }

        playerUIManager.UpdateRockCount();
    }

    public void ResetData()
    {
        playerScriptable.tiredness = 0f;
        foreach (var rockData in playerScriptable.rockDatas)
        {
            rockData.rockCount = 0;
        }
    }

}
