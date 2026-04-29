using UnityEngine;

public class SkeletonWarriorAI : MonoBehaviour
{
    public float moveSpeed = 2f; // Tốc độ di chuyển
    public float patrolDistance = 5f; // Khoảng cách tuần tra
    public float attackRange = 1.5f; // Khoảng cách để tấn công
    public float attackCooldown = 2f; // Thời gian hồi chiêu tấn công

    private Vector3 startPoint;
    private bool movingRight = true;
    private float attackTimer = 0f;
    private Animator animator;

    void Start()
    {
        startPoint = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Patrol();
        DetectAndAttack();
    }

    void Patrol()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x >= startPoint.x + patrolDistance)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= startPoint.x - patrolDistance)
            {
                movingRight = true;
                Flip();
            }
        }

        animator.SetBool("isWalking", true); // Kích hoạt animation đi bộ
    }

    void DetectAndAttack()
    {
        // Phát hiện mục tiêu trong phạm vi tấn công
        RaycastHit2D hit = Physics2D.Raycast(transform.position, movingRight ? Vector2.right : Vector2.left, attackRange);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            animator.SetBool("isWalking", false); // Dừng animation đi bộ
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown; // Đặt lại thời gian hồi chiêu
            }
        }

        attackTimer -= Time.deltaTime;
    }

    void Attack()
    {
        // Kích hoạt animation tấn công
        animator.SetTrigger("Attack");
        Debug.Log("Skeleton Warrior is attacking!");
        // Thêm logic gây sát thương ở đây
    }

    void Flip()
    {
        // Lật nhân vật khi đổi hướng
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
