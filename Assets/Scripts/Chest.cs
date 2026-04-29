using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    public GameObject coinPrefab;  
    public GameObject gunPrefab;
    public GameObject healPotionPrefab;
    public int coinAmount = 3;    
    public int gunAmount = 1;
    public int potionAmount = 1;
    public Transform spawnPoint;   

    private bool isOpened = false;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Lấy Animator từ GameObject
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpened && collision.CompareTag("Player"))
        {
            isOpened = true;
            animator.SetTrigger("Open"); // Chạy animation mở rương
            Invoke("SpawnCoins", 2f);
            Invoke("SpawnGun",2f);
            Invoke("SpawnHealPotion", 2f);
            Invoke("StopAnimation", 1f); // Dừng animation sau khi mở
        }
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 randomPos = spawnPoint.position + new Vector3(Random.Range(-1f, 1f), 1f, 0);
            GameObject coin = Instantiate(coinPrefab, randomPos, Quaternion.identity);

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                Vector2 forceDirection = new Vector2(Random.Range(-2f, 2f), Random.Range(4f, 6f));
                rb.AddForce(forceDirection, ForceMode2D.Impulse);
            }
        }
    }

    private void SpawnGun()
    {
        for (int i = 0; i < gunAmount; i++)
        {
            Vector3 randomPos = spawnPoint.position + new Vector3(Random.Range(-1f, 1f), 1f, 0);
            GameObject coin = Instantiate(gunPrefab, randomPos, Quaternion.identity);

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                Vector2 forceDirection = new Vector2(Random.Range(-2f, 2f), Random.Range(4f, 6f));
                rb.AddForce(forceDirection, ForceMode2D.Impulse);
            }
        }
    }
    private void SpawnHealPotion()
    {
        for (int i = 0; i < potionAmount; i++)
        {
            Vector3 randomPos = spawnPoint.position + new Vector3(Random.Range(-1f, 1f), 1f, 0);
            GameObject coin = Instantiate(healPotionPrefab, randomPos, Quaternion.identity);

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                Vector2 forceDirection = new Vector2(Random.Range(-2f, 2f), Random.Range(4f, 6f));
                rb.AddForce(forceDirection, ForceMode2D.Impulse);
            }
        }
    }

    private void StopAnimation()
    {
        animator.enabled = false; // Tắt Animator để animation dừng lại
    }
}