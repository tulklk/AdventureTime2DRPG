using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarTrigger : MonoBehaviour
{
    public GameObject healthBar;
    public BossController bossController;
    private AudioManager audioManager;
    private bool isBossActivated = false;
    public GameManager gameManager;

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.SetActive(false);
        }

        if (bossController != null)
        {
            bossController.enabled = false;
        }

        audioManager = FindObjectOfType<AudioManager>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBossActivated)
        {
            if (healthBar != null)
            {
                healthBar.SetActive(true);
            }

            if (bossController != null)
            {
                bossController.enabled = true;
                isBossActivated = true;
            }

            if (audioManager != null)
            {
                audioManager.PlayBossMusic(); 
            }

            if (gameManager != null)
            {
                gameManager.ResetTimer(900); // 15 phút = 900 giây
            }
        }
    }

    // Gọi khi boss chết
    public void OnBossDefeated()
    {
        if (audioManager != null)
        {
            audioManager.RestoreBackgroundMusic(); 
        }
    }
}