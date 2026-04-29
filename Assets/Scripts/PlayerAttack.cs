using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAttack : MonoBehaviour
{
    public Transform firePosition;
    public GameObject projectile;
    public Animator animator;

    public Button readyToShootButton; // Nút bật chế độ sẵn sàng
    public Button shootButton; // Nút bắn súng


    private bool isReadyToShoot = false;
    private bool canShoot = true;
    private bool hasGun = false; // Kiểm tra xem người chơi đã nhặt súng chưa
    private PlayerController playerController;
    public AudioManager audioManager;

    //private void Awake()
    //{
    //    animator = GetComponent<Animator>();
    //    playerController = GetComponent<PlayerController>();
    //    audioManager = FindObjectOfType<AudioManager>();

    //    // Kiểm tra trạng thái đã nhặt súng từ PlayerPrefs
    //    if (PlayerPrefs.GetInt("HasGun", 0) == 1)
    //    {
    //        hasGun = true;
    //    }

    //}
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
        playerController = GetComponent<PlayerController>();

        if (PlayerPrefs.GetInt("HasGun", 0) == 1)
        {
            hasGun = true;
        }
        // Gán sự kiện cho nút UI
        if (readyToShootButton != null)
            readyToShootButton.onClick.AddListener(ToggleShootingMode);

        if (shootButton != null)
            shootButton.onClick.AddListener(Shoot);
    }


    
    private void Update()
    {
        // Bật chế độ sẵn sàng bằng bàn phím
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleShootingMode();
        }

        // Bắn bằng bàn phím
        if (Input.GetKeyDown(KeyCode.K))
        {
            Shoot();
        }
    }
    private void ToggleShootingMode()
    {
        if (!hasGun) return;

        isReadyToShoot = !isReadyToShoot;

        if (isReadyToShoot)
        {
            animator.SetTrigger("PlayerShoot"); // Kích hoạt animation bắn
        }
        else
        {
            animator.ResetTrigger("PlayerShoot"); // Hủy animation
            animator.Play("PlayerIdle"); // Chuyển về trạng thái mặc định
        }

        // Cập nhật trạng thái di chuyển
        if (playerController != null)
        {
            playerController.SetShootingState(isReadyToShoot);
        }
    }

    private void Shoot()
    {
        if (!hasGun || !isReadyToShoot || !canShoot) return;

        StartCoroutine(ShootWithDelay());
        audioManager.PlayGunSound();
    }

    //private IEnumerator ShootWithDelay()
    //{
    //    canShoot = false;
    //    yield return new WaitForSeconds(0.5f); // Giả lập thời gian delay giữa các phát bắn
    //    canShoot = true;
    //}
    IEnumerator ShootWithDelay()
    {
        canShoot = false;

        // Tạo viên đạn
       GameObject newProjectile = Instantiate(projectile, firePosition.position, Quaternion.identity);

       // Lấy hướng nhân vật
        float direction = transform.localScale.x;

       newProjectile.transform.localScale = new Vector3(direction, 1, 1);

      yield return new WaitForSeconds(0.5f);
       canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gun"))
        {
            hasGun = true;
           PlayerPrefs.SetInt("HasGun", 1); // Lưu trạng thái đã nhặt súng
            PlayerPrefs.Save(); // Lưu lại dữ liệu
           Destroy(collision.gameObject); // Xóa vật phẩm sau khi nhặt
       }
    }
}
