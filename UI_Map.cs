using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Map : MonoBehaviour // ���� UI�� ����
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _closeButton = null;    // �ݱ� ��ư
    private static UI_Map _instance = null; public static UI_Map instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _status = false; // ���� UI Ȱ��ȭ ���� 

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);    // ���� �ݱ� ��ư Ŭ�� �� Close �޼��� ȣ��
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // ���� UI �ݱ� �޼���
    private void Close()
    {
        // ���� UI�� �ִ� ��� ��ư Ȱ��ȭ
        UI_Main.instance._shopButton.interactable = true;
        UI_Main.instance._mapButton.interactable = true;
        UI_Main.instance._playerInfoButton.interactable = true;
        UI_Main.instance._menuButton.interactable = true;

        _status = false;    // ���� UI ���� ��Ȱ��ȭ
        UI_Main.instance.isActive = true;   // ���� UI Ȱ��ȭ
        SetStatus(false); // ���� UI ��Ȱ��ȭ
    }
}
