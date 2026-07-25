using UnityEngine; // Assited with AI

namespace game
{
    public class Projectile : MonoBehaviour
    {
        [Tooltip("The speed of the projectile.")]
        [SerializeField] private float speed = 10f;
        [Tooltip("How long the projectile travels for.")]
        [SerializeField] private float lifetime = 3f;
        [Tooltip("Damage the projectile does.")]
        [SerializeField] protected float damage = 5f;

<<<<<<< HEAD
        // Rigidbody of the projectile
=======
        // Rigidbody of projectile
>>>>>>> main
        private Rigidbody rb;

        // Assigns rigidbody
        private void Awake()
        {
            
            rb = GetComponent<Rigidbody>();
        }

        // Sends the projectile in the direction at speed
        public void Launch(Vector3 direction)
        {
            rb.linearVelocity = direction * speed;
        }

        // Destroys game object after lifetime
        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        // When the projectiles collides with the player, they take damage and the projectile is destroyed
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("The player has been hit!");
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
            else
            {
                //Debug.Log("A projectile has hit " + collision.collider.gameObject.name);
            }
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
            
        }
    }
}
