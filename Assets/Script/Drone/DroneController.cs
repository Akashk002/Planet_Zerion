using Unity.VisualScripting;
using UnityEngine;

public class DroneController
{
    private float moveSpeed = 0f;
    private float yaw, pitch;
    private bool isRotating = false;

    private DroneView droneView;
    private DroneScriptable droneScriptable;
    private Transform initialPosition;
    private DroneState droneState;

    private LTDescr batteryLeenTween;
    public bool IsInteracted;
    private AudioSource audioSource;
    public DroneController(DroneScriptable droneScriptable)
    {
        this.droneScriptable = droneScriptable;
        this.droneView = Object.Instantiate(droneScriptable.droneView);
        droneView.SetController(this);
    }

    public void Configure()
    {
        initialPosition = droneView.transform;
        droneView.rb.freezeRotation = true;

        Vector3 euler = droneView.transform.eulerAngles;
        yaw = euler.y;
        pitch = euler.x;

        droneState = DroneState.Deactivate;

        //audioSource = droneView.AddComponent<AudioSource>();
    }

    public void Update()
    {
        if (droneState == DroneState.Activate)
            HandleMovement();

        if (GetDronetype() == DroneType.SecurityDrone && droneState == DroneState.Surveillance)
            PerformSurveillance();

        Interact(); // for non-surveillance drones
    }

    private void HandleMovement()
    {
        Vector3 moveDir = Vector3.zero;
        isRotating = false;

        bool forward = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool up = Input.GetKey(KeyCode.A);
        bool down = Input.GetKey(KeyCode.D);

        if (forward || backward)
        {
            float direction = forward ? 1 : -1;
            moveSpeed += droneScriptable.AccelerationSpeed;
            moveSpeed = Mathf.Clamp(moveSpeed, 0, droneScriptable.maxSpeed);
            moveDir += droneView.transform.forward * moveSpeed * direction;

            SetBattery(moveSpeed);
            isRotating = true;
        }
        else
        {
            moveSpeed = 0;
        }

        if (up) moveDir += droneView.transform.up * droneScriptable.verticalSpeed;
        if (down) moveDir -= droneView.transform.up * droneScriptable.verticalSpeed;

        droneView.rb.velocity = moveDir;

        if (moveDir != Vector3.zero)
        {
            if (audioSource == null || !audioSource.isPlaying)
            {
                audioSource = GameService.Instance.audioManager.PlayLoopingAt(GameAudioType.DroneMoving, droneView.transform.position, 1);
            }
            else
                audioSource.transform.position = droneView.transform.position;
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                GameService.Instance.audioManager.StopSound(audioSource);
                audioSource = null;
            }
        }


        if (isRotating || Input.GetMouseButton(0))
            ApplyRotation();

        HandleZoom();
    }

    private void ApplyRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * droneScriptable.rotationSpeed * droneScriptable.mouseSensitivity * Time.fixedDeltaTime;
        pitch -= mouseY * droneScriptable.rotationSpeed * droneScriptable.mouseSensitivity * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, -droneScriptable.maxPitch, droneScriptable.maxPitch);

        Quaternion targetRot = Quaternion.Euler(pitch, yaw, 0f);
        droneView.rb.MoveRotation(targetRot);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            droneView.cam.fieldOfView -= scroll * 10f;
            droneView.cam.fieldOfView = Mathf.Clamp(droneView.cam.fieldOfView, 0f, 75f);
            Debug.Log($"Camera Zoom Scroll: {scroll}, FOV: {droneView.cam.fieldOfView}");
        }
    }

    private void PerformSurveillance()
    {
        Vector3 euler = droneView.transform.eulerAngles;
        euler.x = 0f;
        droneView.transform.eulerAngles = euler;

        droneView.transform.Rotate(Vector3.up * 30f * Time.deltaTime, Space.Self);
    }

    public void Activate()
    {
        if (GetDronetype() == DroneType.SecurityDrone && droneState == DroneState.Surveillance) return;

        droneState = DroneState.Activate;
        droneView.cam.gameObject.SetActive(true);
        UIManager.Instance.droneUIManager.currentDroneScriptable = droneScriptable;
    }

    public void Deactivate()
    {
        if (GetDronetype() == DroneType.SecurityDrone && droneState == DroneState.Surveillance) return;

        droneState = DroneState.Deactivate;
        droneView.cam.gameObject.SetActive(false);
        isRotating = false;
    }

    public void ToggleDroneSurveillanceState()
    {
        if (droneState == DroneState.Surveillance)
        {
            Activate();
        }
        else
        {
            droneState = DroneState.Surveillance;
        }
    }

    public bool CheckSurveillanceState()
    {
        return droneState == DroneState.Surveillance;
    }

    public void Reset()
    {
        droneView.rb.velocity = Vector3.zero;
        droneView.transform.position = initialPosition.position;
        droneView.transform.rotation = initialPosition.rotation;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E)) IsInteracted = true;
        else if (Input.GetKeyUp(KeyCode.E)) IsInteracted = false;
    }

    public void AddRock(RockType rockType)
    {
        if (GetTotalRock() < droneScriptable.RockStorageCapacity)
        {
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.CollectRock, droneView.transform.position);
            RockData rockData = droneScriptable.rockDatas.Find(r => r.RockType == rockType);
            rockData?.AddRock();
            UIManager.Instance.droneUIManager.SetRockCount(rockType, rockData.rockCount);
        }
        else
        {
            UIManager.Instance.GetInfoHandler().ShowInstruction(InstructionType.BagFull);
        }
    }

    private int GetTotalRock()
    {
        int total = 0;
        foreach (var r in droneScriptable.rockDatas)
            total += r.rockCount;
        return total;
    }

    public void SetBattery(float usage)
    {
        droneScriptable.droneBattery -= usage * droneScriptable.droneBatteryDecRate;
        droneScriptable.droneBattery = Mathf.Clamp(droneScriptable.droneBattery, 0f, 100f);
    }

    public void ChargeBattery()
    {
        float chargeTime = droneScriptable.droneBatteryChargingTime * (droneScriptable.droneBattery / 100f);
        batteryLeenTween = LeanTween.value(droneScriptable.droneBattery, 100f, chargeTime)
            .setOnUpdate(val => droneScriptable.droneBattery = val)
            .setOnComplete(() => batteryLeenTween = null);
    }

    public DroneType GetDronetype() => droneScriptable.droneType;
    public DroneScriptable GetDroneScriptable() => droneScriptable;
    public DroneState GetDroneState() => droneState;
}


public enum DroneState
{
    Activate,
    Deactivate,
    Surveillance,
}


