using UnityEngine;

public class BossController : MonoBehaviour
{
    

    public float moveSpeed = 2.0f;
    public int maxHealth = 1000;
    private int currentHealth;
    public HealthBar healthBar;

    private bool isUpgraded = false;
    private bool isFinalForm = false;
    private bool isDead = false;

    private float attackCooldown = 2.0f;
    private float nextAttackTime = 0;
    private int damage;

    [SerializeField] private int normalDamage = 10;
    [SerializeField] private int upgradedDamage = 20;
    [SerializeField] private int finalDamage = 30;

    private Animator animator;
    public Transform player;
    public BossBarTrigger bossBarTrigger;
    private AudioManager audioManager;

    

    public GameObject portal;
    




    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
        healthBar.SetHealth(maxHealth);
        bossBarTrigger = FindObjectOfType<BossBarTrigger>();
        if (portal != null)
        {
            portal.SetActive(false);
        }
    }

    void Update()
    {
        if (isDead) return;
        FacePlayer();

        if (currentHealth <= 0)
        {
            if (!isUpgraded)
            {
                // Dạng Bình Thường chết, chuyển sang Nâng Cấp
                animator.SetTrigger("DeathA");
                isDead = true;
                Invoke("UpgradeBoss", 3.0f);
            }
            else if (isUpgraded && !isFinalForm)
            {
                // Dạng Nâng Cấp chết, chuyển sang Final Form
                animator.SetTrigger("DeathB");
                isDead = true;
                Invoke("FinalUpgrade", 1.0f);
            }
            else if (isFinalForm)
            {
                if (currentHealth <= 0)
                {
                    // Dạng Cuối Cùng chết, phá hủy
                    animator.SetTrigger("UpdateAmorBreak");
                    isDead = true;

                    // Tắt thanh máu thông qua BossBarTrigger
                    if (bossBarTrigger != null && bossBarTrigger.healthBar != null)
                    {
                        bossBarTrigger.healthBar.SetActive(false);
                    }

                    // Gọi sự kiện khi Boss bị đánh bại
                    if (bossBarTrigger != null)
                    {
                        bossBarTrigger.OnBossDefeated();
                    }

                    if (portal != null)
                    {
                        portal.SetActive(true);
                    }


                    Destroy(gameObject, 3.0f);
                    audioManager.PlayWinSound();
                }
            }

        }
        else
        {
            if (!isUpgraded)
            {
                NormalState();
            }
            else if (isUpgraded && !isFinalForm)
            {
                UpgradedState();
            }
            else if (isFinalForm)
            {
                FinalState();
            }
        }
    }
    void FacePlayer()
    {
        if (player != null)
        {
            // Lấy kích thước gốc của boss (giữ nguyên y và z)
            Vector3 scale = transform.localScale;

            // Nếu player ở bên trái => mặt boss quay sang trái
            if (player.position.x < transform.position.x)
            {
                scale.x = Mathf.Abs(scale.x); // Đảm bảo x luôn dương
            }
            else
            {
                scale.x = -Mathf.Abs(scale.x); // Đổi dấu x để lật mặt
            }

            // Áp dụng lại scale
            transform.localScale = scale;
        }
    }

    // ================= Dạng Bình Thường ====================
    void NormalState()
    {
        damage = 10;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > 2.0f)
        {
            // Di chuyển đến gần Player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
            animator.Play("IdleA");
        }
        else
        {
            // Ngẫu nhiên chọn đòn tấn công nếu đủ thời gian hồi chiêu
            if (Time.time >= nextAttackTime)
            {
                int attackType = Random.Range(0, 3);
                switch (attackType)
                {
                    case 0:
                        animator.SetTrigger("AttackA");
                        break;
                    case 1:
                        animator.SetTrigger("AttackB");
                        break;
                    case 2:
                        animator.SetTrigger("AttackC");
                        break;
                }
                AttackPlayer(normalDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    // ================= Dạng Nâng Cấp ====================
    void UpgradedState()
    {
        damage = 20;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > 2.0f)
        {
            // Đuổi theo Player bằng Run
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * 1.5f * Time.deltaTime;
            animator.Play("Run");
        }
        else
        {
            // Ngẫu nhiên chọn đòn tấn công nếu đủ thời gian hồi chiêu
            if (Time.time >= nextAttackTime)
            {
                int attackType = Random.Range(0, 2);
                switch (attackType)
                {
                    case 0:
                        animator.SetTrigger("HitA");
                        break;
                    case 1:
                        animator.SetTrigger("HitB");
                        break;
                }
                AttackPlayer(upgradedDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
    

    // ================= Dạng Cuối Cùng ====================
    void FinalState()
    {
        damage = 30;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > 2.0f)
        {
            // Di chuyển tới Player bằng UpgradeRun
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * 2.0f * Time.deltaTime;
            animator.Play("UpgradeRun");
        }
        else
        {
            // Ngẫu nhiên chọn đòn tấn công nếu đủ thời gian hồi chiêu
            if (Time.time >= nextAttackTime)
            {
                int attackType = Random.Range(0, 2);
                switch (attackType)
                {
                    case 0:
                        animator.SetTrigger("UpgradeAttackA");
                        break;
                    case 1:
                        animator.SetTrigger("UpgradeAttackB");
                        break;
                }
                AttackPlayer(finalDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
    //hàm tấn công
    void AttackPlayer(int attackDamage)
    {
        if (Vector2.Distance(transform.position, player.position) <= 2.0f)
        {
            player.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }
    // ================= Nâng Cấp Boss ====================
    void UpgradeBoss()
    {
        animator.SetTrigger("Reset");
        isUpgraded = true;
        isDead = false;
        currentHealth = maxHealth + 200; // Tăng HP khi nâng cấp
        healthBar.SetHealth(currentHealth);
    }

    void FinalUpgrade()
    {
        animator.SetTrigger("UpgradeIdle");
        isFinalForm = true;
        isDead = false;
        currentHealth = maxHealth + 500; // Tăng HP cho dạng cuối
        healthBar.SetHealth(currentHealth);
    }

    // ================= Nhận Sát Thương ====================
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth < 0)
            currentHealth = 0;
    }
}
