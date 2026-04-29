using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip backGroundClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip gunClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip healHpClip;
    [SerializeField] private AudioClip bossBackgroundClip;
    [SerializeField] private AudioClip checkPointClip;
    [SerializeField] private AudioClip bossattack1Clip;
    [SerializeField] private AudioClip bossattack2Clip;
    [SerializeField] private AudioClip bossattack3Clip;


    [SerializeField] private Slider backgroundVolumeSlider;
    [SerializeField] private Slider effectVolumeSlider;

    private const string BackgroundVolumeKey = "BackgroundVolume";
    private const string EffectVolumeKey = "EffectVolume";
    private const float DefaultVolume = 0.05f;

    void Start()
    {
        // Lấy âm lượng đã lưu, nếu chưa có thì dùng giá trị mặc định
        float savedBackgroundVolume = PlayerPrefs.GetFloat(BackgroundVolumeKey, DefaultVolume);
        float savedEffectVolume = PlayerPrefs.GetFloat(EffectVolumeKey, DefaultVolume);

        backgroundAudioSource.volume = savedBackgroundVolume;
        effectAudioSource.volume = savedEffectVolume;

        PlayBackGroundMusic();

        // Gán giá trị ban đầu của slider theo âm lượng đã lưu
        backgroundVolumeSlider.value = savedBackgroundVolume;
        effectVolumeSlider.value = savedEffectVolume;

        // Đăng ký sự kiện khi slider thay đổi
        backgroundVolumeSlider.onValueChanged.AddListener(SetBackgroundVolume);
        effectVolumeSlider.onValueChanged.AddListener(SetEffectVolume);
    }

    public void PlayBackGroundMusic()
    {
        backgroundAudioSource.clip = backGroundClip;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }
    public void PlayBossMusic()
    {
        backgroundAudioSource.clip = bossBackgroundClip;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    public void RestoreBackgroundMusic()
    {
        backgroundAudioSource.clip = backGroundClip;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    public void PlayCoinSound() => effectAudioSource.PlayOneShot(coinClip);
    public void PlayGunSound() => effectAudioSource.PlayOneShot(gunClip);
    public void PlayDamageSound() => effectAudioSource.PlayOneShot(damageClip);
    public void PlayJumpSound() => effectAudioSource.PlayOneShot(jumpClip);
    public void PlayGameOverSound() => effectAudioSource.PlayOneShot(gameOverClip);
    public void PlayhealHpSound() => effectAudioSource.PlayOneShot(healHpClip);
    public void PlayCheckPointSound() => effectAudioSource.PlayOneShot(checkPointClip);
    public void PlayBossAttack1Sound() => effectAudioSource.PlayOneShot(bossattack1Clip);
    public void PlayBossAttack2Sound() => effectAudioSource.PlayOneShot(bossattack2Clip);
    public void PlayBossAttack3Sound() => effectAudioSource.PlayOneShot(bossattack3Clip);

    public void PlayWinSound()
    {
        effectAudioSource.volume = 1.0f;  
        effectAudioSource.PlayOneShot(winClip, 1.0f);
    }


    public void StopBackgroundMusic()
    {
        if (backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }
    }

    public void StopEffectSounds()
    {
        effectAudioSource.Stop();
    }

    // Cập nhật và lưu âm lượng nhạc nền
    public void SetBackgroundVolume(float volume)
    {
        backgroundAudioSource.volume = volume;
        PlayerPrefs.SetFloat(BackgroundVolumeKey, volume);
        PlayerPrefs.Save();
    }

    // Cập nhật và lưu âm lượng hiệu ứng âm thanh
    public void SetEffectVolume(float volume)
    {
        effectAudioSource.volume = volume;
        PlayerPrefs.SetFloat(EffectVolumeKey, volume);
        PlayerPrefs.Save();
    }


}
