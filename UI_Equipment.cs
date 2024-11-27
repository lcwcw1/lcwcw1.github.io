using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Equipment : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _closeButton = null;    // �ݱ� ��ư
    [SerializeField] public bool weapon1_status;
    [SerializeField] public bool weapon2_status;
    [SerializeField] public bool weapon3_status;
    [SerializeField] public bool weapon4_status;
    [SerializeField] public bool weapon5_status;
    [SerializeField] public GameObject _swordElements1 = null;
    [SerializeField] public GameObject _swordElements2 = null;
    [SerializeField] public GameObject _swordElements3 = null;
    [SerializeField] public GameObject _swordElements4 = null;
    [SerializeField] public GameObject _swordElements5 = null;
    [SerializeField] public Image _weaponIconImage = null;
    public Sprite _weaponIcon1 = null; // ������
    public Sprite _weaponIcon2 = null; // ������
    public Sprite _weaponIcon3 = null; // ������
    public Sprite _weaponIcon4 = null; // ������
    public Sprite _weaponIcon5 = null; // ������

    private static UI_Equipment _instance = null; public static UI_Equipment instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _status = false;

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close); 
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // �ǹ� ���� UI �ݱ� �޼���
    private void Close()
    {
        // ���� UI�� �ִ� ��� ��ư Ȱ��ȭ
        UI_Main.instance._shopButton.interactable = true;
        UI_Main.instance._mapButton.interactable = true;
        UI_Main.instance._playerInfoButton.interactable = true;
        UI_Main.instance._menuButton.interactable = true;

        _status = false;
        UI_Main.instance.isActive = true;   // ���� UI Ȱ��ȭ
        SetStatus(false);
        Building.selectedInstance = null;

        _swordElements1.SetActive(false);
        _swordElements2.SetActive(false);
        _swordElements3.SetActive(false);
        _swordElements4.SetActive(false);
        _swordElements5.SetActive(false);
    }

}
