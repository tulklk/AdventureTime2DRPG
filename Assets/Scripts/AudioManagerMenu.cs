using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagerMenu : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioClip backGroundClip;
    [SerializeField] private Slider backgroundVolumeSlider;

    private const string BackgroundVolumeKey = "BackgroundVolume"; // Key để lưu âm lượng
    private const float DefaultVolume = 0.05f;

    void Start()
    {
        // Lấy âm lượng đã lưu, nếu chưa có thì dùng giá trị mặc định
        float savedVolume = PlayerPrefs.GetFloat(BackgroundVolumeKey, DefaultVolume);
        backgroundAudioSource.volume = savedVolume;

        PlayBackGroundMusic();

        // Gán giá trị ban đầu của slider theo âm lượng AudioSource
        backgroundVolumeSlider.value = savedVolume;

        // Đăng ký sự kiện khi slider thay đổi
        backgroundVolumeSlider.onValueChanged.AddListener(SetBackgroundVolume);
    }

    public void PlayBackGroundMusic()
    {
        backgroundAudioSource.clip = backGroundClip;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        if (backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }
    }

    // Hàm để cập nhật và lưu âm lượng nhạc nền
    public void SetBackgroundVolume(float volume)
    {
        backgroundAudioSource.volume = volume;
        PlayerPrefs.SetFloat(BackgroundVolumeKey, volume); // Lưu lại âm lượng
        PlayerPrefs.Save(); // Lưu thay đổi vào bộ nhớ
    }
}