using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    private float moveSpeed;
    private float tiredness;
    private PlayerScriptable playerScriptable;
    private float turnSmoothVelocity;
    private Vector3 gravityVelocity;
    private PlayerView playerView;
    private PlayerStateMachine stateMachine;
    private State state;
    private int rockCount;
    private LTDescr tireNessleenTween;
    public bool IsInteracted;
    public AudioSource walkAudioSource;
    public AudioSource runAudioSource;

    public PlayerController(PlayerView playerPrefab, PlayerScriptable playerScriptable)
    {
        this.playerView = Object.Instantiate(playerPrefab);
        playerView.SetController(this);
        this.playerScriptable = playerScriptable;
        playerView.characterController = new CharacterController(); // Assuming you have a way to initialize this
        CreateStateMachine();
        stateMachine.ChangeState(PlayerStates.Idle);
        playerScriptable.tiredness = 0f;
        Activate();
    }
    public void Interact() => IsInteracted = Input.GetKeyDown(KeyCode.E) ? true : (Input.GetKeyUp(KeyCode.E) ? false : IsInteracted);

    private void CreateStateMachine() => stateMachine = new PlayerStateMachine(this);

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
        moveSpeed = (v != 0) ? (!isRunning) ? playerScriptable.walkSpeed : playerScriptable.runSpeed : 0;

        PlayerMove(v);
    }

    public void PlayerMove(float v)
    {
        // Get the camera's X-axis rotation value from the FreeLook component
        float cameraXAxisValue = playerView.cam.m_XAxis.Value;

        // Always rotate player to match camera's horizontal rotation
        float targetAngle = cameraXAxisValue;
        float angle = Mathf.SmoothDampAngle(playerView.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, playerScriptable.rotationSmoothTime);
        playerView.transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // Movement relative to camera direction
        if (new Vector3(0, 0, v).magnitude >= 0.1f)
        {
            Vector3 inputDir = new Vector3(0, 0f, v).normalized;

            // Calculate movement direction based on camera rotation
            float moveAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraXAxisValue;
            Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;
            playerView.characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime * ((100 - playerScriptable.tiredness) / 100));

            SetTiredness(moveSpeed);
        }

        // Apply gravity
        if (playerView.characterController.isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2f;
        }

        gravityVelocity.y += playerScriptable.gravity * Time.deltaTime;
        playerView.characterController.Move(gravityVelocity * Time.deltaTime);
    }

    public void Activate()
    {
        UIManager.Instance.ShowPanel(PanelType.Player);
        state = State.Activate;
        playerView.gameObject.SetActive(true);
        playerView.cam.Priority = 1;
        UIManager.Instance.minimapIconPanel.SetTarget(playerView.transform);

        if (tireNessleenTween != null)
        {
            LeanTween.cancel(tireNessleenTween.id);
            tireNessleenTween = null;
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

    public void CarryBagPack()
    {
        playerView.CarryBagPack();
    }

    public void AddRock(RockType rockType)
    {
        if (GetTotalRock() < playerScriptable.RockStorageCapacity)
        {
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.CollectRock, playerView.transform.position);
            RockData rockData = playerScriptable.rockDatas.Find(r => r.RockType == rockType);
            rockData.AddRock();
            UIManager.Instance.playerPanel.UpdateRockCount();
        }
        else
        {
            UIManager.Instance.GetInfoHandler().ShowInstruction(InstructionType.StorageFull);
        }
    }

    private void SetTiredness(float val)
    {
        int extraWeitht = playerView.IsCarryBagPack() ? 2 : 1;
        playerScriptable.tiredness += val * playerScriptable.tirednessIncRate * extraWeitht;
        Mathf.Clamp(playerScriptable.tiredness, 0, playerScriptable.maxTiredness);
        UIManager.Instance.playerPanel.SetTiredness(playerScriptable.tiredness, playerScriptable.maxTiredness);
    }

    public void TakeRest()
    {
        float tirednessRecoverTime = playerScriptable.tirednessRecoverTime * (playerScriptable.maxTiredness / playerScriptable.maxTiredness);
        tireNessleenTween = LeanTween.value(playerScriptable.tiredness, 0, tirednessRecoverTime).setOnUpdate((float val) =>
        {
            playerScriptable.tiredness = val;
            UIManager.Instance.playerPanel.SetTiredness(playerScriptable.tiredness, playerScriptable.maxTiredness);
        }).setOnComplete(() => tireNessleenTween = null);
    }

    public void SpendRock(int rockRequire)
    {
        int remaining = rockRequire;

        foreach (var rockData in playerScriptable.rockDatas)
        {
            if (remaining <= 0) break;

            int spendAmount = Mathf.Min(rockData.rockCount, remaining);
            rockData.SpendRock(spendAmount);
            remaining -= spendAmount;

            UIManager.Instance.playerPanel.UpdateRockCount();
        }

        if (remaining > 0)
        {
            Debug.LogWarning("Not enough total rocks to spend the required amount!");
            // Optionally: rollback if partial spending is not allowed
        }
    }

    public Animator GetPlayerAnimator()
    {
        return playerView.animator;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public float GetPlayerWalkSpeed()
    {
        return playerScriptable.walkSpeed;
    }

    public Vector3 GetPos()
    {
        return playerView.transform.position;
    }

    public List<RockData> GetRockDatas()
    {
        return playerScriptable.rockDatas;
    }

    public int GetTotalRock()
    {
        int totalCount = 0;
        foreach (var rockData in playerScriptable.rockDatas)
        {
            totalCount += rockData.rockCount;
        }
        return totalCount;
    }

    public void TakeRocks(List<RockData> droneRocks)
    {
        int currentTotal = GetTotalRock();

        foreach (var droneRock in droneRocks)
        {
            var playerRock = playerScriptable.rockDatas.Find(r => r.RockType == droneRock.RockType);
            if (playerRock == null) continue;

            int spaceLeft = playerScriptable.RockStorageCapacity - currentTotal;
            if (spaceLeft <= 0) return; // Player inventory full

            int transferableAmount = Mathf.Min(droneRock.rockCount, spaceLeft);

            playerRock.AddRock(transferableAmount);
            droneRock.SpendRock(transferableAmount);
            currentTotal += transferableAmount;
        }

        UIManager.Instance.playerPanel.UpdateRockCount();
    }

}

public enum State
{
    Activate,
    deactivate
}