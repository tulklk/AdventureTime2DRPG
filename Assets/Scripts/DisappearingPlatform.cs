using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    private bool isTriggered = false; // Kiểm tra xem player đã chạm vào chưa
    public float disappearDelay = 1f; // Sau 1s chạm vào thì biến mất

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // Đánh dấu đã bị kích hoạt
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearDelay);
        gameObject.SetActive(false); // Ẩn platform

        // Nếu muốn platform xuất hiện lại sau 5 giây, bỏ comment dòng dưới
        Invoke("Reappear", 5f);
    }

    void Reappear()
    {
        isTriggered = false; // Reset trạng thái
        gameObject.SetActive(true); // Hiện lại platform
    }
}