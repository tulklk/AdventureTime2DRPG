using UnityEngine;
using UnityEngine.UI;

public class WallJumpController : MonoBehaviour
{
    public Button wallJumpButton; // Kéo thả Button từ Inspector vào đây

    private Rigidbody2D rb;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingCounter;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(10f, 15f);
    private float wallJumpingDirection;
    private bool isFacingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (wallJumpButton != null)
        {
            wallJumpButton.onClick.AddListener(WallJump);
        }
    }

    private void Update()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
    }

    private void WallJump()
    {
        if (wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x = -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}