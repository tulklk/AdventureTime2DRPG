using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1f; // Tốc độ bay lên
    public float destroyTime = 1f; // Thời gian tồn tại của text
    private TextMeshPro textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        Destroy(gameObject, destroyTime); // Hủy text sau thời gian tồn tại
    }

    void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }

    public void SetText(int damage)
    {
        textMesh.text = damage.ToString();
    }
}