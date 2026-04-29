using UnityEngine;
using UnityEngine.UI;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject curentTeleporter;
    public Button teleportButton; // Kéo Button UI vào đây

    private void Start()
    {
        if (teleportButton != null)
        {
            teleportButton.gameObject.SetActive(false); // Ẩn nút lúc đầu
            teleportButton.onClick.AddListener(TeleportPlayer); // Gán sự kiện cho nút
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Dịch chuyển bằng phím T
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        if (curentTeleporter != null)
        {
            transform.position = curentTeleporter.GetComponent<Teleporter>().GetDestination().position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            curentTeleporter = collision.gameObject;
            if (teleportButton != null)
            {
                teleportButton.gameObject.SetActive(true); // Hiện nút
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if (collision.gameObject == curentTeleporter)
            {
                curentTeleporter = null;
                if (teleportButton != null)
                {
                    teleportButton.gameObject.SetActive(false); // Ẩn nút
                }
            }
        }
    }
}