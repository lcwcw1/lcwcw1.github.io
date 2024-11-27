using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BuildingInfo : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI 요소를 포함하는 게임 오브젝트
    [SerializeField] private Button _closeButton = null;    // 닫기 버튼
    [SerializeField] public TextMeshProUGUI _buildingNameText = null;
    [SerializeField] public TextMeshProUGUI _buildingLevelText = null;
    [SerializeField] public Image _iconImage = null; // 아이콘을 표시할 Image 컴포넌트
    [SerializeField] public GameObject _gmElements = null;  // 금광산 건물
    [SerializeField] public TextMeshProUGUI _gmCurrentAmount = null;  // 금광산 현재 골드 획득량
    [SerializeField] public GameObject _ecElements = null;  // 강화소 건물
    [SerializeField] public GameObject _bsElements = null;  // 대장간 건물
    [SerializeField] public GameObject _erElements = null;  // 정제소 건물
    [SerializeField] public TextMeshProUGUI _erCurrentAmount = null;  // 금광산 현재 현재 정수 필요량
    [SerializeField] public GameObject _stElements = null;  // 저장소 건물
    [SerializeField] public TextMeshProUGUI _stgCurrentAmount = null;  // 저장소 현재 골드 필요량
    [SerializeField] public TextMeshProUGUI _steCurrentAmount = null;  // 저장소 현재 정제된 정수 필요량

    private static UI_BuildingInfo _instance = null; public static UI_BuildingInfo instance { get { return _instance; } } // 싱글턴 인스턴스

    public bool _status = false; // 건물 정보 UI 활성화 여부 

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
        _elements.SetActive(false); // UI 비활성화
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);    // 건물 정보 닫기 버튼 클릭 시 Close 메서드 호출
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

        _status = false;    // 건물 정보 UI 상태 비활성화
        UI_Main.instance.isActive = true;   // 메인 UI 활성화
        _gmElements.SetActive(false);
        _ecElements.SetActive(false);
        _bsElements.SetActive(false);
        _erElements.SetActive(false);
        _stElements.SetActive(false);
        SetStatus(false); // 건물 정보 UI 비활성화
        Building.selectedInstance = null;
    }

    public void SetBuildingInfo(Building building)
    {
        _buildingNameText.text = building._buildingName; // 건물 이름 설정
        _buildingLevelText.text = "Lv " + building.CurrentLevel.level; // 건물 레벨 설정
        _iconImage.sprite = building.CurrentLevel.icon; // 건물 이미지 설정

        if (building._buildingName == "금광산")
        {
            _gmCurrentAmount.text = Building.selectedInstance.CurrentLevel.Amount1.ToString();
            _gmElements.SetActive(true);
        }
        else if (building._buildingName == "강화소")
        {
            _ecElements.SetActive(true);
        }
        else if (building._buildingName == "대장간")
        {
            _bsElements.SetActive(true);
        }
        else if (building._buildingName == "정제소")
        {
            _erCurrentAmount.text = Building.selectedInstance.CurrentLevel.Amount1.ToString();
            _erElements.SetActive(true);
        }
        else
        {
            _stgCurrentAmount.text = Building.selectedInstance.CurrentLevel.Amount1.ToString();
            _steCurrentAmount.text = Building.selectedInstance.CurrentLevel.Amount2.ToString();
            _stElements.SetActive(true);
        }

        SetStatus(true); // UI 활성화
    }

}
