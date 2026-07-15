using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Ruta de la Plataforma")]
    [SerializeField] private Transform[] waypoints; // Puntos de ruta
    [SerializeField] private float speed = 3.0f;     // Velocidad de movimiento
    [SerializeField] private float waitTime = 1.0f;  // Tiempo de espera en cada punto

    private int currentWaypointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Asegurar que la plataforma empiece en el primer punto
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        if (isWaiting)
        {
            waitTimer += Time.fixedDeltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
            return;
        }

        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;

        // Mover el Rigidbody2D de forma física y fluida
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        // Si está muy cerca del objetivo, empieza a esperar
        if (Vector2.Distance(rb.position, targetPosition) < 0.05f)
        {
            isWaiting = true;
        }
    }

    // --- LOGICA DE "HIJO" EN 2D ---
    // Hace que el jugador se mueva solidariamente con la plataforma al pararse sobre ella

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Opcional: Verificar que el jugador caiga desde ARRIBA de la plataforma
            // para evitar que se "pegue" si choca desde abajo o por los lados.
            if (collision.relativeVelocity.y <= 0f)
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
