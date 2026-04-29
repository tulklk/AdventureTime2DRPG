using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Tốc độ di chuyển
    [SerializeField] private float distance = 5f; // Khoảng cách giới hạn di chuyển
    [SerializeField] private int maxHealth = 100; // Máu tối đa của Enemy
    
    public HealthbarBehaviour Healthbar;


    private int currentHealth; // Máu hiện tại
    private Vector3 startPos; // Vị trí bắt đầu
    private bool movingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        currentHealth = maxHealth; // Khởi tạo máu ban đầu
        Healthbar.SetHealth(currentHealth,maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;

        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    // Method quay đầu khi chạm 2 đầu giới hạn
    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Nhận sát thương khi bị trúng đạn
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Giảm máu của Enemy
        Healthbar.SetHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die(); // Gọi hàm Die() nếu máu <= 0
        }
    }

    // Xử lý khi Enemy chết
    protected virtual void Die()
    {
        Destroy(gameObject); // Hủy Enemy
    }
}