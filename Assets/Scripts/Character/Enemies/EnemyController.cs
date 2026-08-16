using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private float patrolDistance = 3f; // Qué tanto se mueve a la izquierda y derecha desde donde lo colocas
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool flipSprite = true;

    [Header("Detección y Disparo")]
    [SerializeField] private float detectionRadius = 4f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 2f;

    [Header("Animación")]
    [SerializeField] private Animator animator;
    [SerializeField] private string isWalkingBool = "IsWalking";

    private Rigidbody2D rb;
    private int direction = 1; // 1 = derecha, -1 = izquierda
    private float originalScaleX;
    private float fireTimer;
    private Transform playerTransform;
    private bool isPlayerInRange = false;

    private float leftLimitX;
    private float rightLimitX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScaleX = Mathf.Abs(transform.localScale.x);

        if (animator == null) animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Calculamos los límites automáticamente basándonos en dónde se coloca el enemigo en el mapa
        float startX = transform.position.x;
        leftLimitX = startX - patrolDistance;
        rightLimitX = startX + patrolDistance;
    }

    private void Update()
    {
        DetectPlayer();

        if (isPlayerInRange)
        {
            // Detenido disparando (Idle)
            StopMovement();
            if (animator != null) animator.SetBool(isWalkingBool, false);

            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
        else
        {
            // Patrullando (Caminando)
            fireTimer = 0f;
            if (animator != null) animator.SetBool(isWalkingBool, true);
        }
    }

    private void FixedUpdate()
    {
        if (isPlayerInRange)
        {
            StopMovement();
            return;
        }

        MoveAndCheckPatrol();
    }

    private void MoveAndCheckPatrol()
    {
        // Movimiento horizontal con Rigidbody2D
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        // Verificamos los límites automáticos
        if (direction == 1 && transform.position.x >= rightLimitX)
        {
            direction = -1;
            UpdateSpriteDirection(direction);
        }
        else if (direction == -1 && transform.position.x <= leftLimitX)
        {
            direction = 1;
            UpdateSpriteDirection(direction);
        }
    }

    private void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (hit != null)
        {
            if (!isPlayerInRange)
            {
                playerTransform = hit.transform;
                FacingPlayerDirection();
            }
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
            playerTransform = null;
        }
    }

    private void UpdateSpriteDirection(int dirVal)
    {
        if (!flipSprite) return;

        Vector3 scale = transform.localScale;
        scale.x = originalScaleX * dirVal;
        transform.localScale = scale;
    }

    private void FacingPlayerDirection()
    {
        if (playerTransform == null || !flipSprite) return;

        if (Mathf.Abs(playerTransform.position.x - transform.position.x) > 0.1f)
        {
            if (playerTransform.position.x > transform.position.x)
            {
                UpdateSpriteDirection(1);
            }
            else
            {
                UpdateSpriteDirection(-1);
            }
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        EnemyProjectile bullet = projectile.GetComponent<EnemyProjectile>();

        if (bullet != null)
        {
            float shootDir = transform.localScale.x > 0 ? 1 : -1;
            bullet.SetDirection(Vector2.right * shootDir);
        }
    }

    private void StopMovement()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujamos una línea en la escena para ver el rango de patrulla basado en su posición actual
        Gizmos.color = Color.green;
        float currentX = Application.isPlaying ? transform.position.x : transform.position.x;
        Gizmos.DrawLine(new Vector3(currentX - patrolDistance, transform.position.y, 0), new Vector3(currentX + patrolDistance, transform.position.y, 0));
        Gizmos.DrawWireSphere(new Vector3(currentX - patrolDistance, transform.position.y, 0), 0.15f);
        Gizmos.DrawWireSphere(new Vector3(currentX + patrolDistance, transform.position.y, 0), 0.15f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
