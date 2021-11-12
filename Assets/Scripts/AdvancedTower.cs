namespace AFSInterview
{
    using UnityEngine;

    public class AdvancedTower : Tower
    {
        private void Update()
        {
            targetEnemy = FindClosestEnemy();
            if (targetEnemy != null)
            {
                var lookRotation = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                if (targetEnemy != null)
                {
                    var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Bullet>();
                    bullet.Initialize(targetEnemy.gameObject);
                }

                fireTimer = firingRate;
            }
        }

        private Enemy FindClosestEnemy()
        {
            Enemy closestEnemy = null;
            var closestDistance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                var distance = (enemy.transform.position - transform.position).magnitude;
                if (distance <= firingRange && distance <= closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }

            return closestEnemy;
        }
    }
}