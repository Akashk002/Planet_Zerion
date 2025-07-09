using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceCraftView : MonoBehaviour
{
    [SerializeField] private List<Transform> shootPoints;
    private EnemySpaceCraftController enemySpaceCraftController;
    private float shootInterval = 3f;
    private Coroutine shootCoroutine;

    // Update is called once per frame
    void Update()
    {
        enemySpaceCraftController.Update();
    }

    public void SetController(EnemySpaceCraftController enemySpaceCraftController)
    {
        this.enemySpaceCraftController = enemySpaceCraftController;
    }

    public void startShooting()
    {
        shootCoroutine = StartCoroutine(ShootLoop());
    }

    private IEnumerator ShootLoop()
    {
        while (true)
        {
            Transform initialTrans = shootPoints[Random.Range(0, shootPoints.Count)];
            Vector3 targetPos;
            // First, check if player is within 100 meters
            SpacecraftController spacecraftController = GameService.Instance.spacecraftService.GetSpacecraftController(); // Assumes a method that returns the player's transform
            if (spacecraftController != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, spacecraftController.GetPos());

                Debug.Log($"Distance to player: {distanceToPlayer}");

                if (distanceToPlayer <= GameService.Instance.EnemyHitPlayerDistance)
                {
                    // Target the player
                    targetPos = spacecraftController.GetPos();
                }
                else
                {
                    // Target a building instead
                    targetPos = GameService.Instance.buildingManager.GetRandomBuildingPos();
                }
            }
            else
            {
                // Fallback: target a building if player reference is missing
                targetPos = GameService.Instance.buildingManager.GetRandomBuildingPos();
            }

            enemySpaceCraftController.Shoot(initialTrans, targetPos);

            yield return new WaitForSeconds(enemySpaceCraftController.GetFireInteral());
        }
    }

    private void OnDisable()
    {
        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);
    }

    public void TakeDamage(float damage)
    {
        enemySpaceCraftController.TakeDamage(damage);
    }

    public void Die()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = StartCoroutine(ShootLoop());
        }
    }
}
