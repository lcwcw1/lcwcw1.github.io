using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    
    private void Start()
    {
        // PlayerPrefs에서 볼륨 값 불러오기
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        
        SetVolume(musicVolume);
    }

    public void SetVolume(float musicVolume)
    {
        backgroundMusic.volume = musicVolume;
        
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void ResetSettings()
    {
        PlayerPrefs.DeleteKey("MusicVolume");
        PlayerPrefs.DeleteKey("EffectsVolume");
        SetVolume(0.5f); // 기본값으로 초기화
    }
}


/*using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource; // AudioSource 컴포넌트
    public AudioSource soundEffects;

    void Start()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        
        // AudioSource가 없으면 에러 메시지 출력
        if (audioSource == null)
        {
            Debug.LogError("AudioSource 컴포넌트가 없습니다. SoundManager에 추가해 주세요.");
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume; // 볼륨 조정
        }
    }
}
*/