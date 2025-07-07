using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour, ITriggerObject
{
    public CinemachineFreeLook cam;
    public CharacterController characterController;
    public Animator animator;
    public GameObject bagPack;
    public AudioSource audioSource;
    private PlayerController playerController;

    public void SetController(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (playerController != null)
        {
            playerController.Update();
        }
    }

    public void TriggerEnter(GameObject gameObject)
    {
        IInteractable interactable;
        if (gameObject.TryGetComponent(out interactable))
        {
            UIManager.Instance.GetInfoHandler().ShowObjectName(interactable.GetName());

            if (gameObject.GetComponent<Rock>())
            {
                UIManager.Instance.GetInfoHandler().ShowInstruction(InstructionType.RockCollect);
            }
        }
    }

    public void TriggerStay(GameObject gameObject)
    {
        IInteractable interactable;
        if (gameObject.TryGetComponent(out interactable) && !gameObject.GetComponent<Building>() && playerController.IsInteracted)
        {
            playerController.IsInteracted = false;
            if (gameObject.GetComponent<Entrance>() || (gameObject.GetComponent<Rock>() && IsCarryBagPack()))
            {
                interactable.Interact();

                if (gameObject.GetComponent<Rock>())
                {
                    Rock rock = gameObject.GetComponent<Rock>();
                    playerController.AddRock(rock.GetRockType());
                }
            }
            else
            {
                UIManager.Instance.GetInfoHandler().ShowInstruction(InstructionType.CarryBagpack);
            }
        }
    }

    public void TriggerExit(GameObject gameObject)
    {
        if (gameObject.GetComponent<IInteractable>() != null)
        {
            UIManager.Instance.GetInfoHandler().HideTextPopup();
        }
    }
    public bool IsCarryBagPack()
    {
        return bagPack.activeSelf;
    }

    public void CarryBagPack()
    {
        bagPack.SetActive(!IsCarryBagPack());
    }
}
