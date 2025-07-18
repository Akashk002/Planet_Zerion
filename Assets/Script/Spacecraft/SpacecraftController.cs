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
    private Vector3? currentTargetPosition = null; // Nullable type

    private AudioSource audioSource;
    private float moveSpeed;

    // Add to class fields
    private Vector3 cachedMoveDirection;
    private Quaternion cachedTargetRotation;
    private bool hasMovementInput;
    private bool hasRotationInput;


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

        ProcessInput(); // New method
    }


    private void ProcessInput()
    {
        Vector3 moveDirection = Vector3.zero;
        hasMovementInput = false;
        hasRotationInput = false;

        // --- Forward/Backward Input ---
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

        moveDirection += spacecraftView.transform.forward * moveSpeed;
        spacecraftModel.SetRange(moveSpeed);

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += spacecraftView.transform.up * spacecraftModel.verticalSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection -= spacecraftView.transform.up * spacecraftModel.verticalSpeed;
        }

        // Store movement input
        cachedMoveDirection = moveDirection;
        hasMovementInput = moveDirection != Vector3.zero;
        spacecraftModel.SetAltitude((int)spacecraftView.transform.position.y);

        // --- Mouse Rotation Input ---
        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * spacecraftModel.rotationSpeed * spacecraftModel.mouseSensitivity * Time.fixedDeltaTime;
            pitch -= mouseY * spacecraftModel.rotationSpeed * spacecraftModel.mouseSensitivity * Time.fixedDeltaTime;
            pitch = Mathf.Clamp(pitch, -spacecraftModel.maxPitch, spacecraftModel.maxPitch);

            cachedTargetRotation = Quaternion.Euler(pitch, yaw, 0f);
            hasRotationInput = true;
        }
    }

    public void FixedUpdate()
    {
        if (state != State.Activate) return;

        // Apply movement
        spacecraftView.rb.velocity = cachedMoveDirection;

        // Apply rotation
        if (hasRotationInput)
        {
            spacecraftView.rb.MoveRotation(cachedTargetRotation);
        }

        // Audio logic (optional, or move to LateUpdate)
        if (hasMovementInput)
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
        int playerLayer = LayerMask.NameToLayer("Player");
        int ignorePlayerMask = ~(1 << playerLayer);

        Ray ray = spacecraftView.Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5000f, ignorePlayerMask))
        {
            currentTargetPosition = hit.point;
        }
        else
        {
            currentTargetPosition = null; // Reset
        }
    }

    private void FireMissileAtTarget()
    {
        if (currentTargetPosition == null) return; // No valid target

        if (spacecraftModel.missileCount > 0)
        {
            spacecraftModel.missileCount--;
            Transform shootPoint = spacecraftView.GetShootTransform();
            GameService.Instance.missileService.CreateMissile(spacecraftModel.missileType, shootPoint, currentTargetPosition.Value, false);
            spacecraftModel.SetMissileCount(spacecraftModel.missileCount);
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
