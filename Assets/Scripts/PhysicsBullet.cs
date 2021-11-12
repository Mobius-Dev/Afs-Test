namespace AFSInterview
{
    using UnityEngine;

    public class PhysicsBullet : MonoBehaviour
    {
        private Rigidbody rb;

        public void Initialize(Vector3 targetPosition, float force)
        {
            rb = GetComponent<Rigidbody>();

            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.AddForce(direction * force);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.GetMask("Enemy"))
            {
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
            else Destroy(gameObject);
        }
    }
}