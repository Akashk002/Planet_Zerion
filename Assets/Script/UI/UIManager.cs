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
    public TextMeshProUGUI warningText;

    [Header("Room Panels")]
    public GameObject HomePanel;
    public GameObject DroneControlPanel;
    public GameObject SpaceCraftSelectionPanel;

    [Header("Popups")]
    [SerializeField] private GameObject missionStartPanel;
    [SerializeField] private GameObject missionCompletePanel;
    [SerializeField] private GameObject missionFailedPanel;
    [SerializeField] private GameObject gamePausePanel;
    [SerializeField] private GameObject gameCompletePanel;
    [SerializeField] private GameObject controlInfoPanel;
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
                playerPanel.gameObject.SetActive(true);
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

    public void ShowWarning(int value)
    {
        warningText.enabled = true;
        warningText.SetText($"Base damage level: {value}%");
        Invoke(nameof(HideWarning), 3f);
    }
    public void HideWarning()
    {
        warningText.enabled = false;
    }

    public void ShowMissionStartPanel(string missionName, string missionDesc)
    {
        PlayPauseGame();
        missionStartPanel.SetActive(true);
        this.missionName.SetText(missionName);
        missionDescription.SetText(missionDesc.Replace("\\n", "\n"));
    }

    public void ShowMissionFailedPanelWithDelay(int value = 2)
    {
        Invoke(nameof(ShowMissionFailedPanel), value);
    }

    public void ShowMissionFailedPanel()
    {
        PlayPauseGame();
        missionFailedPanel.SetActive(true);
    }

    public void ShowGameCompletePanelWithDelay(int value = 2)
    {
        Invoke(nameof(ShowGameCompletePanel), value);
    }

    public void ShowGameCompletePanel()
    {
        PlayPauseGame();
        gameCompletePanel.SetActive(true);
    }

    public void ShowMissionCompletePanelWithDelay(int value = 2)
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

    public void showControlInfopanel()
    {
        gamePausePanel.SetActive(false);
        controlInfoPanel.SetActive(true);
    }

    public void HideControlInfoPanel()
    {
        controlInfoPanel.SetActive(false);
        gamePausePanel.SetActive(true);
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