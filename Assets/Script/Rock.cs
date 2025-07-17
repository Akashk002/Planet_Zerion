using UnityEngine;

public class Rock : MonoBehaviour, IInteractable
{
    [SerializeField] private RockType rockType;
    public void Interact()
    {
        UIManager.Instance.GetInfoHandler().HideTextPopup();
        gameObject.SetActive(false);
    }

    public RockType GetRockType()
    {
        return rockType;
    }

    public string GetName()
    {
        return rockType + " Rock";
    }
}