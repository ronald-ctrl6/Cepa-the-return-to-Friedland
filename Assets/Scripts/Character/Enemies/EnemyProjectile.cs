using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody2D rb;
    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Configuramos el Rigidbody para que sea cinemático y no sufra físicas extrañas
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Usamos OnTriggerEnter2D para que cruce al jugador sin empujarlo físicamente
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Le baja un corazón
            }

            Destroy(gameObject); // Se destruye al impactar
        }
        else if (!other.isTrigger && !other.CompareTag("Enemy"))
        {
            // Si choca contra una pared u otro objeto sólido que no sea trigger, también se destruye
            Destroy(gameObject);
        }
    }
}