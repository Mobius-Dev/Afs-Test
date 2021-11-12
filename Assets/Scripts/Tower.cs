namespace AFSInterview
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Tower : MonoBehaviour
    {
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected Transform bulletSpawnPoint;
        [SerializeField] protected float firingRate;
        [SerializeField] protected float firingRange;

        protected float fireTimer;
        protected Enemy targetEnemy;

        protected IReadOnlyList<Enemy> enemies;

        public void Initialize(IReadOnlyList<Enemy> enemies)
        {
            this.enemies = enemies;
            fireTimer = firingRate;
        }
    }
}