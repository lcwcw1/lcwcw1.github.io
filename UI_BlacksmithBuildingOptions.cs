using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BlacksmithBuildingOptions : MonoBehaviour // 대장간 건물의 옵션 UI에 연결
{
    [SerializeField] public GameObject _elements = null;    // UI 요소를 포함하는 게임 오브젝트
    [SerializeField] private Button _weaponButton = null;    // 장비 버튼
    [SerializeField] private Button _upgradeButton = null;  // 건물 업그레이드 버튼
    [SerializeField] private Button _infoButton = null;     // 건물 정보 버튼
    [SerializeField] public Button _replaceButton = null;   // 건물 재배치 버튼
    [SerializeField] public TextMeshProUGUI _buildingText = null;
    private static UI_BlacksmithBuildingOptions _instance = null; public static UI_BlacksmithBuildingOptions instance { get { return _instance; } } // 싱글턴 인스턴스

    public bool _isReplacing = false;   // 재배치 여부

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
        _elements.SetActive(false); // UI 비활성화
    }

    private void Start()
    {
        _weaponButton.onClick.AddListener(weapon);    // 장비 강화 버튼 클릭 시 reinforce 메서드 호출
        _upgradeButton.onClick.AddListener(upgrade);    // 건물 업그레이드 버튼 클릭 시 upgrade 메서드 호출
        _infoButton.onClick.AddListener(info);  // 건물 정보 버튼 클릭 시 info 메서드 호출
        _replaceButton.onClick.AddListener(replace);    // 건물 재배치 버튼 클릭 시 replace 메서드 호출
    }

    // UI 활성화 상태를 설정하는 메서드
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // 장비 버튼 클릭 시 호출되는 메서드
    private void weapon()
    {
        UI_Build.instance.Cancel(); // 현재 건물 배치 취소
        UI_Equipment.instance.SetStatus(true); // 장비 UI 활성화

        if (UI_Equipment.instance.weapon1_status)
        {
            UI_Equipment.instance._swordElements1.SetActive(true);
            UI_Equipment.instance._weaponIconImage.sprite = UI_Equipment.instance._weaponIcon1;
        }
        else if (UI_Equipment.instance.weapon2_status)
        {
            UI_Equipment.instance._swordElements2.SetActive(true);
            UI_Equipment.instance._weaponIconImage.sprite = UI_Equipment.instance._weaponIcon2;
        }
        else if (UI_Equipment.instance.weapon3_status)
        {
            UI_Equipment.instance._swordElements3.SetActive(true);
            UI_Equipment.instance._weaponIconImage.sprite = UI_Equipment.instance._weaponIcon3;
        }
        else if (UI_Equipment.instance.weapon4_status)
        {
            UI_Equipment.instance._swordElements4.SetActive(true);
            UI_Equipment.instance._weaponIconImage.sprite = UI_Equipment.instance._weaponIcon4;
        }
        else
        {
            UI_Equipment.instance._swordElements5.SetActive(true);
            UI_Equipment.instance._weaponIconImage.sprite = UI_Equipment.instance._weaponIcon5;
        }

        // 메인 UI가 켜져 있어 화면이 움직이고 버튼이 눌리기에 이를 방지하기 위한 코드
        UI_Main.instance.isActive = false;
        if (UI_PlayerInfo.instance._status)
        {
            UI_Main.instance._shopButton.interactable = false;
            UI_Main.instance._mapButton.interactable = false;
            UI_Main.instance._playerInfoButton.interactable = false;
            UI_Main.instance._menuButton.interactable = false;
        }

        // 모든 건물의 옵션 UI 비활성화
        SetStatus(false);
        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);
    }


    // 건물 업그레이드 버튼 클릭 시 호출되는 메서드
    private void upgrade()
    {
        UI_Build.instance.Cancel(); // 현재 건물 배치 취소
        UI_BuildingLvUp.instance.SetStatus(true); // 건물 레벨업 UI 활성화

        // 메인 UI가 켜져 있어 화면이 움직이고 버튼이 눌리기에 이를 방지하기 위한 코드
        UI_Main.instance.isActive = false;
        if (UI_PlayerInfo.instance._status)
        {
            UI_Main.instance._shopButton.interactable = false;
            UI_Main.instance._mapButton.interactable = false;
            UI_Main.instance._playerInfoButton.interactable = false;
            UI_Main.instance._menuButton.interactable = false;
        }

        // 현재 선택된 건물 정보 UI에 전달
        if (Building.selectedInstance != null)
        {
            UI_BuildingLvUp.instance.SetBuildingInfo(Building.selectedInstance);
        }

        // 모든 건물의 옵션 UI 비활성화
        SetStatus(false);
        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);
    }

    // 건물 정보 버튼 클릭 시 호출되는 메서드
    private void info()
    {
        UI_Build.instance.Cancel(); // 현재 건물 배치 취소
        UI_BuildingInfo.instance.SetStatus(true); // 건물 정보 UI 활성화

        // 메인 UI가 켜져 있어 화면이 움직이고 버튼이 눌리기에 이를 방지하기 위한 코드
        UI_Main.instance.isActive = false;
        if (UI_PlayerInfo.instance._status)
        {
            UI_Main.instance._shopButton.interactable = false;
            UI_Main.instance._mapButton.interactable = false;
            UI_Main.instance._playerInfoButton.interactable = false;
            UI_Main.instance._menuButton.interactable = false;
        }

        // 현재 선택된 건물 정보 UI에 전달
        if (Building.selectedInstance != null)
        {
            UI_BuildingInfo.instance.SetBuildingInfo(Building.selectedInstance);
        }

        // 모든 건물의 옵션 UI 비활성화
        SetStatus(false);
        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);
    }

    // 건물 재배치 버튼 클릭 시 호출되는 메서드
    private void replace()
    {
        _isReplacing = !_isReplacing;   // 재배치 여부 토글
        if (!_isReplacing)
        {
            Building.selectedInstance._baseArea.gameObject.SetActive(false);    // 재배치 종료 시 재배치 가능 여부 판단 바닥 영역 비활성화
        }
        else
        {
            Building.selectedInstance._baseArea.gameObject.SetActive(true);     // 재배치 시작 시 재배치 가능 여부 판단 바닥 영역 활성화
        }

        // 추후 개선사항 : 재배치 버튼을 누를 시 UI가 꺼지면서 처음 건물을 건설했을 때처럼 확인 버튼이 나오게 하기

    }
}
