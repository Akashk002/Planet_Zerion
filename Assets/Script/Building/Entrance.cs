using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour, IInteractable
{
    [SerializeField] private BuildingType entranceBuildingType;
    [SerializeField] private GameObject roomPanel;
    private bool playerEntered;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && playerEntered && roomPanel.activeSelf)
        {
            ExitRoom();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerGameObject triggerGameObject;
        if (roomPanel && other.TryGetComponent(out triggerGameObject) && triggerGameObject.triggerByEntrance)
        {
            UIManager.Instance.GetInfoHandler().ShowInstruction(InstructionType.EnterRoom);
        }
    }

    public void Interact()
    {
        UIManager.Instance.GetInfoHandler().HideTextPopup();
        if (roomPanel && !playerEntered && !roomPanel.activeSelf)
        {
            EnterRoom();
        }
    }
    public string GetName()
    {
        return SplitCamelCase(entranceBuildingType + " Entrance");
    }

    public BuildingType GetEntranceBuildingType()
    {
        return entranceBuildingType;
    }

    public static string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(
            input,
            "(\\B[A-Z])",
            " $1"
        );
    }

    private bool CheckedBuildingUnlocked()
    {
        if (GameService.Instance.buildingManager.CheckedBuildingUnlocked(entranceBuildingType))
        {
            return true;
        }

        return false;
    }

    public void EnterRoom()
    {
        if (CheckedBuildingUnlocked())
        {

            roomPanel.SetActive(true);
            GameService.Instance.playerController.Deactivate();
            playerEntered = true;
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.EnterRoom, transform.position);

            if (entranceBuildingType == BuildingType.DroneControlRoom)
            {
                UIManager.Instance.ShowPanel(PanelType.Drone);
                GameService.Instance.droneService.StartDrone(DroneType.CarrierDrone);
            }
        }
        else
        {
            UIManager.Instance.GetInfoHandler().ShowInstruction(InstructionType.Roomlocked);
        }
    }

    public void ExitRoom()
    {
        roomPanel.SetActive(false);
        GameService.Instance.playerController.Activate();
        playerEntered = false;
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ExitRoom, transform.position);

        if (entranceBuildingType == BuildingType.DroneControlRoom)
        {
            GameService.Instance.droneService.StopDrone();
        }
    }
}