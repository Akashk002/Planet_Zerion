using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private List<RockUIData> rockUIDatas = new List<RockUIData>();
    [SerializeField] private Slider tirednessSlider;
    [SerializeField] private GridLayoutGroup stoneInfoPanel;
    [SerializeField] private Button bagPackButton;
    [SerializeField] private Button takeRestButton;
    [SerializeField] private TextMeshProUGUI bagPackText;
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        bagPackButton.onClick.AddListener(ToggleBagPack);
        takeRestButton.onClick.AddListener(TakeRest);
        if (playerController == null)
        {
            playerController = GameService.Instance.playerController;
        }
    }

    private void OnEnable()
    {
        takeRestButton.interactable = true;
    }

    public void UpdateRockCount()
    {
        List<RockData> rockDatas = playerController.GetRockDatas();
        foreach (var rockData in rockDatas)
        {
            RockUIData rockUIData = rockUIDatas.Find(data => data.rockType == rockData.RockType);
            rockUIData.SetText(rockData.rockCount);
        }
    }

    public void SetTiredness(float value, float maxValue)
    {
        tirednessSlider.maxValue = maxValue;
        tirednessSlider.value = value;
    }

    public void TakeRest()
    {
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        takeRestButton.interactable = false;
        playerController.TakeRest();
    }

    public void ToggleBagPack()
    {
        if (stoneInfoPanel != null)
        {
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
            stoneInfoPanel.gameObject.SetActive(!stoneInfoPanel.gameObject.activeSelf);
            bagPackButton.targetGraphic.color = stoneInfoPanel.gameObject.activeSelf ? new Color(1, 1, 1, 0.5f) : Color.white;
            bagPackText.text = stoneInfoPanel.gameObject.activeSelf ? "Click to drop the bag" : "Click to get the bag";
            playerController.CarryBagPack();
        }
    }
}

[System.Serializable]
public class RockUIData
{
    public RockType rockType;
    public TMP_Text rockNameText;

    public void SetText(int count)
    {
        rockNameText.SetText(count.ToString());
    }
}

