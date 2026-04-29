using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;
    private GameManager gameManager;

    [SerializeField] private float fallSpeed = 5f; // Tăng tốc độ rơi nhanh

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Ban đầu không rơi

        // Tìm GameManager trong scene
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    // Khi Player đi vào vùng kích hoạt
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasFallen)
        {
            rb.gravityScale = 1; // Kích hoạt trọng lực
            rb.velocity = new Vector2(0, -fallSpeed); // Rơi ngay lập tức với tốc độ cao
            hasFallen = true;
        }
    }

    // Khi chạm vào Player => Gọi GameOver
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player hit! Game Over!");
            gameManager.GameOver();
        }
    }
}