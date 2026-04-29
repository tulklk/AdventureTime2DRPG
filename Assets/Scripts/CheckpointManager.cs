using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    [SerializeField] private GameObject checkpointPrefab;  // Prefab của Checkpoint
    private Vector3 lastCheckpointPos;
    private GameObject currentCheckpoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Đặt checkpoint tại vị trí mới
    public void SetCheckpoint(Vector3 position)
    {
        if (currentCheckpoint != null)
        {
            Destroy(currentCheckpoint);  // Xoá checkpoint cũ
        }
        currentCheckpoint = Instantiate(checkpointPrefab, position, Quaternion.identity);
        lastCheckpointPos = position;
        Debug.Log("Checkpoint Created at: " + position);
    }

    // Lấy vị trí checkpoint gần nhất
    public Vector3 GetLastCheckpoint()
    {
        return lastCheckpointPos;
    }
}