using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject StoryPanel;
    [SerializeField] private GameObject ControlInfoPanel;
    [SerializeField] private TextMeshProUGUI loadingText;

    public void ShowStory()
    {
        MainMenu.SetActive(false);
        StoryPanel.SetActive(true);
    }
    public void StartGame()
    {
        StoryPanel.SetActive(false);
        loadingText.enabled = true;
        SceneManager.LoadScene("Game");
    }

    public void ShowControl()
    {
        MainMenu.SetActive(false);
        ControlInfoPanel.SetActive(true);
    }

    public void Back()
    {
        MainMenu.SetActive(true);
        StoryPanel.SetActive(false);
        ControlInfoPanel.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
