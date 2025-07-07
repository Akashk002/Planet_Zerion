
public interface IStatePlayer
{
    public PlayerController Owner { get; set; }
    public void OnStateEnter();
    public void Update();
    public void OnStateExit();
}
