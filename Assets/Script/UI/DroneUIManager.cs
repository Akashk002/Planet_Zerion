using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor;

public class DroneUIManager : MonoBehaviour
{
    [SerializeField] private List<RockUIData> rockUIDatas = new List<RockUIData>();
    [SerializeField] private GridLayoutGroup stoneInfoPanel;
    [SerializeField] private Button switchDroneButton;
    [SerializeField] private TextMeshProUGUI switchDroneButtonText;
    [SerializeField] private Button ToggleSurveillanceButton;
    [SerializeField] private TextMeshProUGUI ToggleSurveillanceButtonText;
    [SerializeField] private Slider batterySlider;
    [SerializeField] private Button takeRocksButton;
    [SerializeField] private TextMeshProUGUI altitudeText;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private string altitudeWarning;
    [SerializeField] private string takeRockWarning;
    private DroneScriptable currentDroneScriptable;
    private int altitude = 0;
    public int SurveillanceAltitude = 200;

    private void Start()
    {
        switchDroneButton.onClick.AddListener(SwitchDrone);
        ToggleSurveillanceButton.onClick.AddListener(ToggleSurveillanceMode);
        takeRocksButton.onClick.AddListener(TakeRocks);
    }

    public void SetDroneScriptable(DroneScriptable droneScriptable)
    {
        currentDroneScriptable = droneScriptable;
    }

    public void UpdateRockCount()
    {
        List<RockData> rockDatas = currentDroneScriptable.rockDatas;
        foreach (var rockData in rockDatas)
        {
            RockUIData rockUIData = rockUIDatas.Find(data => data.rockType == rockData.RockType);
            rockUIData.SetText(rockData.rockCount);
        }
    }

    private void SwitchDrone()
    {
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        GameService.Instance.droneService.SwitchDrone();
        ToggleSurveillanceButton.gameObject.SetActive(currentDroneScriptable.droneType == DroneType.SecurityDrone);
        takeRocksButton.gameObject.SetActive(currentDroneScriptable.droneType != DroneType.SecurityDrone);
        stoneInfoPanel.gameObject.SetActive(currentDroneScriptable.droneType != DroneType.SecurityDrone);
        UpdateSwitchDroneButtonText();
    }

    public void ToggleSurveillanceMode()
    {
        if (altitude >= SurveillanceAltitude)
        {
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
            GameService.Instance.droneService.GetDroneController().ToggleDroneSurveillanceState();
            UpdateToggleSurveillanceButtonText();
        }
        else
        {
            EnableWarningText(altitudeWarning);
        }

    }

    private void UpdateSwitchDroneButtonText()
    {
        switchDroneButtonText.SetText((currentDroneScriptable.droneType != DroneType.CarrierDrone) ? "Switch to Carrier Drone" : "Switch to Security Drone");
    }
    private void UpdateToggleSurveillanceButtonText()
    {
        DroneState droneState = GameService.Instance.droneService.GetDroneController().GetDroneState();
        ToggleSurveillanceButtonText.SetText((droneState != DroneState.Surveillance) ? "Turn On Surveillance Mode" : "Turn Off Surveillance Mode");

    }

    private void EnableWarningText(string str)
    {
        warningText.SetText(str);
        warningText.enabled = true;
        Invoke(nameof(DisableWarningText), 3);
    }

    private void DisableWarningText()
    {
        warningText.enabled = false;
    }

    public void SetDroneBattery(float value)
    {
        batterySlider.maxValue = 100;
        batterySlider.value = value;
    }

    public void TakeRocks()
    {
        if (GameService.Instance.droneService.GetDroneControllerByType(DroneType.CarrierDrone).nearDroneControlRoom)
        {
            List<RockData> rockDatas = currentDroneScriptable.rockDatas;
            GameService.Instance.playerController.TakeRocks(rockDatas);
            UpdateRockCount();
        }
        else
        {
            EnableWarningText(takeRockWarning);
        }
    }

    public void SetAltitude(int val)
    {
        altitude = val;
        altitudeText.text = val.ToString();
    }

}
