using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigid;

    [Header("Configuración de Movimiento")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 19f;

    [Header("Configuración de Escalada")]
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float grabCooldownDuration = 0.25f;

    private float verticalInput;
    private bool isNearLadder = false;
    private bool isClimbing = false;
    private float originalGravity;
    private int laddersTouchingCount = 0;
    private float grabCooldownTimer = 0f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength = 0.15f;
    [SerializeField] private float rayWidth = 0.3f;

    [Header("Límites de Salto")]
    [SerializeField] private int maxJumps = 1;

    [Header("Ajustes de Gravedad Dinámica")]
    [SerializeField] private float riseMultiplier = 5f;
    [SerializeField] private float fallMultiplier = 5f;
    [SerializeField] private float lowJumpMultiplier = 22f;

    private float moveInput;
    private bool isGrounded;
    private int jumpCount;
    private bool jumpRequested;

    private MovingPlatform activePlatform;


    void Start()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        originalGravity = rigid.gravityScale;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (grabCooldownTimer > 0f)
        {
            grabCooldownTimer -= Time.deltaTime;
        }

        animator.SetBool("IsRunning", moveInput != 0);
        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isClimbing)
            {
                isClimbing = false;
                rigid.gravityScale = originalGravity;
                grabCooldownTimer = grabCooldownDuration;
                jumpRequested = true;

                bool isMovingOnLadder = isClimbing && Mathf.Abs(verticalInput) > 0.1f;
                animator.SetBool("IsMovingClimbing", isMovingOnLadder); // O el nombre que prefieras usar en t
            }
            else if (jumpCount < maxJumps)
            {
                jumpRequested = true;
            }
        }

        if (isNearLadder && Mathf.Abs(verticalInput) > 0.1f && grabCooldownTimer <= 0f)
        {
            if (isGrounded && verticalInput < -0.1f && !isClimbing)
            {
                // Evita descender en el suelo firme
            }
            else
            {
                isClimbing = true;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 leftRayPosition = (Vector2)groundCheck.position + Vector2.left * (rayWidth / 2f);
        Vector2 rightRayPosition = (Vector2)groundCheck.position + Vector2.right * (rayWidth / 2f);

        bool leftGrounded = Physics2D.Raycast(leftRayPosition, Vector2.down, rayLength, groundLayer);
        bool rightGrounded = Physics2D.Raycast(rightRayPosition, Vector2.down, rayLength, groundLayer);

        isGrounded = leftGrounded || rightGrounded;

        Debug.DrawRay(leftRayPosition, Vector2.down * rayLength, leftGrounded ? Color.green : Color.red);
        Debug.DrawRay(rightRayPosition, Vector2.down * rayLength, rightGrounded ? Color.green : Color.red);

        if (isClimbing && isGrounded && Mathf.Abs(moveInput) > 0.1f)
        {
            isClimbing = false;
            rigid.gravityScale = originalGravity;
        }

        if (isClimbing)
        {
            rigid.gravityScale = 0f;
            rigid.velocity = new Vector2(moveInput * speed, verticalInput * climbSpeed);
            jumpCount = 0;
        }
        else
        {
            rigid.gravityScale = originalGravity;

            // Se obtiene la velocidad horizontal de la plataforma activa si el personaje está sobre ella
            float platformHorizontalVelocity = 0f;
            if (activePlatform != null)
            {
                platformHorizontalVelocity = activePlatform.CurrentVelocity.x;
            }

            // Suma la velocidad del movimiento lateral y la velocidad propia de la plataforma
            float horizontalSpeed = (moveInput * speed) + platformHorizontalVelocity;
            rigid.velocity = new Vector2(horizontalSpeed, rigid.velocity.y);

            if (isGrounded && rigid.velocity.y <= 0.1f)
            {
                jumpCount = 0;
            }

            if (rigid.velocity.y < 0)
            {
                rigid.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rigid.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rigid.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rigid.velocity.y > 0 && Input.GetKey(KeyCode.Space))
            {
                rigid.velocity += Vector2.up * Physics2D.gravity.y * (riseMultiplier - 1) * Time.fixedDeltaTime;
            }
        }

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VelocityY", rigid.velocity.y);
        animator.SetBool("IsClimbing", isClimbing);

        // Control para evitar que la animación de escalar se congele al estar quieto en la escalera
        bool isMovingOnLadder = isClimbing && (Mathf.Abs(verticalInput) > 0.1f || Mathf.Abs(moveInput) > 0.1f);
        animator.SetBool("IsMovingClimbing", isMovingOnLadder);

        if (jumpRequested)
        {
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            jumpCount++;
            jumpRequested = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            laddersTouchingCount++;
            isNearLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            laddersTouchingCount--;

            if (laddersTouchingCount <= 0)
            {
                laddersTouchingCount = 0;
                isNearLadder = false;

                if (isClimbing)
                {
                    isClimbing = false;
                    rigid.gravityScale = originalGravity;
                    grabCooldownTimer = grabCooldownDuration;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            MovingPlatform platform = collision.gameObject.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                activePlatform = platform;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (activePlatform != null && collision.gameObject == activePlatform.gameObject)
            {
                activePlatform = null;
            }
        }
    }
}