using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f; // Điều chỉnh phạm vi tấn công nhỏ hơn
    public int maxHealth = 100;

    private int currentHealth;
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isAttacking = false;

    private enum BossState { Idle, Walk, Attack, Jump, Spin, Sleep }
    private BossState currentState = BossState.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (currentHealth > 0)
        {
            switch (currentState)
            {
                case BossState.Idle:
                    animator.Play("Idle");
                    yield return new WaitForSeconds(2f);
                    if (PlayerInDetectionRange())
                        ChangeState(BossState.Walk);
                    break;

                case BossState.Walk:
                    WalkTowardsPlayer();
                    yield return new WaitForSeconds(0.2f); // Giảm thời gian để kiểm tra liên tục
                    break;

                case BossState.Attack:
                    Attack();
                    yield return new WaitForSeconds(1f);
                    ChangeState(BossState.Idle);
                    break;

                case BossState.Sleep:
                    Sleep();
                    yield return new WaitForSeconds(5f);
                    ChangeState(BossState.Idle);
                    break;
            }
            yield return null;
        }
    }

    void ChangeState(BossState newState)
    {
        if (currentHealth <= 0) return;

        currentState = newState;
    }

    void WalkTowardsPlayer()
    {
        if (player == null) return;

        animator.Play("Walk");

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            ChangeState(BossState.Attack);
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.Play("Attack");
        rb.velocity = Vector2.zero; // Dừng di chuyển khi tấn công

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Boss attacks player!");
        }

        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    void Sleep()
    {
        animator.Play("Sleep");
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRange && !isAttacking)
        {
            ChangeState(BossState.Attack);
        }
        else if (distanceToPlayer < detectionRange && distanceToPlayer > attackRange)
        {
            ChangeState(BossState.Walk);
        }
        else
        {
            ChangeState(BossState.Idle);
        }
    }

    bool PlayerInDetectionRange()
    {
        return player != null && Vector2.Distance(transform.position, player.position) < detectionRange;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss is defeated!");
        animator.Play("Die");
        Destroy(gameObject, 2f);
    }
}
