using UnityEngine;

public class BoneEmenyController : MonoBehaviour
{
    public float speed = 2f;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 1f;

    public HealthbarBehaviour Healthbar;
    private Animator animator;
    private bool isDead = false;

    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;
    private Vector3 originalScale;

    private PlayerController playerController;

    void Start()
    {
        currentHealth = maxHealth;
        Healthbar.SetHealth(currentHealth, maxHealth);
        animator = GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Phát hiện và đuổi theo Player
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            animator.SetBool("isFlying", true);
            animator.SetBool("isAttacking", false);
           
            MoveTowardsPlayer();


        }
        // Tấn công khi tới gần Player
        else if (distanceToPlayer <= attackRange)
        {
            animator.SetBool("isFlying", false);
            animator.SetBool("isAttacking", true);

            // Tấn công với cooldown
            if (Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            animator.SetBool("isFlying", false);
            animator.SetBool("isAttacking", false);
        }
    }

    void MoveTowardsPlayer()
    {
        // Di chuyển về phía Player
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Quay đầu về phía Player mà không đổi kích thước
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = player.position.x < transform.position.x;
        }
    }


    void AttackPlayer()
    {
        if (playerController != null)
        {
            playerController.TakeDamage(10);
            animator.SetBool("isAttacking", true);
            Debug.Log("Player bị tấn công! -10 HP");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Healthbar.SetHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2f); // Xóa sau 2 giây
    }
}
