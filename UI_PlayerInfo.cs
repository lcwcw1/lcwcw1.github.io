using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerInfo : MonoBehaviour // �÷��̾� ���� UI�� ����
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _closeButton = null;    // �ݱ� ��ư
    [SerializeField] public TextMeshProUGUI _playerNameText = null;
    [SerializeField] public TextMeshProUGUI _playerLevelText = null;
    [SerializeField] public TextMeshProUGUI _AtkPowText = null;
    [SerializeField] public TextMeshProUGUI _AtkSpdText = null;
    [SerializeField] public TextMeshProUGUI _GetEssenceText = null;

    private static UI_PlayerInfo _instance = null; public static UI_PlayerInfo instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _status = false; // �÷��̾� ���� UI Ȱ��ȭ ���� 

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);    // �÷��̾� ���� �ݱ� ��ư Ŭ�� �� Close �޼��� ȣ��
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // �÷��̾� ���� UI �ݱ� �޼���
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
}
