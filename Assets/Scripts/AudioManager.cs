using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM Clips")]
    public AudioClip lobbyBGM;
    public AudioClip stage1BGM;
    public AudioClip stage2BGM;
    public AudioClip stage3BGM;

    [Header("SFX Clips")]
    public AudioClip buttonSFX;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        var sources = GetComponents<AudioSource>();
        bgmSource = sources[0];
        sfxSource = sources[1];
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // 편의 함수들
    public void PlayLobbyBGM() => PlayBGM(lobbyBGM);
    public void PlayStage1() => PlayBGM(stage1BGM);
    public void PlayStage2() => PlayBGM(stage2BGM);
    public void PlayStage3() => PlayBGM(stage3BGM);
    public void PlayButtonSound() => PlaySFX(buttonSFX);
}
