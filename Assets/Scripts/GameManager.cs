using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
//using static Cinemachine.DocumentationSortingAttribute;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Video;


public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthPotionText;

    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameObject gameWinUi;
    [SerializeField] private GameObject pausedGameUi;
    [SerializeField] private GameObject optionMenuUi;
    //[SerializeField] private GameObject itemMenuUi;
    [SerializeField] private GameObject[] starImages;
    public Button checkpointButton;
    public Button pauseButton;


    public TextMeshProUGUI timerText;
    public float timeRemaining = 480f; // 5 phút
    private bool timerRunning = false;

    [SerializeField] private TextMeshProUGUI checkpointText;

    public VideoPlayer videoPlayer;
    private int totalStars = 0;
    private AudioManager audioManager;
    private bool isGameOver = false;
    private bool isGameWin = false;
    private bool isPaused = false;

    [SerializeField] private GameObject checkpointPrefab;
    private List<Vector3> checkpoints = new List<Vector3>();
    private Vector3 lastCheckpoint;
    private bool hasCheckpoint = false;
    // Số lượng checkpoint tối đa có thể đặt trong level hiện tại
    public int maxCheckpoints = 3;
    private int currentCheckpointCount = 0;
    public LayerMask restrictedZoneLayer;
    public TextMeshProUGUI checkpointMessage;

    //private void Awake()
    //{

    //}

    void Start()
    {
        if (checkpointButton != null)
            checkpointButton.onClick.AddListener(() => SetCheckpoint(GameObject.FindWithTag("Player").transform));
        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseGame);


        audioManager = FindAnyObjectByType<AudioManager>();
        currentCheckpointCount = maxCheckpoints; // Bắt đầu với số checkpoint tối đa
        UpdateCheckpointUI();
        UpdateScore();
        UpdateHealthPotionCount();
        
        gameOverUi.SetActive(false);
        gameWinUi.SetActive(false);
        pausedGameUi.SetActive(false);
        //itemMenuUi.SetActive(false);
       


    }

    void Update()
    {
        UpdateHealthPotionCount();

        if (Input.GetKeyDown(KeyCode.C))
        {
            SetCheckpoint(GameObject.FindWithTag("Player").transform);
        }

        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                timerText.text = "00:00";
                GameOver();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && !isGameWin)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
    }
    
    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer(float newTime)
    {
        timeRemaining = newTime;
        UpdateTimerUI();
    }

    private void UpdateCheckpointUI()
    {
        checkpointText.text = currentCheckpointCount.ToString();
    }


    public void SetMaxCheckpoints(int max)
    {
        maxCheckpoints = max;
        currentCheckpointCount = 0;
        checkpoints.Clear(); // Xóa checkpoint của level trước (nếu có)
    }

    public void SetCheckpoint(Transform playerTransform)
    {
        if (currentCheckpointCount <= 0)
        {
            Debug.Log("Hết checkpoint!");
            return;
        }

        Vector3 offset = new Vector3(0, -0.3f, 0);
        Vector3 checkpointPosition = playerTransform.position + offset;

        // Kiểm tra nếu checkpoint nằm trong vùng cấm
        //if (Physics.CheckSphere(checkpointPosition, 0.5f, restrictedZoneLayer))
        //{
        //    ShowMessage("You cannot place a checkpoint here !");
        //    return;
        //}
        if (Physics2D.OverlapBox(checkpointPosition, new Vector2(1f, 1f), 0f, restrictedZoneLayer))
        {
            ShowMessage("You cannot place a checkpoint here!");
            return;
        }

        checkpoints.Add(checkpointPosition);
        audioManager.PlayCheckPointSound();
        lastCheckpoint = checkpointPosition;
        currentCheckpointCount--; // Giảm số checkpoint còn lại

        Instantiate(checkpointPrefab, checkpointPosition, Quaternion.identity);

        UpdateCheckpointUI(); // Cập nhật UI sau khi đặt checkpoint
    }

    void ShowMessage(string message)
    {
        checkpointMessage.text = message;
        checkpointMessage.gameObject.SetActive(true);
        CancelInvoke("HideMessage");
        Invoke("HideMessage", 1f); // Ẩn sau 2 giây
    }

    void HideMessage()
    {
        checkpointMessage.gameObject.SetActive(false);
    }

    public void UpdateHealthPotionCount()
    {
        int potionCount = PlayerPrefs.GetInt("HealthPotionCount", 0);
        healthPotionText.text = "" + potionCount;
    }


    public void AddScore(int points)
    {
        if (!isGameOver && !isGameWin)
        {
            score += points;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        isGameOver = true;
        audioManager.StopEffectSounds();
        audioManager.StopBackgroundMusic();
        audioManager.PlayGameOverSound();

        // Nếu có checkpoint đã lưu, quay lại checkpoint gần nhất
        if (checkpoints.Count > 0)
        {
            // Nếu có checkpoint nhưng hết thời gian thì hiện Game Over UI và dừng game
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                gameOverUi.SetActive(true);
                Time.timeScale = 0;  // Dừng trò chơi lại
            }
            else
            {
                // Nếu còn thời gian, tiếp tục quay lại checkpoint
                StartCoroutine(RespawnAtCheckpoint());
            }
        }

        else
        {
            // Nếu không có checkpoint nào, xử lý thua game bình thường
            score = 0;
            Time.timeScale = 0;
            gameOverUi.SetActive(true);
        }

    }

    // Coroutine để delay 0.5 giây trước khi quay lại checkpoint
    private IEnumerator RespawnAtCheckpoint()
    {
        // Dừng thời gian trong lúc chờ spawn lại
        Time.timeScale = 0;

        // Đợi 0.5 giây
        yield return new WaitForSecondsRealtime(0.5f);

        // Sau 0.5 giây, đưa người chơi quay lại checkpoint
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = lastCheckpoint;

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.HealToFull();
        

        // Bật lại thời gian
        Time.timeScale = 1;
        isGameOver = false;

        // Phát nhạc nền sau khi quay lại checkpoint
        StartCoroutine(PlayBackgroundMusicWithDelay());
    }
    private IEnumerator PlayBackgroundMusicWithDelay()
    {
        yield return new WaitForSeconds(1f);
        audioManager.PlayBackGroundMusic();
    }

    public void GameWin(int level)
    {
        isGameWin = true;

        // Định nghĩa số sao đạt được theo từng level
        if (level == 1)
        {
            totalStars = score >= 16 ? 3 : (score >= 10 ? 2 : (score >= 5 ? 1 : 0));
        }
        else if (level == 2)
        {
            totalStars = score >= 63 ? 3 : (score >= 42 ? 2 : (score >= 21 ? 1 : 0));
        }
        else if (level == 3)
        {
            totalStars = score >= 78 ? 3 : (score >= 52 ? 2 : (score >= 26 ? 1 : 0));
        }

        // Hiển thị số sao đạt được
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].SetActive(i < totalStars);
        }

        // Lưu số sao đạt được cho level hiện tại
        PlayerPrefs.SetInt("Stars_Level_" + level, totalStars);

        // Kiểm tra và mở khóa level tiếp theo
        int currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (currentUnlockedLevel == level)
        {
            PlayerPrefs.SetInt("UnlockedLevel", level + 1);
        }
        PlayerPrefs.SetInt("LevelToLoad", level + 1); // Lưu level tiếp theo
        PlayerPrefs.Save();

        score = 0;
        Time.timeScale = 0;
        gameWinUi.SetActive(true);
        audioManager.StopBackgroundMusic();
        audioManager.PlayWinSound();
    }


    //public void ItemGame()
    //{
    //    itemMenuUi.SetActive(true);
    //    StartCoroutine(PlayVideoWithDelay(0.2f)); // Gọi Coroutine để phát video sau 1 giây
    //    Time.timeScale = 0;
    //}
    //private IEnumerator PlayVideoWithDelay(float delay)
    //{
    //    yield return new WaitForSecondsRealtime(delay); // Dùng WaitForSecondsRealtime để không bị ảnh hưởng bởi Time.timeScale = 0
    //    videoPlayer.Play();
    //}

    public void RestartGame()
    {
        isGameOver = false;
        isGameWin = false;
        isPaused = false;
        score = 0;
        UpdateScore();
        Time.timeScale = 1;
        FindObjectOfType<Interstitial>().ShowInterstitialAd(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        

    }
    


    public void NextLevel()
    {
        isGameWin = false;
        score = 0;
        Time.timeScale = 1;

        int nextLevel = PlayerPrefs.GetInt("LevelToLoad", 1);

        if (nextLevel == 4) // Nếu hoàn thành Level 3, chuyển sang CutScene1
        {
            FindObjectOfType<Interstitial>().ShowInterstitialAd(() =>
            {
                SceneManager.LoadScene("CutScene1");
            });
            
        }
        else
        {
            FindObjectOfType<Interstitial>().ShowInterstitialAd(() =>
            {
                string levelName = "Level " + nextLevel;
                SceneManager.LoadScene("LoadingScene");
            });
            
            //SceneManager.LoadScene(levelName);
        }
    }


    public void MainMenu()
    {
        FindObjectOfType<Interstitial>().ShowInterstitialAd(() =>
        {
            SceneManager.LoadScene("Menu");
            Time.timeScale = 1;
        });
        
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pausedGameUi.SetActive(true);
        optionMenuUi.SetActive(false);
        pausedGameUi.transform.Find("PausedCenter").gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausedGameUi.SetActive(false);
        //itemMenuUi.SetActive(false);
    }

    public void ShowOptionMenu()
    {
        optionMenuUi.SetActive(!optionMenuUi.activeSelf);
        pausedGameUi.transform.Find("PausedCenter").gameObject.SetActive(!optionMenuUi.activeSelf);
        Time.timeScale = optionMenuUi.activeSelf ? 0 : 1;
    }

    public void ReturnToPausedUI()
    {
        optionMenuUi.SetActive(false);
        pausedGameUi.transform.Find("PausedCenter").gameObject.SetActive(true);
    }

    public bool IsGameOver() => isGameOver;
    public bool IsGameWin() => isGameWin;

    public void StartTimer()
    {
        timerRunning = true;
    }

    public bool IsTimerRunning()
    {
        return timerRunning;
    }

}