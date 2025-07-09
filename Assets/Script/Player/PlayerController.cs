using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    private PlayerView playerView;
    private PlayerModel playerModel;
    private PlayerStateMachine stateMachine;
    private float moveSpeed;
    private float turnSmoothVelocity;
    private Vector3 gravityVelocity;
    private State state;
    public bool IsInteracted;
    public AudioSource walkAudioSource;
    public AudioSource runAudioSource;

    public PlayerController(PlayerView playerPrefab, PlayerModel playerModel)
    {
        this.playerView = Object.Instantiate(playerPrefab);
        playerView.SetController(this);
        this.playerModel = playerModel;
        CreateStateMachine();
        stateMachine.ChangeState(PlayerStates.Idle);

        Activate();
    }

    public void Update()
    {
        if (state == State.Activate)
        {
            GetInput();
            stateMachine.Update();
            Interact();
        }
    }

    private void GetInput()
    {
        float v = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.Space);
        moveSpeed = (v != 0) ? (!isRunning) ? playerModel.walkSpeed : playerModel.runSpeed : 0;

        PlayerMove(v);
    }

    public void PlayerMove(float v)
    {
        // Get the camera's X-axis rotation value from the FreeLook component
        float cameraXAxisValue = playerView.cam.m_XAxis.Value;

        // Always rotate player to match camera's horizontal rotation
        float targetAngle = cameraXAxisValue;
        float angle = Mathf.SmoothDampAngle(playerView.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, playerModel.rotationSmoothTime);
        playerView.transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // Movement relative to camera direction
        if (new Vector3(0, 0, v).magnitude >= 0.1f)
        {
            Vector3 inputDir = new Vector3(0, 0f, v).normalized;

            // Calculate movement direction based on camera rotation
            float moveAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraXAxisValue;
            Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;
            playerView.characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime * ((100 - playerModel.tiredness) / 100));

            playerModel.SetTiredness(moveSpeed);
        }

        // Apply gravity
        if (playerView.characterController.isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2f;
        }

        gravityVelocity.y += playerModel.gravity * Time.deltaTime;
        playerView.characterController.Move(gravityVelocity * Time.deltaTime);
    }

    public void Activate()
    {
        playerView.gameObject.SetActive(true);
        playerView.cam.Priority = 1;
        state = State.Activate;

        UIManager.Instance.minimapIconPanel.SetTarget(playerView.transform);
        UIManager.Instance.ShowPanel(PanelType.Player);

        if (playerModel.tireNessleenTween != null)
        {
            LeanTween.cancel(playerModel.tireNessleenTween.id);
            playerModel.tireNessleenTween = null;
        }
    }

    public void Deactivate()
    {
        state = State.deactivate;
        playerView.gameObject.SetActive(false);
        playerView.cam.Priority = 0;
        walkAudioSource.Stop();
        runAudioSource.Stop();
    }

    public void Interact() => IsInteracted = Input.GetKeyDown(KeyCode.E) ? true : (Input.GetKeyUp(KeyCode.E) ? false : IsInteracted);
    private void CreateStateMachine() => stateMachine = new PlayerStateMachine(this);
    public void CarryBagPack() => playerView.CarryBagPack();
    public Animator GetPlayerAnimator() => playerView.animator;
    public float GetMoveSpeed() => moveSpeed;
    public float GetPlayerWalkSpeed() => playerModel.walkSpeed;
    public Vector3 GetPos() => playerView.transform.position;
    public List<RockData> GetRockDatas() => playerModel.rockDatas;
    public void TakeRest() => playerModel.TakeRest();
    public int GetTotalRock() => playerModel.GetTotalRock();
    public void AddRock(RockType rockType) => playerModel.AddRock(rockType, playerView.transform.position);
    public void TakeRocks(List<RockData> rockDatas) => playerModel.TakeRocks(rockDatas);
    public void SpendRock(int rockRequire) => playerModel.SpendRock(rockRequire);
}

public enum State
{
    Activate,
    deactivate
}