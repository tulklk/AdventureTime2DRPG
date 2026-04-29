using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;      
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private GameObject confirmationDialog;

    private void Start()
    {
        // Ẩn bảng xác nhận xóa dữ liệu khi mới vào
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false);
        }
    }
    public void OptionGame()
    {
        Debug.Log("OptionGame Called");
        // Hiển thị OptionMenu và ẩn MainMenu
        if (mainMenu != null && optionMenu != null)
        {
            mainMenu.SetActive(false);
            optionMenu.SetActive(true);
        }
    }
    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to MainMenu");
        // Hiển thị MainMenu và ẩn OptionMenu
        if (mainMenu != null && optionMenu != null)
        {
            mainMenu.SetActive(true);
            optionMenu.SetActive(false);
        }
    }

    // Hiển thị bảng xác nhận xóa dữ liệu
    public void ShowDeleteConfirmation()
    {
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(true);
        }
    }

    // Ẩn bảng xác nhận xóa dữ liệu
    public void HideDeleteConfirmation()
    {
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false);
        }
    }

    // Xóa toàn bộ dữ liệu PlayerPrefs
    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Đã xóa toàn bộ dữ liệu PlayerPrefs");
        HideDeleteConfirmation(); // Ẩn bảng xác nhận sau khi xóa
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
}
