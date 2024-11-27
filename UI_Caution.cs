using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Caution : MonoBehaviour
{
    [SerializeField] public GameObject _rElements = null;    // UI 요소를 포함하는 게임 오브젝트
    [SerializeField] public GameObject _lElements = null;    // UI 요소를 포함하는 게임 오브젝트
    [SerializeField] private Button _rConfirmButton = null;    // 확인 버튼
    [SerializeField] private Button _lConfirmButton = null;    // 확인 버튼
    private static UI_Caution _instance = null; public static UI_Caution instance { get { return _instance; } } // 싱글턴 인스턴스

    public bool _rstatus = false; // 자원 부족 경고문 UI 활성화 여부 
    public bool _lstatus = false; // 레벨 경고문 UI 활성화 여부 

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
        _rElements.SetActive(false); // UI 비활성화
        _lElements.SetActive(false); // UI 비활성화
    }

    private void Start()
    {
        _rConfirmButton.onClick.AddListener(Close);    // 경고문 확인 버튼 클릭 시 Close 메서드 호출
        _lConfirmButton.onClick.AddListener(Close);    // 경고문 확인 버튼 클릭 시 Close 메서드 호출
    }

    // UI 활성화 상태를 설정하는 메서드
    public void SetRStatus(bool status)
    {
        _rElements.SetActive(status);
    }
    public void SetLStatus(bool status)
    {
        _lElements.SetActive(status);
    }

    // 지도 UI 닫기 메서드
    private void Close()
    {
        _rstatus = false;    // 자원 부족 경고문 UI 상태 비활성화
        _lstatus = false;    // 레벨 경고문 UI 상태 비활성화
        UI_Main.instance.isActive = true;   // 메인 UI 활성화
        SetRStatus(false); // 경고문 UI 비활성화
        SetLStatus(false); // 경고문 UI 비활성화
    }
}
