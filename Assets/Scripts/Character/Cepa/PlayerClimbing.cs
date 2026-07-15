using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    [Header("Configuración de Escalada")]
    [SerializeField] private float climbSpeed = 4f;

    private float verticalInput;
    private bool isNearLadder = false;
    private bool isClimbing = false;
    private float originalGravity;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Guardamos la gravedad original del jugador para poder restaurarla al bajarse
        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        // Leer la entrada vertical (W / S o Flechas)
        verticalInput = Input.GetAxisRaw("Vertical");

        // Si estamos cerca de la escalera y el jugador presiona W o S (Input vertical != 0)
        if (isNearLadder && Mathf.Abs(verticalInput) > 0.1f)
        {
            isClimbing = true;
        }
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            // Al escalar, apagamos la gravedad para que no se caiga
            rb.gravityScale = 0f;

            // Movemos al jugador hacia arriba o abajo
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
        }
        else
        {
            // Si no está escalando, restauramos su gravedad original
            rb.gravityScale = originalGravity;
        }
    }

    // --- DETECCIÓN DE LA ESCALERA ---

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isNearLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isNearLadder = false;
            isClimbing = false; // Deja de escalar automáticamente al salir de la escalera
        }
    }
}