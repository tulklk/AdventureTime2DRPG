using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBoundary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có phải là player
        if (collision.CompareTag("Player"))
        {
            // Gọi hàm GameOver từ GameManager
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameOver();
            }
        }
    }
}
