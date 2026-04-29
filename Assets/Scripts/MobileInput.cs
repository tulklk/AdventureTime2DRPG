using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour
{
    private PlayerController playerController;
    private bool moveLeft = false;
    private bool moveRight = false;
    private bool isJumping = false;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Nút di chuyển sang trái
    public void OnLeftButtonDown()
    {
        moveLeft = true;
    }
    public void OnLeftButtonUp()
    {
        moveLeft = false;
    }

    // Nút di chuyển sang phải
    public void OnRightButtonDown()
    {
        moveRight = true;
    }
    public void OnRightButtonUp()
    {
        moveRight = false;
    }

    // Nút nhảy
    public void OnJumpButtonDown()
    {
        isJumping = true;
    }
    public void OnJumpButtonUp()
    {
        isJumping = false;
    }

    private void Update()
    {
        if (moveLeft)
        {
            playerController.Move(-1); // Di chuyển sang trái
        }
        else if (moveRight)
        {
            playerController.Move(1); // Di chuyển sang phải
        }
        else
        {
            playerController.Move(0); // Dừng lại
        }

        if (isJumping)
        {
            playerController.Jump();
        }
    }
}