using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    void Update()
    {
        // Nhấn phím C để đặt checkpoint tại vị trí hiện tại
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckpointManager.instance.SetCheckpoint(transform.position);
        }
    }

    public void Respawn()
    {
        Vector3 respawnPosition = CheckpointManager.instance.GetLastCheckpoint();

        // Nếu chưa có checkpoint nào được lưu, hồi sinh ở vị trí ban đầu
        if (respawnPosition != Vector3.zero)
        {
            transform.position = respawnPosition;
        }
        else
        {
            Debug.Log("Chưa có checkpoint nào được lưu.");
        }
    }

    // Hàm này gọi khi player chết
    public void PlayerDied()
    {
        Respawn();
    }
}