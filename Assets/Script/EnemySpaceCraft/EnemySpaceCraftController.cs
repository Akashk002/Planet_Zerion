using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemySpaceCraftController
{
    private EnemySpaceCraftScriptable enemySpaceCraftScriptable;
    private EnemySpaceCraftView enemySpaceCraftView;
    private Vector3 targetPosition;

    private bool isTargetReached = false;
    private bool isMoving = false;
    private AudioSource audioSource;
    private int currentHealth;
    public EnemySpaceCraftType enemySpaceCraftType;
    internal float GetFireInterval;

    public EnemySpaceCraftController(EnemySpaceCraftScriptable enemySpaceCraftScriptable)
    {
        enemySpaceCraftView = Object.Instantiate(enemySpaceCraftScriptable.enemySpaceCraftView);
        enemySpaceCraftView.SetController(this);
        this.enemySpaceCraftScriptable = enemySpaceCraftScriptable;
        this.enemySpaceCraftType = enemySpaceCraftScriptable.enemySpaceCraftType;
    }

    public void Configure(Vector3 initialPos, Vector3 tragetPos)
    {
        enemySpaceCraftView.gameObject.SetActive(true);
        enemySpaceCraftView.transform.position = initialPos;
        targetPosition = tragetPos;

        LookAtTarget();
        isMoving = true;
        currentHealth = (int)enemySpaceCraftScriptable.health;
        isTargetReached = false;
    }

    public void Update()
    {
        if (isMoving)
        {
            MoveTowardTarget();
        }
    }

    private void LookAtTarget()
    {
        Vector3 dir = (targetPosition - enemySpaceCraftView.transform.position).normalized;
        if (dir != Vector3.zero)
            enemySpaceCraftView.transform.rotation = Quaternion.LookRotation(dir);
    }

    private void MoveTowardTarget()
    {
        enemySpaceCraftView.transform.position = Vector3.MoveTowards(enemySpaceCraftView.transform.position, targetPosition, enemySpaceCraftScriptable.moveSpeed * Time.deltaTime);
        if (!isTargetReached && Vector3.Distance(enemySpaceCraftView.transform.position, targetPosition) < 0.1f)
        {
            isTargetReached = true;
            isMoving = false;
            enemySpaceCraftView.startShooting();
        }

        if (!isTargetReached)
        {
            if (audioSource == null || !audioSource.isPlaying)
            {
                audioSource = GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.EnemySpacecraftMoving, enemySpaceCraftView.transform.position);
            }
            else
            {
                audioSource.transform.position = enemySpaceCraftView.transform.position;
            }
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                GameService.Instance.audioManager.StopSound(audioSource);
                audioSource = null;
            }
        }
    }

    public void Shoot(Transform initialTrans, Vector3 targetPos)
    {
        bool EnableTurning = isTargetReached;
        GameService.Instance.missileService.CreateMissile(enemySpaceCraftScriptable.missileType, initialTrans, targetPos, EnableTurning);
        Debug.Log($"Enemy Spacecraft shooting at target: {targetPos} with missile type: {enemySpaceCraftScriptable.missileType}");
    }

    internal void TakeDamage(float damage)
    {
        currentHealth -= (int)damage;

        Debug.Log($"Enemy Spacecraft took damage: {damage}. Current Health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Spacecraft died.");
        enemySpaceCraftView.Die();
        GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.MissileBlastBig, enemySpaceCraftView.transform.position);
        GameService.Instance.VFXService.PlayVFXAtPosition(VFXType.MissileExplosionGround, enemySpaceCraftView.transform.position);
        GameService.Instance.enemySpaceCraftService.ReturnEnemySpaceCraftPool(this);
        enemySpaceCraftView.gameObject.SetActive(false);
    }

    public bool IsDead() => !enemySpaceCraftView.gameObject.activeSelf;
    internal float GetFireInteral() => enemySpaceCraftScriptable.fireInterval;
}
