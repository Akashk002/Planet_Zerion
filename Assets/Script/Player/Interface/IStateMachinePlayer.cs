
public interface IStateMachinePlayer
{
    public void ChangeState(PlayerStates newState);
}

public enum PlayerStates
{
    Idle,
    Walk,
    Run
}

