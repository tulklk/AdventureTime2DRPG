using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //biến di chuyển
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;
    private float horizontal;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    //biến check ground
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    //biến check tường
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallLCheck;

    //biến hp
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [SerializeField] public HealthBar healthBar;

    [SerializeField] private float shootSpeedMultiplier = 0.3f; 
    private bool isShooting = false;

    //Joystick
    public Joystick joystick;
    public float jumpThreshold = 0.5f;
    public Button jumpButton;
    public Button healButton;
    public Button wallJumpButton;

    private bool isGround;
    private bool isOnStairs = false;
    [SerializeField] private float stairSpeed = 4f;

    //biến trượt tường
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    //biến nhảy trên tường
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f,16f);



    private Animator animator;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private AudioManager audioManager;

    private bool canShoot = false;  

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        
    }

    void Start()
    {
        if (jumpButton != null)
        {
            EventTrigger trigger = jumpButton.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown; // Kích hoạt ngay khi nhấn nút
            entry.callback.AddListener((data) => { HandleJumpUI(); });

            trigger.triggers.Add(entry);
        }

        if (wallJumpButton != null)
        {
            wallJumpButton.onClick.AddListener(WallJumpFromButton);
        }

        // Gán sự kiện cho nút UI
        if (healButton != null)
            healButton.onClick.AddListener(UseHealthPotion);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
    }

    void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Xử lý phím Space đồng bộ với UI Button
        if (Input.GetButtonDown("Jump") && isGround)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ResetPlayerPrefs();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseHealthPotion();
        }


        if (gameManager.IsGameOver() || gameManager.IsGameWin()) return;

        if (isOnStairs)
        {
            HandleStairMovement();
        }
        else
        {
            HandleMovement();
            HandleJump();

        }

        UpdateAnimation();

        
        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
        
        


    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            HandleMovement();
        }

        
    }

    public void SetShootingState(bool shooting)
    {
        isShooting = shooting;
    }

    //hàm di chuyển
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Lấy giá trị từ bàn phím
        moveInput += joystick.Horizontal; // Cộng thêm giá trị từ Joystick

        // Đảm bảo giá trị nằm trong khoảng [-1, 1] để tránh bị vượt quá khi cộng cả hai giá trị
        moveInput = Mathf.Clamp(moveInput, -1f, 1f);

        if (moveInput != 0 && !gameManager.IsTimerRunning())
        {
            gameManager.StartTimer(); // Bắt đầu đếm giờ khi người chơi di chuyển
        }

        float currentSpeed = isShooting ? moveSpeed * shootSpeedMultiplier : moveSpeed;
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }


    private void HandleJump()

    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        bool isKeyJump = Input.GetButtonDown("Jump"); // Kiểm tra phím nhảy

        if (isKeyJump && isGround)
        {
            Jump();
        }

        
    }

// Gán sự kiện nhảy cho nút UI (Cập nhật lại)
    

// Hàm xử lý nhảy từ UI
    private void HandleJumpUI()

    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (isGround) // Kiểm tra ngay lập tức
        {
            Jump();
        }
    }

// Hàm nhảy
    public void Jump()
    {
        //if (isGround)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //    audioManager.PlayJumpSound();
        //}
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        audioManager.PlayJumpSound();
    }



//hàm di chuyển cầu thang


