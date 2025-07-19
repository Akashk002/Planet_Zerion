using Cinemachine;
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

    private void FixedUpdate()
    {
        if (controller != null)
        {
            controller.FixedUpdate();
        }
    }

    private void Update()
    {
        if (controller != null)
        {
            controller.Update();
        }
    }

    internal void SetController(SpacecraftController spacecraftController)
    {
        this.controller = spacecraftController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LandingZone>())
        {
            UIManager.Instance.spacecraftPanel.ShowAndHideBtn(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<LandingZone>())
        {
            UIManager.Instance.spacecraftPanel.ShowAndHideBtn(false);
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

    public void Destroy()
    {
        GameService.Instance.VFXService.PlayVFXAtPosition(VFXType.MissileExplosion, transform.position);
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.MissileBlastBig, transform.position);
        gameObject.SetActive(false);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
