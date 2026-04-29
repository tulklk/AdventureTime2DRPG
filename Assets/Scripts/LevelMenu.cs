using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class StarIconRow
{
    public List<Image> starImages;
}

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject selectMenu;
    public List<StarIconRow> starIcons;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            int levelIndex = i + 1;

            if (levelIndex <= unlockedLevel)
            {
                buttons[i].interactable = true;
                buttons[i].onClick.AddListener(() => OpenLevel(levelIndex));

                int stars = PlayerPrefs.GetInt("Stars_Level_" + levelIndex, 0);
                for (int j = 0; j < starIcons[i].starImages.Count; j++)
                {
                    starIcons[i].starImages[j].enabled = (j < stars);
                }
            }
            else
            {
                buttons[i].interactable = false;
                foreach (var star in starIcons[i].starImages)   
                {
                    star.enabled = false;
                }
            }
        }
    }




    public void OpenLevel(int levelId)
    {
        if (levelId == 1)
        {
            Debug.Log("Chuyển đến CutScene cho Level: " + levelId);
            SceneManager.LoadScene("CutScene");
        }
        else
        {
            Debug.Log("Chuyển đến LoadingScene với Level: " + levelId);
            PlayerPrefs.SetInt("LevelToLoad", levelId);
            PlayerPrefs.Save();
            SceneManager.LoadScene("LoadingScene");
        }
    }


    public void ReturnToMainMenu()
    {
        if (mainMenu != null && selectMenu != null)
        {
            mainMenu.SetActive(true);
            selectMenu.SetActive(false);
        }
    }
}