using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text loadingText;

    private void Start()
    {
        //if (!PlayerPrefs.HasKey("HasSeenTutorial")) // Nếu chưa xem tutorial
        //{
        //    PlayerPrefs.SetInt("HasSeenTutorial", 100);
        //    PlayerPrefs.Save();
        //    StartCoroutine(LoadTutorialWithDelay());
        //    return;
        //}

        int levelId = PlayerPrefs.GetInt("LevelToLoad", 1);
        string levelName = "Level " + levelId;

        Debug.Log("Đang tải Level: " + levelName);
        StartCoroutine(LoadLevelWithStep(levelName, 0.08f, 5f));
    }

    //private IEnumerator LoadTutorialWithDelay()
    //{
    //    yield return StartCoroutine(LoadLevelWithStep("Tutorial", 0.08f, 5f));
    //}


    private IEnumerator LoadLevelWithStep(string levelName, float step, float totalTime)
    {
        float progress = 0f;
        float delay = totalTime / (1f / step);
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
        operation.allowSceneActivation = false;

        while (progress < 1f)
        {
            progress += step;
            progress = Mathf.Clamp01(progress);

            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (loadingText != null)
            {
                loadingText.text = $"Loading... {Mathf.FloorToInt(progress * 100)}%";
            }

            yield return new WaitForSeconds(delay);
        }

        operation.allowSceneActivation = true;
    }
}