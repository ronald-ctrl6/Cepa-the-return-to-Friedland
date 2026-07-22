using System.Collections;
using System.Collections.Generic;
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

    [Header("Habilidades Desbloqueables")]
    [SerializeField] private bool hasDoubleJump = false;
    [SerializeField] private bool hasDash = false;

    [Header("Límites de Salto")]
    [SerializeField] private int maxJumps = 1;

    [Header("Configuración de Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.2f; // Breve pausa entre dashes
    private bool isDashing = false;
    private float dashTimeLeft;
    private float dashCooldownTimer;
    private float dashDirection;
    private bool hasDashedInAir = false; // Control opcional para limitar o permitir en aire

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

        // Al iniciar la escena, sincronizamos con el GameManager guardado
        if (GameManager.Instance != null)
        {
            hasDoubleJump = GameManager.Instance.hasDoubleJump;
            hasDash = GameManager.Instance.hasDash;

            if (hasDoubleJump)
            {
                maxJumps = 2;
            }
            else
            {
                maxJumps = 1;
            }
        }
        else
        {
            maxJumps = 1;
        }
    }

    public void UnlockDoubleJump()
    {
        hasDoubleJump = true;
        maxJumps = 2;
        if (GameManager.Instance != null) GameManager.Instance.hasDoubleJump = true;
    }

    public void UnlockDash()
    {
        hasDash = true;
        if (GameManager.Instance != null) GameManager.Instance.hasDash = true;
    }

    void Update()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Si está haciendo dash, ignoramos el input normal
        if (isDashing) return;

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

        // Detección del Dash (Tecla LeftShift)
        // Permite dash infinito en el suelo, o múltiples en el aire si lo deseas quitando restricciones
        if (hasDash && Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f)
        {
            if (isGrounded || !hasDashedInAir)
            {
                StartDash();
                if (!isGrounded) hasDashedInAir = true; // Si quieres que en el aire solo se haga una vez por salto, déjalo. Si quieres infinitos en el aire, borra esta línea.
            }
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
                animator.SetBool("IsMovingClimbing", isMovingOnLadder);
            }
            else if (isGrounded || (hasDoubleJump && jumpCount < maxJumps))
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

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        dashCooldownTimer = dashCooldown;

        // Toma la dirección actual en la que mira el personaje
        dashDirection = transform.localScale.x > 0 ? 1f : -1f;
        rigid.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            dashTimeLeft -= Time.fixedDeltaTime;
            rigid.velocity = new Vector2(dashDirection * dashSpeed, 0f);

            if (dashTimeLeft <= 0f)
            {
                isDashing = false;
                rigid.gravityScale = originalGravity;
            }
            return;
        }

        Vector2 leftRayPosition = (Vector2)groundCheck.position + Vector2.left * (rayWidth / 2f);
        Vector2 rightRayPosition = (Vector2)groundCheck.position + Vector2.right * (rayWidth / 2f);

        bool leftGrounded = Physics2D.Raycast(leftRayPosition, Vector2.down, rayLength, groundLayer);
        bool rightGrounded = Physics2D.Raycast(rightRayPosition, Vector2.down, rayLength, groundLayer);

        isGrounded = leftGrounded || rightGrounded;

        Debug.DrawRay(leftRayPosition, Vector2.down * rayLength, leftGrounded ? Color.green : Color.red);
        Debug.DrawRay(rightRayPosition, Vector2.down * rayLength, rightGrounded ? Color.green : Color.red);

        // Al tocar el suelo reseteamos saltos y el permiso de dash aéreo
        if (isGrounded)
        {
            jumpCount = 0;
            hasDashedInAir = false;
        }

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

            float platformHorizontalVelocity = 0f;
            if (activePlatform != null)
            {
                platformHorizontalVelocity = activePlatform.CurrentVelocity.x;
            }

            float horizontalSpeed = (moveInput * speed) + platformHorizontalVelocity;
            rigid.velocity = new Vector2(horizontalSpeed, rigid.velocity.y);

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

        bool isMovingOnLadder = isClimbing && (Mathf.Abs(verticalInput) > 0.1f || Mathf.Abs(moveInput) > 0.1f);
        animator.SetBool("IsMovingClimbing", isMovingOnLadder);

        if (jumpRequested)
        {
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            if (isGrounded)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
                jumpCount = 1;
            }
            else if (hasDoubleJump && jumpCount < maxJumps)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
                jumpCount++;
            }

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