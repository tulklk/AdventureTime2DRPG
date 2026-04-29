using UnityEngine;
using UnityEngine.UI;

public class RopeSwingMovement : MonoBehaviour
{
    public float climbSpeed = 2f; // Tốc độ di chuyển lên/xuống dây
    public float jumpForce = 10f; // Lực nhảy khi thoát khỏi dây
    public float horizontalJumpForce = 20f;

    private Rigidbody2D rb;
    private bool isOnRope = false;
    private Transform currentRope;
    public Transform jumpDirection;

    public Button jumpButton; // Button nhảy
    public Joystick joystick; // Joystick di chuyển

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpButton.onClick.AddListener(JumpFromRope);
    }

    void Update()
    {
        if (isOnRope)
        {
            HandleRopeMovement();
        }
    }

    private void HandleRopeMovement()
    {
        // Vô hiệu hóa trọng lực khi đang đu dây
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        // Di chuyển lên/xuống dây
        float vertical = Input.GetAxis("Vertical") + joystick.Vertical;
        transform.Translate(Vector2.up * vertical * climbSpeed * Time.deltaTime);

        // Xoay mặt player khi di chuyển ngang
        float horizontal = Input.GetAxis("Horizontal") + joystick.Horizontal;
        if (horizontal > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontal < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Nhảy khỏi dây bằng phím Space
        //if (Input.GetButtonDown("Jump"))
        //{
        //    Vector2 jumpDir = (jumpDirection.position - transform.position).normalized;

        //    // Điều chỉnh lại vận tốc nhảy
        //    float jumpVelocityX = horizontalJumpForce * jumpDir.x;
        //    float jumpVelocityY = jumpForce;

        //    rb.velocity = new Vector2(jumpVelocityX, jumpVelocityY);
        //    rb.gravityScale = 5; // Bạn có thể thử tăng lên 6 hoặc 7 nếu cần rơi nhanh hơn
        //    ExitRope();
        //}
        JumpFromRope();

    }

    public void JumpFromRope()
    {
        if (isOnRope)
        {
            Vector2 jumpDir = (jumpDirection.position - transform.position).normalized;

            // Điều chỉnh lại vận tốc nhảy
            float jumpVelocityX = horizontalJumpForce * jumpDir.x;
            float jumpVelocityY = jumpForce;

            rb.velocity = new Vector2(jumpVelocityX, jumpVelocityY);
            rb.gravityScale = 5; // Bạn có thể thử tăng lên 6 hoặc 7 nếu cần rơi nhanh hơn
            ExitRope();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi player chạm vào vùng đu dây
        if (collision.CompareTag("RopeZone"))
        {
            isOnRope = true;
            currentRope = collision.transform;
            transform.SetParent(currentRope); // Gắn player vào rope để di chuyển theo
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Khi player thoát khỏi vùng đu dây
        if (collision.CompareTag("RopeZone"))
        {
            ExitRope();
        }
    }

    private void ExitRope()
    {
        isOnRope = false;
        transform.SetParent(null); // Hủy bỏ quan hệ cha-con với rope
        rb.gravityScale = 5; // Bật lại trọng lực
        transform.rotation = Quaternion.identity;
    }
}
