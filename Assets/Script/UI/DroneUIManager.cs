using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DroneUIManager : MonoBehaviour
{
    [SerializeField] private List<RockUIData> rockUIDatas = new List<RockUIData>();
    [SerializeField] private GridLayoutGroup stoneInfoPanel;
    [SerializeField] private Button switchDroneButton;
    [SerializeField] private Button takeRocksButton;
    [SerializeField] private Button ToggleSurveillanceButton;
    [SerializeField] private TextMeshProUGUI switchDroneButtonText;
    [SerializeField] private TextMeshProUGUI ToggleSurveillanceButtonText;
    [SerializeField] private TextMeshProUGUI altitudeText;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private string altitudeWarning;
    [SerializeField] private string takeRockWarning;
    [SerializeField] private Slider batterySlider;
    [SerializeField] private int SurveillanceAltitude = 200;
    private DroneController currentDroneController;
    private int altitude = 0;

    private void Start()
    {
        switchDroneButton.onClick.AddListener(SwitchDrone);
        ToggleSurveillanceButton.onClick.AddListener(ToggleSurveillanceMode);
        takeRocksButton.onClick.AddListener(TakeRocks);
        SetDroneController();
    }

    private void SwitchDrone()
    {
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        GameService.Instance.droneService.SwitchDrone();

        SetDroneController();

        bool isDroneSeurityDrone = currentDroneController.GetDronetype() == DroneType.SecurityDrone;

        ToggleSurveillanceButton.gameObject.SetActive(isDroneSeurityDrone);
        takeRocksButton.gameObject.SetActive(!isDroneSeurityDrone);
        stoneInfoPanel.gameObject.SetActive(!isDroneSeurityDrone);
        UpdateSwitchDroneButtonText(isDroneSeurityDrone);

        SetAltitude((int)currentDroneController.GetAltitude());
        SetDroneBattery(currentDroneController.GetBattery());
    }

    public void ToggleSurveillanceMode()
    {
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.ClickButton, transform.position);
        if (altitude >= SurveillanceAltitude)
        {
            currentDroneController.ToggleDroneSurveillanceState();
            UpdateToggleSurveillanceButtonText();
        }
        else
        {
            EnableWarningText(altitudeWarning);
        }
    }

    public void UpdateRockCount()
    {
        List<RockData> rockDatas = currentDroneController.GetRockDatas();
        foreach (var rockData in rockDatas)
        {
            RockUIData rockUIData = rockUIDatas.Find(data => data.rockType == rockData.RockType);
            rockUIData.SetText(rockData.rockCount);
        }
    }

    public void TakeRocks()
    {
        if (currentDroneController.nearDroneControlRoom)
        {
            List<RockData> rockDatas = currentDroneController.GetRockDatas();
            GameService.Instance.playerController.TakeRocks(rockDatas);
            UpdateRockCount();
        }
        else
        {
            EnableWarningText(takeRockWarning);
        }
    }

    private void SetDroneController()
    {
        currentDroneController = GameService.Instance.droneService.GetCurrentDroneController();
    }

    private void UpdateSwitchDroneButtonText(bool isDroneSeurityDrone)
    {
        switchDroneButtonText.SetText((isDroneSeurityDrone) ? "Switch to Carrier Drone" : "Switch to Security Drone");
    }
    private void UpdateToggleSurveillanceButtonText()
    {
        DroneState droneState = currentDroneController.GetDroneState();
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

    public void SetAltitude(int val)
    {
        altitude = val;
        altitudeText.text = val.ToString();
    }
}
