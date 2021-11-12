namespace AFSInterview
{
    using UnityEngine;

    public class AdvancedTower : Tower
    {
        [SerializeField] private int burstBullets;
        [SerializeField] private float burstRate;
        [SerializeField] private float bulletForce;

        [Tooltip("In frames")]
        [SerializeField] private float maxLeadOnTarget;

        private int burstBulletsLeft;
        private float burstTimer;
        private bool isBursting;

        private void Update()
        {
            targetEnemy = FindClosestEnemy();
            if (targetEnemy != null)
            {
                var lookRotation = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            else isBursting = false; //end burst if no target

            if (isBursting)
            {
                if (burstBulletsLeft == 0)
                {
                    isBursting = false;
                    return;
                }

                burstTimer -= Time.deltaTime;

                if (burstTimer < 0f)
                {
                    burstTimer = burstRate;
                    burstBulletsLeft--;
                    FirePhysicsBullet();
                }
            }
            else
            {
                fireTimer -= Time.deltaTime;

                if (fireTimer <= 0f)
                {
                    fireTimer = firingRate;
                    isBursting = true;
                    burstBulletsLeft = burstBullets;
                }
            }
        }

        private void FirePhysicsBullet()
        {
            if (targetEnemy != null)
            {
                float lead = maxLeadOnTarget * Vector3.Distance(transform.position, targetEnemy.transform.position) / firingRange; //the further the target is the greater the lead amount

                Vector3 targetPosition = targetEnemy.GetPredictedPosition(lead);

                PhysicsBullet physicsBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<PhysicsBullet>();
                physicsBullet.Initialize(targetPosition, bulletForce);
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