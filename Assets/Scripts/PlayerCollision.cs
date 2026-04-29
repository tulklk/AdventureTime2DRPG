using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager; //để lấy method từ class GameManager
    private AudioManager audioManager; //để lấy method từ class AudioManager
    private PlayerController playerController;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>(); //để lấy method từ class GameManager
        audioManager = FindAnyObjectByType<AudioManager>();//để lấy method từ class AudioManager
        playerController = GetComponent<PlayerController>();
    }
        
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);//xóa đồng xu khi chạm vào
            audioManager.PlayCoinSound();//khi đồng xu xóa sẽ xuất hiện effect sound của đồng xu
            gameManager.AddScore(1);
            
        }
        
        else if (collision.CompareTag("Trap") || collision.CompareTag("Enemy"))
        {
            //gameManager.GameOver();
            audioManager.PlayDamageSound();
            playerController.TakeDamage(20);
            
        }
        else if(collision.CompareTag("Gun"))
        {
          Destroy(collision.gameObject);
          audioManager.PlayCoinSound();

        }
        else if (collision.CompareTag("KeyLevel1"))
        {
            UnlockNewLevel();
            Destroy(collision.gameObject);//xóa key khi chạm vào
            gameManager.GameWin(1);
        }
        else if (collision.CompareTag("KeyLevel2"))
        {
            UnlockNewLevel();
            Destroy(collision.gameObject);//xóa key khi chạm vào
            gameManager.GameWin(2);
        }
        else if (collision.CompareTag("KeyLevel3"))
        {
            UnlockNewLevel();
            Destroy(collision.gameObject);//xóa key khi chạm vào
            gameManager.GameWin(3);
        }


    }

    void UnlockNewLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int reachedIndex = PlayerPrefs.GetInt("ReachedIndex", 0);

        if (currentLevel >= reachedIndex)
        {
            PlayerPrefs.SetInt("ReachedIndex", currentLevel + 1);
            PlayerPrefs.SetInt("UnlockedLevel", Mathf.Max(PlayerPrefs.GetInt("UnlockedLevel", 1), currentLevel + 1));
            PlayerPrefs.Save();
        }
    }

}
