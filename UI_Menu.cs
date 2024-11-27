using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Menu : MonoBehaviour // �޴� UI�� ����
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _closeButton = null;    // �ݱ� ��ư
    [SerializeField] private Button _musicButton = null;   // ���� ��ư
    [SerializeField] private Button _seButton = null;   // ȿ���� ��ư
    [SerializeField] public Image musicIcon = null;
    [SerializeField] public Sprite musicOnIcon = null;
    [SerializeField] public Sprite musicOffIcon = null;
    [SerializeField] public Image soundIcon = null;
    [SerializeField] public Sprite soundOnIcon = null;
    [SerializeField] public Sprite soundOffIcon = null;
    [SerializeField] public GameObject musicObject1 = null;  // ���� ������Ʈ 1
    [SerializeField] public GameObject musicObject2 = null;  // ���� ������Ʈ 2
    [SerializeField] public GameObject soundObject = null;  // ȿ���� ������Ʈ
    [SerializeField] private Button _saveButton = null;     // ���� ��ư
    [SerializeField] private Button _titleButton = null;     // Ÿ��Ʋ ��ư

    private AudioSource musicSource1;
    private AudioSource musicSource2;
    private AudioSource soundSource;

    private static UI_Menu _instance = null; public static UI_Menu instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _status = false; // �÷��̾� ���� UI Ȱ��ȭ ���� 
    private bool isMusicOn = true;
    private bool isSoundOn = true;

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ

        // AudioSource ������Ʈ ��������
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
        _closeButton.onClick.AddListener(Close);    // �޴� �ݱ� ��ư Ŭ�� �� Close �޼��� ȣ��
        _musicButton.onClick.AddListener(music);    // ���� ��ư Ŭ�� �� music �޼��� ȣ��
        _seButton.onClick.AddListener(se);    // ȿ���� ��ư Ŭ�� �� se �޼��� ȣ��
        _saveButton.onClick.AddListener(save);    // ���� ��ư Ŭ�� �� save �޼��� ȣ��
        _titleButton.onClick.AddListener(title);    // Ÿ��Ʋ ��ư Ŭ�� �� title �޼��� ȣ��
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // ���� �޼���
    private void music()
    {
        isMusicOn = !isMusicOn;
        UpdateMusicIcon();

        // ���� mute ���� ���
        if (musicSource1 != null && musicSource2 != null)
        {
            musicSource1.mute = !isMusicOn;
            musicSource2.mute = !isMusicOn;
        }
    }

    // ȿ���� �޼���
    private void se()
    {
        isSoundOn = !isSoundOn;
        UpdateSoundIcon();

        // ȿ���� mute ���� ���
        if (soundSource != null)
        {
            soundSource.mute = !isSoundOn;
        }
    }

    // ���� ���� �޼���
    private void save()
    {
        GameManager.instance.SaveGameData();
    }

    // Ÿ��Ʋ�� ���� �޼���
    private void title()
    {
        GameManager.instance.SaveGameData();
        SceneManager.LoadScene("Main_title");
    }

    // �޴� UI �ݱ� �޼���
    private void Close()
    {
        // ���� UI�� �ִ� ��� ��ư Ȱ��ȭ
        UI_Main.instance._shopButton.interactable = true;
        UI_Main.instance._mapButton.interactable = true;
        UI_Main.instance._playerInfoButton.interactable = true;
        UI_Main.instance._menuButton.interactable = true;

        _status = false;    // �÷��̾� ���� UI ���� ��Ȱ��ȭ
        UI_Main.instance.isActive = true;   // ���� UI Ȱ��ȭ
        SetStatus(false); // �÷��̾� ���� UI ��Ȱ��ȭ
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
