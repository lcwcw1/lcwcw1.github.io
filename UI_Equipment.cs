using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Equipment : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI 요소를 포함하는 게임 오브젝트
    [SerializeField] private Button _closeButton = null;    // 닫기 버튼
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
    public Sprite _weaponIcon1 = null; // 아이콘
    public Sprite _weaponIcon2 = null; // 아이콘
    public Sprite _weaponIcon3 = null; // 아이콘
    public Sprite _weaponIcon4 = null; // 아이콘
    public Sprite _weaponIcon5 = null; // 아이콘

    private static UI_Equipment _instance = null; public static UI_Equipment instance { get { return _instance; } } // 싱글턴 인스턴스

    public bool _status = false;

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
        _elements.SetActive(false); // UI 비활성화
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close); 
    }

    // UI 활성화 상태를 설정하는 메서드
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // 건물 정보 UI 닫기 메서드
    private void Close()
    {
        // 메인 UI에 있는 모든 버튼 활성화
        UI_Main.instance._shopButton.interactable = true;
        UI_Main.instance._mapButton.interactable = true;
        UI_Main.instance._playerInfoButton.interactable = true;
        UI_Main.instance._menuButton.interactable = true;

        _status = false;
        UI_Main.instance.isActive = true;   // 메인 UI 활성화
        SetStatus(false);
        Building.selectedInstance = null;

        _swordElements1.SetActive(false);
        _swordElements2.SetActive(false);
        _swordElements3.SetActive(false);
        _swordElements4.SetActive(false);
        _swordElements5.SetActive(false);
    }

}
