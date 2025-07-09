using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SpaceCraftSelectionHandler : MonoBehaviour
{
    private int SpaceCraftIndex = 0;
    private SpacecraftScriptable spacecraftScriptable;

    [Header("SpaceCraft Data")]
    [SerializeField] private Image spacecraftImage;
    [SerializeField] private TextMeshProUGUI spacecraftName;
    [SerializeField] private SliderAndText maxSpeed;
    [SerializeField] private SliderAndText maxRange;
    [SerializeField] private SliderAndText maxCapacity;

    [Header("Missile Data")]
    [SerializeField] private Image missileImage;
    [SerializeField] private TextMeshProUGUI missileName;
    [SerializeField] private SliderAndText speed;
    [SerializeField] private SliderAndText Damage;

    [Header("Selection data")]
    [SerializeField] private Button selectButton;
    [SerializeField] TextMeshProUGUI spaceCraftStatus;
    [SerializeField] private GameObject costPanel;
    [SerializeField] TextMeshProUGUI rockCount;

    private List<SpacecraftData> spacecraftData;
    private List<MissileData> missileData;

    void Start()
    {
        selectButton.onClick.AddListener(OnSelectButtonClicked);
        spacecraftData = GameService.Instance.GetSpacecraftDatas();
        missileData = GameService.Instance.GetMissileDatas();
        SetCurrentSpacecraftScriptable();
        SetSpaceCraftData();
    }

    private void SetCurrentSpacecraftScriptable()
    {
        spacecraftScriptable = spacecraftData[SpaceCraftIndex].spacecraftScriptable;
    }

    public void OnNextButtonClicked()
    {
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        SpaceCraftIndex++;
        if (SpaceCraftIndex >= spacecraftData.Count)
        {
            SpaceCraftIndex = 0;
        }
        SetCurrentSpacecraftScriptable();
        SetSpaceCraftData();
    }

    public void OnPreviousButtonClicked()
    {
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        SpaceCraftIndex--;
        if (SpaceCraftIndex < 0)
        {
            SpaceCraftIndex = spacecraftData.Count - 1;
        }
        SetCurrentSpacecraftScriptable();
        SetSpaceCraftData();
    }


    private void SetSpaceCraftData()
    {
        spacecraftImage.sprite = spacecraftScriptable.spacecraftSprite;
        spacecraftName.text = spacecraftScriptable.spacecraftType.ToString();
        maxSpeed.SetValue((int)spacecraftScriptable.maxSpeed);
        maxRange.SetValue(spacecraftScriptable.maxRange);
        maxCapacity.SetValue(spacecraftScriptable.missileCapacity);

        SetSelectButtonText();
        SetMissileData(spacecraftScriptable.missileType);
    }

    private void SetMissileData(MissileType missileType)
    {
        MissileScriptable missileData = this.missileData.Find(data => data.missileType == missileType).missileScriptable;
        if (missileData != null)
        {
            missileImage.sprite = missileData.spacecraftSprite;
            missileName.text = missileData.missileType.ToString();
            speed.SetValue((int)missileData.moveSpeed);
            Damage.SetValue((int)missileData.damage);
        }
    }

    private void SetSelectButtonText()
    {
        if (spacecraftScriptable.spacecraftStatus == SpacecraftStatus.Locked)
        {
            spaceCraftStatus.enabled = false;
            costPanel.SetActive(true);
            rockCount.text = spacecraftData[SpaceCraftIndex].spacecraftScriptable.rocksRequire.ToString();
        }
        else
        {
            spaceCraftStatus.enabled = true;
            costPanel.SetActive(false);
        }

        spaceCraftStatus.SetText(spacecraftScriptable.spacecraftStatus.ToString());
    }

    private bool CanPuchase()
    {
        int playerRockCount = GameService.Instance.playerController.GetTotalRock();
        int rockRequire = spacecraftScriptable.rocksRequire;

        if (playerRockCount >= rockRequire)
        {
            return true;
        }

        return true;
        // return false;
    }
    private void SpendRock()
    {
        int rockRequire = spacecraftScriptable.rocksRequire;
        GameService.Instance.playerController.SpendRock(rockRequire);
    }

    private void Select()
    {
        foreach (SpacecraftData spacecraftData in spacecraftData)
        {
            if (spacecraftData.spacecraftScriptable.spacecraftStatus == SpacecraftStatus.Selected)
            {
                spacecraftData.spacecraftScriptable.spacecraftStatus = SpacecraftStatus.Unlocked;
            }
        }
        spacecraftScriptable.spacecraftStatus = SpacecraftStatus.Selected;
        SetSpaceCraftData();
    }

    private void OnSelectButtonClicked()
    {
        if (spacecraftScriptable.spacecraftStatus == SpacecraftStatus.Unlocked || spacecraftScriptable.spacecraftStatus == SpacecraftStatus.Locked && CanPuchase())
        {
            Select();
            SpendRock();
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.Select, transform.position);
        }
    }

    public void FlySpaceCraft()
    {
        SpacecraftScriptable spacecraftScriptable = spacecraftData.Find(data => data.spacecraftScriptable.spacecraftStatus == SpacecraftStatus.Selected).spacecraftScriptable;

        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        GameService.Instance.spacecraftService.CreateSpacecraft(spacecraftScriptable);
        GameService.Instance.spacecraftService.GetSpacecraftController().Activate();
        gameObject.SetActive(false);
    }
}
[System.Serializable]
public struct SliderAndText
{
    public Slider slider;
    public TextMeshProUGUI text;

    public void SetValue(int value, int maxValue = -1)
    {
        if (maxValue != -1)
        {
            slider.maxValue = value;
        }
        slider.value = value;
        text.text = value.ToString();
    }
}