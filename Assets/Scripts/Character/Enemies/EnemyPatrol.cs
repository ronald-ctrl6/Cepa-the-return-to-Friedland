using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool startMovingRight = true;
    [SerializeField] private bool flipSprite = true;

    private Rigidbody2D rb;
    private int direction;
    private float originalScaleX;

    private void Awake()
{
    rb = GetComponent<Rigidbody2D>();

    if (rb == null)
    {
        Debug.LogError("No encontré el Rigidbody2D");
    }

    direction = startMovingRight ? 1 : -1;
    originalScaleX = Mathf.Abs(transform.localScale.x);
}

    private void Start()
    {
        UpdateSpriteDirection();
    }

    private void FixedUpdate()
    {
        if (leftPoint == null || rightPoint == null)
        {
            StopMovement();
            return;
        }

        MoveEnemy();
        CheckPatrolLimits();
    }

    private void MoveEnemy()
{
    if (rb != null)
    {
        Vector2 newPosition = rb.position + Vector2.right * direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
}

    private void CheckPatrolLimits()
    {
        if (direction > 0 && transform.position.x >= rightPoint.position.x)
        {
            ChangeDirection(-1);
        }
        else if (direction < 0 && transform.position.x <= leftPoint.position.x)
        {
            ChangeDirection(1);
        }
    }

    private void ChangeDirection(int newDirection)
    {
        direction = newDirection;
        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        if (!flipSprite)
        {
            return;
        }

        Vector3 scale = transform.localScale;
        scale.x = originalScaleX * direction;
        transform.localScale = scale;
    }

    private void StopMovement()
{
    if (rb != null)
    {
        rb.velocity = Vector2.zero;
    }
}

    private void OnDrawGizmosSelected()
    {
        if (leftPoint == null || rightPoint == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(leftPoint.position, rightPoint.position);
        Gizmos.DrawWireSphere(leftPoint.position, 0.15f);
        Gizmos.DrawWireSphere(rightPoint.position, 0.15f);
    }
}