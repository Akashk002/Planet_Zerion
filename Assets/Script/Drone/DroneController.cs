using System.Collections.Generic;
using UnityEngine;

public class DroneController
{
    private float moveSpeed = 0f;
    private float yaw, pitch;
    private bool isRotating = false;

    private DroneView droneView;
    private DroneModel droneModel;
    private Transform initialPosition;
    private DroneState droneState;
    private AudioSource audioSource;
    private bool nearDroneControlRoom;

    public bool IsInteracted;

    public DroneController(DroneModel droneModel)
    {
        this.droneModel = droneModel;
        this.droneView = Object.Instantiate(droneModel.dronePrefab);
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
            moveSpeed += droneModel.AccelerationSpeed;
            moveSpeed = Mathf.Clamp(moveSpeed, 0, droneModel.maxSpeed);
            moveDir += droneView.transform.forward * moveSpeed * direction;

            droneModel.SetBattery(moveSpeed);
            isRotating = true;
        }
        else
        {
            moveSpeed = 0;
        }

        if (up) moveDir += droneView.transform.up * droneModel.verticalSpeed;
        if (down) moveDir -= droneView.transform.up * droneModel.verticalSpeed;

        droneView.rb.velocity = moveDir;

        if (moveDir != Vector3.zero)
        {
            if (audioSource == null || !audioSource.isPlaying)
            {
                audioSource = GameService.Instance.audioManager.PlayLoopingAt(GameAudioType.DroneMoving, droneView.transform.position, 1);
            }
            else
            {
                audioSource.transform.position = droneView.transform.position;
            }

            UIManager.Instance.droneUIManager.SetAltitude((int)droneView.transform.position.y);
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                GameService.Instance.audioManager.StopSound(audioSource);
                audioSource = null;
            }
        }

        if (isRotating)
        {
            ApplyRotation();
        }

        HandleZoom();
    }

    private void ApplyRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * droneModel.rotationSpeed * droneModel.mouseSensitivity * Time.fixedDeltaTime;
        pitch -= mouseY * droneModel.rotationSpeed * droneModel.mouseSensitivity * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, -droneModel.maxPitch, droneModel.maxPitch);

        Quaternion targetRot = Quaternion.Euler(pitch, yaw, 0f);
        droneView.rb.MoveRotation(targetRot);
    }

    public void Activate()
    {
        if (GetDronetype() == DroneType.SecurityDrone && droneState == DroneState.Surveillance) return;

        droneState = DroneState.Activate;
        droneView.cam.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        if (GetDronetype() == DroneType.SecurityDrone && droneState == DroneState.Surveillance) return;

        droneState = DroneState.Deactivate;
        droneView.cam.gameObject.SetActive(false);
        isRotating = false;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            droneView.cam.fieldOfView -= scroll * 10f;
            droneView.cam.fieldOfView = Mathf.Clamp(droneView.cam.fieldOfView, 0f, 75f);
        }
    }

    private void PerformSurveillance()
    {
        Vector3 euler = droneView.transform.eulerAngles;
        euler.x = 0f;
        droneView.transform.eulerAngles = euler;
        droneView.transform.Rotate(Vector3.up * 30f * Time.deltaTime, Space.Self);
    }

    public void ToggleDroneSurveillanceState()
    {
        if (droneState == DroneState.Surveillance)
        {
            droneState = DroneState.Activate;
            Activate();
        }
        else
        {
            droneState = DroneState.Surveillance;
        }
    }

    public void Interact() => IsInteracted = Input.GetKeyDown(KeyCode.E) ? true : (Input.GetKeyUp(KeyCode.E) ? false : IsInteracted);
    public bool CheckDroneNearControlRoom() => nearDroneControlRoom;
    public bool SetDroneNearControlRoom(bool near) => nearDroneControlRoom = near;
    public DroneType GetDronetype() => droneModel.droneType;
    public DroneState GetDroneState() => droneState;
    public float GetAltitude() => droneView.transform.position.y;
    public float GetBattery() => droneModel.droneBattery;
    public List<RockData> GetRockDatas() => droneModel.rockDatas;
    public void AddRock(RockType rockType) => droneModel.AddRock(rockType, droneView.transform.position);
}

public enum DroneState
{
    Activate,
    Deactivate,
    Surveillance,
}


