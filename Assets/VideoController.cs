using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button skipButton; // Nút bỏ qua video
    public GameObject videoPanel; // Panel chứa video

    void Start()
    {
        // Kiểm tra nếu video đã xem trước đó
        if (PlayerPrefs.GetInt("VideoPlayed", 0) == 0)
        {
            // Nếu chưa xem, phát video
            videoPlayer.Play();
            skipButton.onClick.AddListener(SkipVideo);
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            // Nếu đã xem, ẩn video và tiếp tục trò chơi
            videoPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StartGame();
    }

    void SkipVideo()
    {
        StartGame();
    }

    void StartGame()
    {
        // Ẩn video và tiếp tục trò chơi
        videoPanel.SetActive(false);
        Time.timeScale = 1;

        // Đánh dấu là video đã được xem
        PlayerPrefs.SetInt("VideoPlayed", 1);
        PlayerPrefs.Save();
    }
}