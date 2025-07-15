using UnityEngine;

public class DroneView : MonoBehaviour, ITriggerObject
{
    public Rigidbody rb;
    public Camera cam;
    private DroneController controller;

    public void SetController(DroneController controller)
    {
        this.controller = controller;
    }

    private void Update()
    {
        if (controller != null)
        {
            controller.Update();
        }
    }

    public void TriggerEnter(GameObject gameObject)
    {
        if (gameObject.GetComponent<Rock>())
        {
            UIManager.Instance.GetInfoHandler().ShowInstruction(InstructionType.RockCollect);
        }

        if (gameObject.GetComponent<Entrance>() && gameObject.GetComponent<Entrance>().GetEntranceBuildingType() == BuildingType.DroneControlRoom)
        {
            controller.SetDroneNearControlRoom(true);
        }
    }

    public void TriggerStay(GameObject gameObject)
    {
        IInteractable interactable;
        if (gameObject.TryGetComponent(out interactable))
        {
            Rock rock;
            if ((gameObject.TryGetComponent(out rock) && controller.IsInteracted))
            {
                controller.IsInteracted = false;
                interactable.Interact();
                controller.AddRock(rock.GetRockType());
            }
        }
    }

    public void TriggerExit(GameObject gameObject)
    {
        if (gameObject.GetComponent<IInteractable>() != null)
        {
            if (gameObject.GetComponent<Rock>())
            {
                UIManager.Instance.GetInfoHandler().HideTextPopup();
            }
        }

        if (gameObject.GetComponent<Entrance>() && gameObject.GetComponent<Entrance>().GetEntranceBuildingType() == BuildingType.DroneControlRoom)
        {
            controller.SetDroneNearControlRoom(false);
        }
    }
}

