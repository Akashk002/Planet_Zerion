using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoHandler : MonoBehaviour
{
    [SerializeField] private List<InstructionData> InstructionList = new List<InstructionData>();

    [Header("Instruction Popup")]
    [SerializeField] private TextMeshProUGUI objectnameText;
    [SerializeField] private TextMeshProUGUI instructionsText;
    [SerializeField] private float objectNameDisplayDuration;

    private Coroutine instructionCoroutine;
    private Coroutine ObjectNameCoroutine;

    public void ShowInstruction(InstructionType type)
    {
        stopCoroutine(instructionCoroutine);
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ShowInstruction, transform.position);
        instructionCoroutine = StartCoroutine(setInstructions(type));
    }

    public void HideTextPopup()
    {
        hideInstructionPopup();
        hideObjectNamePopup();
    }

    private IEnumerator setInstructions(InstructionType instructionType)
    {
        InstructionSciprtableObject instruction = InstructionList.Find(i => i.instructionType == instructionType).instructionSciprtableObject;

        yield return new WaitForSeconds(instruction.WaitToTriggerDuration);

        showInstructionPopup(instruction);

        yield return new WaitForSeconds(instruction.DisplayDuration);
        hideInstructionPopup();
    }


    private void hideInstructionPopup()
    {
        instructionsText.SetText(string.Empty);
        instructionsText.gameObject.SetActive(false);
        stopCoroutine(instructionCoroutine);
    }

    private void showInstructionPopup(InstructionSciprtableObject instruction)
    {

        instructionsText.SetText(instruction.Instruction);
        instructionsText.gameObject.SetActive(true);
    }

    private void stopCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void ShowObjectName(string ObjectName)
    {
        stopCoroutine(ObjectNameCoroutine);
        ObjectNameCoroutine = StartCoroutine(setObjectname(ObjectName));

    }

    private IEnumerator setObjectname(string objectname)
    {
        ShowObjectNamePopup(objectname);
        yield return new WaitForSeconds(objectNameDisplayDuration);
        hideObjectNamePopup();
    }

    private void ShowObjectNamePopup(string objectname)
    {
        objectnameText.SetText(objectname);
        objectnameText.gameObject.SetActive(true);
    }

    private void hideObjectNamePopup()
    {
        objectnameText.SetText(string.Empty);
        objectnameText.gameObject.SetActive(false);
        stopCoroutine(ObjectNameCoroutine);
    }
}