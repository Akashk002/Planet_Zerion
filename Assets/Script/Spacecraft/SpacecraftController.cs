using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
public class SpacecraftController
{
    private float yaw;
    private float pitch;
    private bool isRotating = false;
    private SpacecraftView spacecraftView;
    private SpacecraftModel spacecraftModel;
    private Transform initialTransform;
    private State state;
    private Vector3 currentTargetPosition;

    private AudioSource audioSource;
    private float moveSpeed;

    public SpacecraftController(SpacecraftModel spacecraftModel)
    {
        spacecraftView = Object.Instantiate(spacecraftModel.spacecraftPrefab);
        spacecraftView.SetController(this);
        this.spacecraftModel = spacecraftModel;
    }

    public void Configure()
    {
        initialTransform = spacecraftView.transform;
        spacecraftView.rb.freezeRotation = true;
        spacecraftModel.Refuel();
        spacecraftModel.ReloadMissile();
        // Lock cursor for mouse look
        //Cursor.lockState = CursorLockMode.Locked;

        // Initialize rotation values from current orientation
        Vector3 euler = spacecraftView.transform.eulerAngles;
        yaw = euler.y;
        pitch = euler.x;

        Deactivate();
    }

    public void Update()
    {
        if (state != State.Activate) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            AimAtTarget();
            FireMissileAtTarget();
        }

        Move();
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.zero;

        // --- Throttle Forward/Backward ---
        bool isAccelerating = Input.GetKey(KeyCode.W) && moveSpeed < spacecraftModel.maxSpeed;
        bool isBraking = Input.GetKey(KeyCode.S) && moveSpeed > 0;

        if (isAccelerating)
        {
            moveSpeed += spacecraftModel.accelerationSpeed;
            isRotating = true;

            spacecraftModel.SetSpeed((int)moveSpeed);
        }
        else if (isBraking)
        {
            moveSpeed -= spacecraftModel.brakeSpeed;
            spacecraftModel.SetSpeed((int)moveSpeed);
        }

        // --- Forward Movement ---
        moveDirection += spacecraftView.transform.forward * moveSpeed;
        spacecraftModel.SetRange(moveSpeed);

        // --- Vertical Movement ---
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += spacecraftView.transform.up * spacecraftModel.verticalSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection -= spacecraftView.transform.up * spacecraftModel.verticalSpeed;
        }

        // --- Apply Velocity ---
        spacecraftView.rb.velocity = moveDirection;
        spacecraftModel.SetAltitude((int)spacecraftView.transform.position.y);

        if (moveDirection != Vector3.zero)
        {
            if (audioSource == null || !audioSource.isPlaying)
            {
                audioSource = GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.SpacecraftMoving, spacecraftView.transform.position);
            }
            else
            {
                audioSource.transform.position = spacecraftView.transform.position;
            }
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                GameService.Instance.audioManager.StopSound(audioSource);
                audioSource = null;
            }
        }

        // --- Rotation by Mouse ---
        if (isRotating || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * spacecraftModel.rotationSpeed * spacecraftModel.mouseSensitivity * Time.fixedDeltaTime;
            pitch -= mouseY * spacecraftModel.rotationSpeed * spacecraftModel.mouseSensitivity * Time.fixedDeltaTime;
            pitch = Mathf.Clamp(pitch, -spacecraftModel.maxPitch, spacecraftModel.maxPitch);

            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
            spacecraftView.rb.MoveRotation(targetRotation);
        }
    }

    public void Activate()
    {
        state = State.Activate;
        spacecraftView.cam.Priority = 1;
        spacecraftView.EnableBoosterVFX(true);
        UIManager.Instance.ShowPanel(PanelType.Spacecraft);
        UIManager.Instance.minimapIconPanel.SetTarget(spacecraftView.transform);
    }

    public void Deactivate()
    {
        state = State.deactivate;
        spacecraftView.cam.Priority = 0;
        spacecraftView.EnableBoosterVFX(false);
    }

    private void AimAtTarget()
    {
        // Get layer mask that excludes "Player"
        int playerLayer = LayerMask.NameToLayer("Player");
        int ignorePlayerMask = ~(1 << playerLayer); // Invert to ignore Player

        // Get ray from camera center
        Ray ray = spacecraftView.Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5000f, ignorePlayerMask))
        {
            currentTargetPosition = hit.point;
        }
    }

    private void FireMissileAtTarget()
    {
        if (spacecraftModel.missileCount > 0)
        {
            spacecraftModel.missileCount--;
            Transform shootPoint = spacecraftView.GetShootTransform();
            GameService.Instance.missileService.CreateMissile(spacecraftModel.missileType, shootPoint, currentTargetPosition, false);
            spacecraftModel.SetMissileCount(spacecraftModel.missileCount);
            Debug.Log($"Spacecraft shooting at target: {currentTargetPosition} with missile type: {spacecraftModel.missileType}");
        }
    }

    public void Destroy() => spacecraftView.Destroy();
    public Vector3 GetPos() => spacecraftView.transform.position;
    public void Refuel() => spacecraftModel.Refuel();
    public void ReloadMissile() => spacecraftModel.ReloadMissile();
    public object GetMaxSpeed() => spacecraftModel.maxSpeed;
    public int GetMissileCapacity() => spacecraftModel.missileCapacity;
    public int GetMaxRange() => spacecraftModel.maxRange;
}
