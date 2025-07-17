using UnityEngine;

public class MissileView : MonoBehaviour
{
    private MissileController missileController;
    public VFXType ExplosionVFXType;
    public bool playerMissile;

    // Update is called once per frame
    void Update()
    {
        missileController.Update();
    }
    public void SetController(MissileController missileController)
    {
        this.missileController = missileController;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemySpaceCraftView enemySpaceCraftView;

        if (other.gameObject.TryGetComponent(out enemySpaceCraftView) && playerMissile)
        {
            enemySpaceCraftView.TakeDamage(missileController.GetDamage());
        }

        SpacecraftView spacecraftView;

        if (other.gameObject.TryGetComponent(out spacecraftView) && !playerMissile)
        {
            spacecraftView.Destroy();
            UIManager.Instance.ShowMissionFailedPanelWithDelay();
        }

        Building building;

        if (other.gameObject.TryGetComponent(out building))
        {
            building.TakeDamage(missileController.GetDamage());
        }

        Destroy();
    }

    public void Destroy()
    {
        GameService.Instance.missileService.ReturnDefenderPool(missileController);
        GameService.Instance.VFXService.PlayVFXAtPosition(ExplosionVFXType, transform.position);

        GameAudioType gameAudioType = (ExplosionVFXType == VFXType.MissileExplosionGround) ? GameAudioType.MissileBlastBig : GameAudioType.MissileBlastSmall;
        GameService.Instance.audioManager.PlayOneShotAt(gameAudioType, transform.position);
        gameObject.SetActive(false);
    }
}
