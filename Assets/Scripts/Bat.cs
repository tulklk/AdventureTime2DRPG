using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Transform player; // Gán Player vào Inspector
    public float detectionRange = 5f; // Khoảng cách phát hiện
    public float attackRange = 1.5f; // Khoảng cách tấn công
    public float moveSpeed = 2f; // Tốc độ bay
    public int hp = 50; // Máu của dơi
    private Vector3 originalScale;


    private Animator anim;
    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>(); // Lấy Animator
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isDead) return; // Nếu đã chết thì không làm gì nữa

        float distance = Vector2.Distance(transform.position, player.position);

        // Nếu trong phạm vi phát hiện thì bay tới player
        if (distance <= detectionRange && distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else if (distance <= attackRange && !isAttacking) // Nếu đã gần player, bắt đầu tấn công
        {
            StartCoroutine(AttackPlayer());
        }
    }

    // Hàm giúp dơi di chuyển về phía player
    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // Giữ nguyên kích thước và chỉ thay đổi hướng
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Hướng sang phải
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Hướng sang trái
        }
    }



    // Hàm xử lý tấn công
    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        anim.SetTrigger("Attack"); // Chạy animation tấn công

        yield return new WaitForSeconds(1f); // Giả lập thời gian tấn công

        // Gọi hàm gây sát thương cho player ở đây (nếu có)
        Debug.Log("Bat attacked the player!");

        isAttacking = false;
    }

    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    // Hàm xử lý khi dơi chết
    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die"); // Chạy animation chết
        GetComponent<Collider2D>().enabled = false; // Tắt collider
        this.enabled = false; // Tắt script

        Destroy(gameObject, 2f); // Xóa object sau 2s
    }
}
