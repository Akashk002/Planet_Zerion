using UnityEngine;

[CreateAssetMenu(fileName = "InstructionSciprtableObject", menuName = "ScriptableObjects/InstructionSciprtableObject", order = 2)]

public partial class InstructionSciprtableObject : ScriptableObject
{
    public InstructionType InstructionType;
    public string Instruction;
    public int DisplayDuration;
    public int WaitToTriggerDuration = 0;
}
[System.Serializable]
public class InstructionData
{
    public InstructionType instructionType;
    public InstructionSciprtableObject instructionSciprtableObject;
}
public enum InstructionType
{
    RockCollect,
    CarryBagpack,
    EnterRoom,
    StorageFull,
    Roomlocked
}

