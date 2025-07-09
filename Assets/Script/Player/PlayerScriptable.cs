using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerScriptableObject")]
public class PlayerScriptable : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public float rotationSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public List<RockData> rockDatas = new List<RockData>();
    public float maxTiredness = 60;
    public float tiredness;
    public float tirednessIncRate;
    public int RockStorageCapacity;
    public float tirednessRecoverTime = 10f; // How fast tiredness recovers
}

[System.Serializable]

public class RockData
{
    public RockType RockType;
    public int rockCount;

    public void AddRock(int val = 1)
    {
        rockCount += val;
    }

    public void SpendRock(int val)
    {
        if (val >= rockCount)
        {
            rockCount -= val;
        }
    }

    public bool CanSpend(int val)
    {
        return val >= rockCount;
    }
}

public enum RockType
{
    Volcanis,
    Xylora,
    Cryon,
    Droska,
    Zenthra,
    Prime
}