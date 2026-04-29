using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    public GameObject impactEffect;
    public int damage = 20; // Sát thương của viên đạn
    public float effectDuration = 0.5f; // Thời gian hiển thị hiệu ứng

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Xác định hướng bay của viên đạn theo hướng của nhân vật
        float direction = transform.localScale.x;

        rb.velocity = new Vector2(projectileSpeed * direction, 0f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Tạo hiệu ứng va chạm
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);

            // Hủy hiệu ứng sau một thời gian
            Destroy(effect, effectDuration);

            // Gọi phương thức TakeDamage trên Enemy
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Hủy viên đạn
            Destroy(gameObject);
        } else if (collision.CompareTag("BoneEnemy"))
        {
            // Tạo hiệu ứng va chạm
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);

            // Hủy hiệu ứng sau một thời gian
            Destroy(effect, effectDuration);

            // Gọi phương thức TakeDamage trên Enemy
            BoneEmenyController bone = collision.GetComponent<BoneEmenyController>();
            if (bone != null)
            {
                bone.TakeDamage(damage);
            }

            // Hủy viên đạn
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Boss"))
        {
            // Tạo hiệu ứng va chạm
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);

            // Hủy hiệu ứng sau một thời gian
            Destroy(effect, effectDuration);

            // Gọi phương thức TakeDamage trên Enemy
            BossController boss = collision.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(40);
            }

            // Hủy viên đạn
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        } 
    }
}