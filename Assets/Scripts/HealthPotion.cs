using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Lấy số lượng hiện tại và tăng lên
            int potionCount = PlayerPrefs.GetInt("HealthPotionCount", 0);
            potionCount++;
            PlayerPrefs.SetInt("HealthPotionCount", potionCount);
            PlayerPrefs.Save();

            Debug.Log("Đã nhặt bình máu! Số lượng hiện tại: " + potionCount);

            // Cập nhật UI bằng cách gọi GameManager
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.UpdateHealthPotionCount();

            // Phát âm thanh nhặt vật phẩm
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager.PlayCoinSound();

            // Xóa bình máu sau khi nhặt
            Destroy(gameObject);
        }
    }
}