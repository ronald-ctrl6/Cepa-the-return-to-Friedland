using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTime = 0.5f;

    private int currentWaypointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    // NUEVA PROPIEDAD: Permite al jugador leer la velocidad actual de la plataforma
    public Vector2 CurrentVelocity { get; private set; }
    private Vector3 lastPosition;

    void Start()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
        lastPosition = transform.position;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
            }
            CurrentVelocity = Vector2.zero; // Sin velocidad mientras espera
            return;
        }

        MoveTowardsTarget();
    }

    // Usamos FixedUpdate para el cálculo físico de la velocidad
    void FixedUpdate()
    {
        CurrentVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            transform.position = targetPosition;
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            if (waitTime > 0f)
            {
                isWaiting = true;
            }
        }
    }

    // --- DETECCIÓN DE COLISIÓN PARA ADOPTAR AL JUGADOR ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);
            int nextIndex = (i + 1) % waypoints.Length;
            if (waypoints[nextIndex] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
        }
    }
}
