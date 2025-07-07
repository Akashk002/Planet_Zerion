using System.Collections;
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

    private void Start()
    {
        bagPackButton.onClick.AddListener(ToggleBagPack);
        takeRestButton.onClick.AddListener(TakeRest);
    }

    public void SetRockCount(RockType rockType, int Val)
    {
        RockUIData rockUIData = rockUIDatas.Find(data => data.rockType == rockType);
        rockUIData.SetText(Val);
    }

    public void UpdateRockCount()
    {
        List<RockData> rockDatas = GameService.Instance.playerController.GetRockDatas();
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
        GameService.Instance.playerController.TakeRest();
    }

    public void ToggleBagPack()
    {
        if (stoneInfoPanel != null)
        {
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
            stoneInfoPanel.gameObject.SetActive(!stoneInfoPanel.gameObject.activeSelf);
            bagPackButton.targetGraphic.color = stoneInfoPanel.gameObject.activeSelf ? new Color(1, 1, 1, 0.5f) : Color.white;
            bagPackText.text = stoneInfoPanel.gameObject.activeSelf ? "Click to drop the bag" : "Click to get the bag";
            GameService.Instance.playerController.CarryBagPack();
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

