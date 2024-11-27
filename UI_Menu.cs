using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Menu : MonoBehaviour // 메뉴 UI에 연결
{
    [SerializeField] public GameObject _elements = null;    // UI 요소를 포함하는 게임 오브젝트
    [SerializeField] private Button _closeButton = null;    // 닫기 버튼
    [SerializeField] private Button _musicButton = null;   // 음악 버튼
    [SerializeField] private Button _seButton = null;   // 효과음 버튼
    [SerializeField] public Image musicIcon = null;
    [SerializeField] public Sprite musicOnIcon = null;
    [SerializeField] public Sprite musicOffIcon = null;
    [SerializeField] public Image soundIcon = null;
    [SerializeField] public Sprite soundOnIcon = null;
    [SerializeField] public Sprite soundOffIcon = null;
    [SerializeField] public GameObject musicObject1 = null;  // 음악 오브젝트 1
    [SerializeField] public GameObject musicObject2 = null;  // 음악 오브젝트 2
    [SerializeField] public GameObject soundObject = null;  // 효과음 오브젝트
    [SerializeField] private Button _saveButton = null;     // 저장 버튼
    [SerializeField] private Button _titleButton = null;     // 타이틀 버튼

    private AudioSource musicSource1;
    private AudioSource musicSource2;
    private AudioSource soundSource;

    private static UI_Menu _instance = null; public static UI_Menu instance { get { return _instance; } } // 싱글턴 인스턴스

    public bool _status = false; // 플레이어 정보 UI 활성화 여부 
    private bool isMusicOn = true;
    private bool isSoundOn = true;

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
        _elements.SetActive(false); // UI 비활성화

        // AudioSource 컴포넌트 가져오기
        if (musicObject1 != null)
        {
            musicSource1 = musicObject1.GetComponent<AudioSource>();
        }
        if (musicObject2 != null)
        {
            musicSource2 = musicObject2.GetComponent<AudioSource>();
        }
        if (soundObject != null)
        {
            soundSource = soundObject.GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);    // 메뉴 닫기 버튼 클릭 시 Close 메서드 호출
        _musicButton.onClick.AddListener(music);    // 음악 버튼 클릭 시 music 메서드 호출
        _seButton.onClick.AddListener(se);    // 효과음 버튼 클릭 시 se 메서드 호출
        _saveButton.onClick.AddListener(save);    // 저장 버튼 클릭 시 save 메서드 호출
        _titleButton.onClick.AddListener(title);    // 타이틀 버튼 클릭 시 title 메서드 호출
    }

    // UI 활성화 상태를 설정하는 메서드
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // 음악 메서드
    private void music()
    {
        isMusicOn = !isMusicOn;
        UpdateMusicIcon();

        // 음악 mute 상태 토글
        if (musicSource1 != null && musicSource2 != null)
        {
            musicSource1.mute = !isMusicOn;
            musicSource2.mute = !isMusicOn;
        }
    }

    // 효과음 메서드
    private void se()
    {
        isSoundOn = !isSoundOn;
        UpdateSoundIcon();

        // 효과음 mute 상태 토글
        if (soundSource != null)
        {
            soundSource.mute = !isSoundOn;
        }
    }

    // 게임 저장 메서드
    private void save()
    {
        GameManager.instance.SaveGameData();
    }

    // 타이틀로 가는 메서드
    private void title()
    {
        GameManager.instance.SaveGameData();
        SceneManager.LoadScene("Main_title");
    }

    // 메뉴 UI 닫기 메서드
    private void Close()
    {
        // 메인 UI에 있는 모든 버튼 활성화
        UI_Main.instance._shopButton.interactable = true;
        UI_Main.instance._mapButton.interactable = true;
        UI_Main.instance._playerInfoButton.interactable = true;
        UI_Main.instance._menuButton.interactable = true;

        _status = false;    // 플레이어 정보 UI 상태 비활성화
        UI_Main.instance.isActive = true;   // 메인 UI 활성화
        SetStatus(false); // 플레이어 정보 UI 비활성화
    }

    private void UpdateMusicIcon()
    {
        if (isMusicOn)
        {
            musicIcon.sprite = musicOnIcon;
        }
        else
        {
            musicIcon.sprite = musicOffIcon;
        }
    }

    private void UpdateSoundIcon()
    {
        if (isSoundOn)
        {
            soundIcon.sprite = soundOnIcon;
        }
        else
        {
            soundIcon.sprite = soundOffIcon;
        }
    }
}
