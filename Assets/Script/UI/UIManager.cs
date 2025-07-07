using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : GenericMonoSingleton<UIManager>
{
    [Header("UI Panels")]
    public PlayerUIManager playerPanel;
    public SpaceCraftUIManager spacecraftPanel;
    public DroneUIManager droneUIManager;
    public InfoHandler infoHandler;
    public MinimapIconFollow minimapIconPanel;

    [Header("Room Panels")]
    public GameObject HomePanel;
    public GameObject DroneControlPanel;
    public GameObject SpaceCraftSelectionPanel;

    [Header("Popups")]
    [SerializeField] private GameObject missionStartPanel;
    [SerializeField] private GameObject missionCompletePanel;
    [SerializeField] private GameObject missionFailedPanel;
    [SerializeField] private GameObject gamePausePanel;
    [SerializeField] private GameObject GameCompletePanel;
    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI missionDescription;

    public void ShowPanel(PanelType panelType)
    {
        playerPanel.gameObject.SetActive(false);
        spacecraftPanel.gameObject.SetActive(false);
        droneUIManager.gameObject.SetActive(false);

        switch (panelType)
        {
            case PanelType.Player:
                playerPanel.gameObject.SetActive(true);
                break;
            case PanelType.Spacecraft:
                spacecraftPanel.gameObject.SetActive(true);
                break;
            case PanelType.Drone:
                droneUIManager.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public InfoHandler GetInfoHandler()
    {
        return infoHandler;
    }

    public void ShowMissionStartPanel(string missionName, string missionDesc)
    {
        PlayPauseGame();
        missionStartPanel.SetActive(true);
        this.missionName.SetText(missionName);
        missionDescription.SetText(missionDesc.Replace("\\n", "\n"));
    }

    public void ShowMissionFailedPanel()
    {
        PlayPauseGame();
        missionFailedPanel.SetActive(true);
    }

    public void ShowGameCompletePanelWithDelay(int value)
    {
        Invoke(nameof(ShowGameCompletePanel), value);
    }

    public void ShowGameCompletePanel()
    {
        PlayPauseGame();
        GameCompletePanel.SetActive(true);
    }

    public void ShowMissionCompletePanelWithDelay(int value)
    {
        Invoke(nameof(ShowMissionCompletePanel), value);
    }

    public void ShowMissionCompletePanel()
    {
        PlayPauseGame();
        missionCompletePanel.SetActive(true);
    }

    public void StartMission()
    {
        PlayPauseGame();
        missionStartPanel.SetActive(false);
    }
    public void NextMission()
    {
        PlayPauseGame();
        GameService.Instance.missionService.StartNextMission();
        missionCompletePanel.SetActive(false);
    }

    public void ShowGamePausePanel()
    {
        PlayPauseGame();
        gamePausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        PlayPauseGame();
        gamePausePanel.SetActive(false);
    }
    public void RestartGame()
    {
        PlayPauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        PlayPauseGame();
        SceneManager.LoadScene("Menu");
    }

    private void PlayPauseGame()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }
}

public enum PanelType
{
    Player,
    Spacecraft,
    Drone,
}