using System.Collections.Generic;

public class PlayerStateMachine : IStateMachinePlayer
{
    private PlayerController playerController;
    private IStatePlayer currentState;
    protected Dictionary<PlayerStates, IStatePlayer> States = new Dictionary<PlayerStates, IStatePlayer>();

    public PlayerStateMachine(PlayerController playerController)
    {
        this.playerController = playerController;
        CreateState();
        SetOwner();
    }

    private void CreateState()
    {
        States.Add(PlayerStates.Idle, new PlayerIdleState(this));
        States.Add(PlayerStates.Walk, new PlayerWalkState(this));
        States.Add(PlayerStates.Run, new PlayerRunState(this));
    }

    private void SetOwner()
    {
        foreach (IStatePlayer state in States.Values)
        {
            state.Owner = playerController;
        }
    }

    public void Update() => currentState?.Update();

    public void ChangeState(IStatePlayer newState)
    {
        currentState?.OnStateExit();
        currentState = newState;
        currentState?.OnStateEnter();
    }

    public void ChangeState(PlayerStates newState)
    {
        ChangeState(States[newState]);
    }
}
