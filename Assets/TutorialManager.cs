using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer tutorialVideo;
    [SerializeField] private CanvasGroup fadeCanvas; // UI Canvas dùng để fade
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        if (tutorialVideo != null)
        {
            tutorialVideo.loopPointReached += OnTutorialFinished;
        }
        StartCoroutine(PreloadScene("Level 1")); // Tải trước Level 1 để tránh lag
    }

    private void OnTutorialFinished(VideoPlayer vp)
    {
        StartCoroutine(FadeAndLoadScene("Level 1"));
    }

    private IEnumerator PreloadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Không cho load ngay
        yield return null;
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}