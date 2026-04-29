using UnityEngine;
using UnityEngine.UI; // Nếu dùng TextMeshPro thì thêm: using TMPro;

public class ItemTrigger : MonoBehaviour
{
    public GameObject teleportButton; // Kéo Button UI vào đây

    private void Start()
    {
        if (teleportButton != null)
        {
            teleportButton.SetActive(false); // Ẩn nút lúc đầu
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (teleportButton != null)
            {
                teleportButton.SetActive(true); // Hiện nút khi Player vào vùng
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (teleportButton != null)
            {
                teleportButton.SetActive(false); // Ẩn nút khi Player rời đi
            }
        }
    }
}