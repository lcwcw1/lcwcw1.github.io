using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Caution : MonoBehaviour
{
    [SerializeField] public GameObject _rElements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] public GameObject _lElements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _rConfirmButton = null;    // Ȯ�� ��ư
    [SerializeField] private Button _lConfirmButton = null;    // Ȯ�� ��ư
    private static UI_Caution _instance = null; public static UI_Caution instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _rstatus = false; // �ڿ� ���� ��� UI Ȱ��ȭ ���� 
    public bool _lstatus = false; // ���� ��� UI Ȱ��ȭ ���� 

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _rElements.SetActive(false); // UI ��Ȱ��ȭ
        _lElements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _rConfirmButton.onClick.AddListener(Close);    // ��� Ȯ�� ��ư Ŭ�� �� Close �޼��� ȣ��
        _lConfirmButton.onClick.AddListener(Close);    // ��� Ȯ�� ��ư Ŭ�� �� Close �޼��� ȣ��
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetRStatus(bool status)
    {
        _rElements.SetActive(status);
    }
    public void SetLStatus(bool status)
    {
        _lElements.SetActive(status);
    }

    // ���� UI �ݱ� �޼���
    private void Close()
    {
        _rstatus = false;    // �ڿ� ���� ��� UI ���� ��Ȱ��ȭ
        _lstatus = false;    // ���� ��� UI ���� ��Ȱ��ȭ
        UI_Main.instance.isActive = true;   // ���� UI Ȱ��ȭ
        SetRStatus(false); // ��� UI ��Ȱ��ȭ
        SetLStatus(false); // ��� UI ��Ȱ��ȭ
    }
}