private void HandleStairMovement()
    {
        // Nhận input từ bàn phím
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Nhận input từ Joystick
        float joystickHorizontal = joystick.Horizontal;
        float joystickVertical = joystick.Vertical;

        // Kết hợp cả hai nguồn input
        float finalHorizontal = moveHorizontal != 0 ? moveHorizontal : joystickHorizontal;
        float finalVertical = moveVertical != 0 ? moveVertical : joystickVertical;

        // Tạo vector di chuyển
        Vector2 movement = new Vector2(finalHorizontal, finalVertical);
        rb.velocity = movement * stairSpeed;
        rb.gravityScale = 0; // Vô hiệu hóa trọng lực khi trên cầu thang

        // Đổi hướng nhân vật
        if (finalHorizontal > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (finalHorizontal < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.velocity.x) > 0.1f;
        bool isJumping = !isGround && !isOnStairs; // Không coi là nhảy nếu đang trên cầu thang
        bool isClimbing = isOnStairs && Mathf.Abs(rb.velocity.y) > 0.1f;

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        //animator.SetBool("isClimbing", isClimbing);
    }


    //hàm nhận damge
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            gameManager.GameOver();
        }
    }

    //hàm reset dữ liệu
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs has been reset!");
    }


    //hàm tiếp xúc tương tác với các item có tags
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("StairTrigger"))
        {
            isOnStairs = true;
            rb.gravityScale = 0; // Vô hiệu hóa trọng lực khi trên cầu thang
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("StairTrigger"))
        {
            isOnStairs = false;
            rb.gravityScale = 5; // Khôi phục trọng lực khi rời khỏi cầu thang
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalked()
    {
        return Physics2D.OverlapCircle(wallLCheck.position,0.2f, wallLayer);

    }

    //hàm trượt tường
    private void WallSlide()
    {
        if (IsWalked() && !IsGrounded())
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    //private void WallJump()
    //{
    //    if (isWallSliding)
    //    {
    //        isWallJumping = false;
    //        wallJumpingDirection = -transform.localScale.x;
    //        wallJumpingCounter = wallJumpingTime;

    //        CancelInvoke(nameof(StopWallJumping));
    //    }
    //    else
    //    {
    //        wallJumpingCounter -= Time.deltaTime;
    //    }

    //    if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
    //    {
    //        isWallJumping = true;
    //        rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
    //        wallJumpingCounter = 0f;

    //        if (transform.localScale.x != wallJumpingDirection)
    //        {
    //            isFacingRight = !isFacingRight;
    //            Vector3 localScale = transform.localScale;
    //            localScale.x = -1f;
    //            transform.localScale = localScale;
    //        }

    //        Invoke(nameof(StopWallJumping), wallJumpingDuration);
    //    }

    //}
    public void WallJumpFromButton()
    {
        WallJump(true); // Gọi hàm WallJump và truyền tham số true để biết là từ UI Button
    }

    private void WallJump(bool fromUIButton = false)
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

        // Nếu nhấn nút UI hoặc nhấn phím Jump thì thực hiện WallJump
        if ((fromUIButton || Input.GetButtonDown("Jump")) && wallJumpingCounter > 0f)
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


    //private void WallJump()
    //{
    //    if (isWallSliding)
    //    {
    //        // Khi bám vào tường, cho phép nhảy tường
    //        isWallJumping = false;
    //        wallJumpingDirection = -transform.localScale.x;
    //        wallJumpingCounter = wallJumpingTime; // Reset bộ đếm

    //        CancelInvoke(nameof(StopWallJumping)); // Hủy invoke nếu có
    //    }
    //    else
    //    {
    //        wallJumpingCounter -= Time.deltaTime; // Giảm dần bộ đếm khi không bám tường
    //    }

    //    bool isKeyJump = Input.GetButtonDown("Jump"); // Kiểm tra phím Jump

    //    // Chỉ nhảy nếu chưa nhảy trước đó
    //    if (isKeyJump && isWallSliding && !isWallJumping)
    //    {
    //        PerformWallJump();
    //    }
    //}

    //public void PerformWallJump()
    //{
    //    if (wallJumpingCounter > 0f)
    //    {
    //        isWallJumping = true; // Đánh dấu là đã nhảy để tránh spam
    //        rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
    //        wallJumpingCounter = 0f; // Reset bộ đếm ngay sau khi nhảy

    //        if (transform.localScale.x != wallJumpingDirection)
    //        {
    //            isFacingRight = !isFacingRight;
    //            Vector3 localScale = transform.localScale;
    //            localScale.x *= -1;
    //            transform.localScale = localScale;
    //        }

    //        Invoke(nameof(StopWallJumping), wallJumpingDuration); // Kết thúc nhảy sau thời gian quy định
    //    }
    //}


    //private void StopWallJumping()
    //{
    //    isWallJumping = false;
    //}
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void HealToFull()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth); // Cập nhật lại thanh máu
        Debug.Log("Máu đã hồi đầy!");
    }

   
    public void UseHealthPotion()
    {
        int potionCount = PlayerPrefs.GetInt("HealthPotionCount", 0);

        if (potionCount > 0)
        {
            // Hồi máu đầy
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth); // Cập nhật thanh máu
            Debug.Log("Máu đã hồi đầy!");

            // Giảm số lượng bình máu
            potionCount--;
            PlayerPrefs.SetInt("HealthPotionCount", potionCount);
            PlayerPrefs.Save();
            audioManager.PlayhealHpSound();

            Debug.Log("Đã sử dụng bình máu. Số lượng còn lại: " + potionCount);
        }
        else
        {
            Debug.Log("Không có bình máu để sử dụng!");
        }
    }

    public void Move(float direction)
    {
        float currentSpeed = isShooting ? moveSpeed * shootSpeedMultiplier : moveSpeed;
        rb.velocity = new Vector2(direction * currentSpeed, rb.velocity.y);
    }


}
