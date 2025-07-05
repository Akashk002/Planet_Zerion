using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileView : MonoBehaviour
{
    private MissileController missileController;
    public VFXType ExplosionVFXType;

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

        if (other.gameObject.TryGetComponent(out enemySpaceCraftView))
        {
            enemySpaceCraftView.TakeDamage(missileController.missileScriptable.damage);
        }

        Building building;

        if (other.gameObject.TryGetComponent(out building))
        {
            building.TakeDamage(missileController.missileScriptable.damage);
        }

        GameService.Instance.missileService.ReturnDefenderPool(missileController);
        GameService.Instance.VFXService.PlayVFXAtPosition(ExplosionVFXType, transform.position);

        GameAudioType gameAudioType = (ExplosionVFXType == VFXType.MissileExplosionGround) ? GameAudioType.MissileBlastBig : GameAudioType.MissileBlastSmall;
        GameService.Instance.audioManager.PlayOneShotAt(gameAudioType, transform.position);
        gameObject.SetActive(false);
    }
}
