using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildingLvUp : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI 요소를 포함하는 게임 오브젝트
    [SerializeField] public Button _gmLvUpButton = null;    // 금광산 강화 버튼
    [SerializeField] public Button _erLvUpButton = null;    // 정제소 강화 버튼
    [SerializeField] public Button _stLvUpButton = null;    // 저장소 자원 전송 버튼
    [SerializeField] private Button _closeButton = null;    // 닫기 버튼
    [SerializeField] public TextMeshProUGUI _buildingNameText = null;
    [SerializeField] public TextMeshProUGUI _buildingLevelText = null;
    [SerializeField] public TextMeshProUGUI _buildingNextLevelText = null;
    [SerializeField] public Image _iconImage = null; // 아이콘을 표시할 Image 컴포넌트

    [SerializeField] public GameObject _gmElements = null;  // 금광산 건물
    [SerializeField] public TextMeshProUGUI _gmCurrentAmount = null;  // 금광산 현재 골드 획득량
    [SerializeField] public TextMeshProUGUI _gmNextAmount = null;  // 금광산 다음 골드 획득량
    [SerializeField] public TextMeshProUGUI _gmIngredient1 = null;  // 금광산 강화 재료1
    [SerializeField] public TextMeshProUGUI _gmIngredient2 = null;  // 금광산 강화 재료2

    [SerializeField] public GameObject _erElements = null;  // 정제소 건물
    [SerializeField] public TextMeshProUGUI _erCurrentAmount = null;  // 정제소 현재 정수 필요량
    [SerializeField] public TextMeshProUGUI _erNextAmount = null;  // 정제소 다음 정수 필요량
    [SerializeField] public TextMeshProUGUI _erIngredient1 = null;  // 정제소 강화 재료1
    [SerializeField] public TextMeshProUGUI _erIngredient2 = null;  // 정제소 강화 재료2

    [SerializeField] public GameObject _stElements = null;  // 저장소 건물
    [SerializeField] public TextMeshProUGUI _stgCurrentAmount = null;  // 저장소 현재 골드 필요량
    [SerializeField] public TextMeshProUGUI _stgNextAmount = null;  // 저장소 다음 골드 필요량
    [SerializeField] public TextMeshProUGUI _steCurrentAmount = null;  // 저장소 현재 정제된 정수 필요량
    [SerializeField] public TextMeshProUGUI _steNextAmount = null;  // 저장소 다음 정제된 정수 필요량
    [SerializeField] public TextMeshProUGUI _stIngredient1 = null;  // 저장소 강화 재료1
    [SerializeField] public TextMeshProUGUI _stIngredient2 = null;  // 저장소 강화 재료2
    private static UI_BuildingLvUp _instance = null; public static UI_BuildingLvUp instance { get { return _instance; } set { _instance = value; } } // 싱글턴 인스턴스

    public bool _status = false; // 건물 레벨업 UI 활성화 여부 

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
        _elements.SetActive(false); // UI 비활성화
    }

    private void Start()
    {
        _gmLvUpButton.onClick.AddListener(gmLevelUp);    // 금광산 강화 버튼 클릭 시 gmLevelUp 메서드 호출
        _erLvUpButton.onClick.AddListener(erLevelUp);    // 정제소 강화 버튼 클릭 시 erLevelUp 메서드 호출
        _stLvUpButton.onClick.AddListener(stLevelUp);    // 저장소 자원 전송 버튼 클릭 시 stLevelUp 메서드 호출
        _closeButton.onClick.AddListener(Close);    // 건물 레벨업 UI 닫기 버튼 클릭 시 Close 메서드 호출
    }

    // UI 활성화 상태를 설정하는 메서드
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // 건물 레벨업 메서드
    private void gmLevelUp()
    {
        if (UI_Main.instance._gold < Building.selectedInstance.CurrentLevel.ingredient1 || UI_Main.instance._essence < Building.selectedInstance.CurrentLevel.ingredient2)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }
        else
        {
            UI_Main.instance._gold -= Building.selectedInstance.CurrentLevel.ingredient1;
            UI_Main.instance._essence -= Building.selectedInstance.CurrentLevel.ingredient2;
            UI_Main.instance._goldText.text = UI_Main.instance._gold.ToString();
            UI_Main.instance._essenceText.text = UI_Main.instance._essence.ToString();
        }
        Building.selectedInstance.LevelUp();
    }
    private void erLevelUp()
    {
        if (UI_Main.instance._gold < Building.selectedInstance.CurrentLevel.ingredient1 || UI_Main.instance._essence < Building.selectedInstance.CurrentLevel.ingredient2)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }
        else
        {
            UI_Main.instance._gold -= Building.selectedInstance.CurrentLevel.ingredient1;
            UI_Main.instance._essence -= Building.selectedInstance.CurrentLevel.ingredient2;
            UI_Main.instance._goldText.text = UI_Main.instance._gold.ToString();
            UI_Main.instance._essenceText.text = UI_Main.instance._essence.ToString();
        }
        Building.selectedInstance.LevelUp();
    }
    private void stLevelUp()
    {
        if (UI_Main.instance._gold < Building.selectedInstance.CurrentLevel.ingredient1 || UI_Main.instance._refinedEssence < Building.selectedInstance.CurrentLevel.ingredient2)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }
        else
        {
            UI_Main.instance._gold -= Building.selectedInstance.CurrentLevel.ingredient1;
            UI_Main.instance._refinedEssence -= Building.selectedInstance.CurrentLevel.ingredient2;
            UI_Main.instance._goldText.text = UI_Main.instance._gold.ToString();
            UI_Main.instance._refinedEssenceText.text = UI_Main.instance._refinedEssence.ToString();
        }
        Building.selectedInstance.LevelUp();
    }


    // 건물 레벨업 UI 닫기 메서드
    private void Close()
    {
        // 메인 UI에 있는 모든 버튼 활성화
        UI_Main.instance._shopButton.interactable = true;
        UI_Main.instance._mapButton.interactable = true;
        UI_Main.instance._playerInfoButton.interactable = true;
        UI_Main.instance._menuButton.interactable = true;

        _status = false;    // 건물 레벨업 UI 상태 비활성화
        UI_Main.instance.isActive = true;   // 메인 UI 활성화
        _gmElements.SetActive(false);
        _erElements.SetActive(false);
        _stElements.SetActive(false);
        SetStatus(false); // 건물 레벨업 UI 비활성화
        Building.selectedInstance = null;
    }

    public void SetBuildingInfo(Building building)
    {
        _buildingNameText.text = building._buildingName; // 건물 이름 설정
        _buildingLevelText.text = "Lv " + building.CurrentLevel.level; // 건물 레벨 설정
        _buildingNextLevelText.text = "Lv " + (building.CurrentLevel.level + 1);
        _iconImage.sprite = building.CurrentLevel.icon; // 건물 이미지 설정
        building.currentIndex = 0;

        if (building._buildingName == "금광산")
        {
            _gmCurrentAmount.text = building.CurrentLevel.Amount1.ToString();
            _gmNextAmount.text = building.CurrentLevel.NextAmount1.ToString();
            _gmIngredient1.text = UI_Main.instance._gold + " / " + building.CurrentLevel.ingredient1;
            _gmIngredient2.text = UI_Main.instance._essence + " / " + building.CurrentLevel.ingredient2;
            _gmLvUpButton.interactable = true;
            _gmElements.SetActive(true);
        }
        else if (building._buildingName == "정제소")
        {
            _erCurrentAmount.text = building.CurrentLevel.Amount1.ToString();
            _erNextAmount.text = building.CurrentLevel.NextAmount1.ToString();
            _erIngredient1.text = UI_Main.instance._gold + " / " + building.CurrentLevel.ingredient1;
            _erIngredient2.text = UI_Main.instance._essence + " / " + building.CurrentLevel.ingredient2;
            _erLvUpButton.interactable = true;
            _erElements.SetActive(true);
        }
        else
        {
            _stgCurrentAmount.text = building.CurrentLevel.Amount1.ToString();
            _stgNextAmount.text = building.CurrentLevel.NextAmount1.ToString();
            _steCurrentAmount.text = building.CurrentLevel.Amount2.ToString();
            _steNextAmount.text = building.CurrentLevel.NextAmount2.ToString();
            _stIngredient1.text = UI_Main.instance._gold + " / " + building.CurrentLevel.ingredient1;
            _stIngredient2.text = UI_Main.instance._refinedEssence + " / " + building.CurrentLevel.ingredient2;
            _stLvUpButton.interactable = true;
            _stElements.SetActive(true);
        }

        SetStatus(true); // UI 활성화
    }
}