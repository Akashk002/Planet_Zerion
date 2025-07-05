using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacecraftView : MonoBehaviour
{
    public Rigidbody rb;
    public CinemachineVirtualCamera cam;
    private SpacecraftController controller;
    [SerializeField] private List<Transform> shootPoints;
    [SerializeField] private List<ParticleSystem> particleList;

    public Camera Camera;

    private void Start()
    {
        Camera = Camera.main;
    }

    internal void SetController(SpacecraftController spacecraftController)
    {
        this.controller = spacecraftController;
    }

    private void Update()
    {
        if (controller != null)
        {
            controller.Update();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LandingZone>())
        {
            UIManager.Instance.spacecraftPanel.ToggleBackToRoomBtn(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<LandingZone>())
        {
            UIManager.Instance.spacecraftPanel.ToggleBackToRoomBtn(false);
        }
    }

    public Transform GetShootTransform()
    {
        int randomIndex = Random.Range(0, shootPoints.Count);
        return shootPoints[randomIndex];
    }

    public void EnableBoosterVFX(bool enable)
    {
        particleList.ForEach(p =>
        {
            if (enable)
            {
                p.Play();
            }
            else
            {
                p.Stop();
            }
        });
    }
}
