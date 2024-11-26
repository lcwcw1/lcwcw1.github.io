using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel; // 설정 패널
    public SoundManager soundManager;
    public Slider musicSlider;
    public Button resetButton;
    
    void Start()
    {
        // 슬라이더 초기화
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);;

        // 이벤트 등록
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        resetButton.onClick.AddListener(ResetSettings);
    }

    public void SetMusicVolume(float volume)
    {
        soundManager.SetVolume(musicSlider.value);
    }

    public void ResetSettings()
    {
        soundManager.ResetSettings();
        musicSlider.value = 0.5f;
    }
    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf); // 패널 토글
    }
}