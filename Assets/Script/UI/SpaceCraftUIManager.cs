using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpaceCraftUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private TextMeshProUGUI speedTxt;
    [SerializeField] private TextMeshProUGUI maxSpeedTxt;
    [SerializeField] private TextMeshProUGUI rangeRemainingTxt;
    [SerializeField] private TextMeshProUGUI altitudeTxt;
    [SerializeField] private TextMeshProUGUI missileCountTxt;
    [SerializeField] private Button backToRoomBtn;
    [SerializeField] private Button refuelBtn;
    [SerializeField] private Button reloadBtn;
    private int maxMissile;
    private int maxRange;

    private SpacecraftController spacecraftController;

    private void Start()
    {
        backToRoomBtn.onClick.AddListener(BackToRoom);
    }

    private void OnEnable()
    {
        spacecraftController = GameService.Instance.spacecraftService.GetSpacecraftController();
        ShowAndHideBtn(true);
        SetMissileCount(spacecraftController.GetMissileCapacity());
        SetRangeRemaining(spacecraftController.GetMaxRange());
        SetAltitude(0); // Assuming initial altitude is 0
        SetSpeed(0); // Assuming initial speed is 0
        maxSpeedTxt.SetText(spacecraftController.GetMaxSpeed().ToString());
    }

    public void SetMissileCount(int value)
    {
        missileCountTxt.SetText($"{value}/{maxMissile}");
    }

    public void SetAltitude(int height)
    {
        altitudeTxt.SetText(height.ToString());
    }

    public void SetRangeRemaining(int range)
    {
        rangeRemainingTxt.SetText($"{range}/{maxRange}");
    }

    public void SetSpeed(int speed)
    {
        speedTxt.SetText(speed.ToString());
    }

    public void BackToRoom()
    {
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        GameService.Instance.spacecraftService.RemovePreviousSpacecraft();
        UIManager.Instance.SpaceCraftSelectionPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Refuel()
    {
        spacecraftController.Refuel();
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        refuelBtn.interactable = false;
    }
    public void ReloadMissile()
    {
        spacecraftController.ReloadMissile();
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        reloadBtn.interactable = false;
    }

    public void ShowAndHideBtn(bool enable)
    {
        reloadBtn.interactable = true;
        refuelBtn.interactable = true;
        backToRoomBtn.gameObject.SetActive(enable);
        refuelBtn.gameObject.SetActive(enable);
        reloadBtn.gameObject.SetActive(enable);
    }
}
