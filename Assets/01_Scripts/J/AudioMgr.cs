using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                }
            }

            return instance;
        }
    }

    public AudioClip intro_select_Music;
    public AudioClip townMusic;
    public AudioClip chaosMusic;
    public AudioClip raidMusic;
    public AudioClip btnSound;
    private AudioSource audioSource;

    [Header("PlayerSkillSound")]
    public AudioClip aP_qSound;
    public AudioClip aP_eSound;
    public AudioClip aP_rSound;
    public AudioClip aP_normalAtkSound;

    public AudioClip sP_qSound;
    public AudioClip sP_eSound;
    public AudioClip sP_rSound;
    public AudioClip sP_normalAtkSound;

    public AudioClip mP_qSound;
    public AudioClip mP_eSound;
    public AudioClip mP_rSound;
    public AudioClip mP_normalAtkSound;

    // 페이드 효과를 위한 변수들
    private float fadeDuration = 1.5f; // 페이드 시간
    private float targetVolume; // 목표 볼륨
    private bool isFading; // 페이드 중인지 여부

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += SceneLoadSound;

        audioSource.volume = 0.0f;
        targetVolume = 0.55f;

        // 페이드인 시작
        FadeIn();
        audioSource.clip = intro_select_Music;
        audioSource.Play();
    }

    private void Update()
    {
        if (isFading)
        {
            float deltaVolume = targetVolume / fadeDuration;
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, deltaVolume * Time.deltaTime);
            if (Mathf.Approximately(audioSource.volume, targetVolume))
            {
                isFading = false;
            }
        }
    }

    void SceneLoadSound(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            townMusicPlay();
        }
        else if (scene.name == "ChaosD")
        {
            ChaosMusicPlay();
        }
        else if (scene.name == "Raid")
        {
            RaidMusicPlay();
        }
        else if (scene.name == "ChSelect")
        {
            
        }
        else if (scene.name == "Intro")
        {
            intro_select_MusicPlay();
        }
        // 추가적인 씬에 따른 처리를 여기에 추가
    }
    public void BtnSound()
    {
        audioSource.PlayOneShot(btnSound);
    }

    public void SplayerQskillSound()
    {
        audioSource.PlayOneShot(sP_qSound);
    }
    public void SplayerEskillSound()
    {
        audioSource.PlayOneShot(sP_eSound);
    }
    public void SplayerRskillSound()
    {
        audioSource.PlayOneShot(sP_rSound);
    }
    public void SplayerAtklSound()
    {
        audioSource.PlayOneShot(sP_normalAtkSound);
    }

    public void AplayerQskillSound()
    {
        audioSource.PlayOneShot(aP_qSound);
    }
    public void AplayerEskillSound()
    {
        audioSource.PlayOneShot(aP_eSound);
    }
    public void AplayerRskillSound()
    {
        audioSource.PlayOneShot(aP_rSound);
    }
    public void AplayerAtkSound()
    {
        audioSource.PlayOneShot(aP_normalAtkSound);
    }

    public void MplayerQskillSound()
    {
        audioSource.PlayOneShot(mP_qSound);
    }
    public void MplayerEskillSound()
    {
        audioSource.PlayOneShot(mP_eSound);
    }
    public void MplayerRskillSound()
    {
        audioSource.PlayOneShot(mP_rSound);
    }
    public void MplayerAtkSound()
    {
        audioSource.PlayOneShot(mP_normalAtkSound);
    }


    public void intro_select_MusicPlay()
    {
        audioSource.clip = intro_select_Music;
        audioSource.Play();
        FadeIn();
    }

    public void townMusicPlay()
    {
        StopBackgroundMusic();
        FadeIn();
        audioSource.clip = townMusic;
        audioSource.Play();
    }
    public void ChaosMusicPlay()
    {
        StopBackgroundMusic();
        FadeIn();
        audioSource.clip = chaosMusic;
        audioSource.Play();
    }

    public void RaidMusicPlay()
    {
        StopBackgroundMusic();
        FadeIn();
        audioSource.clip = raidMusic;
        audioSource.Play();
    }
    public void StopBackgroundMusic()
    {
        // 페이드아웃 시작
        FadeOut();
    }

    // 페이드인 메서드
    private void FadeIn()
    {
        targetVolume = 0.55f;
        isFading = true;
    }

    // 페이드아웃 메서드
    private void FadeOut()
    {
        targetVolume = 0.0f;
        isFading = true;
    }
}