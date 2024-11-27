using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Building : MonoBehaviour // 상점 UI 안 건물 버튼들에 연결
{
    [SerializeField] private int _prefabIndex = 0; // 사용될 건물 프리팹의 인덱스
    [SerializeField] public Button _button = null; // 버튼 객체
    [SerializeField] public TextMeshProUGUI _limitsText = null;
    [SerializeField] public bool isGold = false;
    [SerializeField] public bool isEssence = false;
    [SerializeField] public bool isRefinedEssence = false;
    [SerializeField] public int limits = 1;
    [SerializeField] public int currency = 0;
    [SerializeField] public int amount = 0;
    private static UI_Building _instance = null; public static UI_Building instance { get { return _instance; } set { _instance = value; } } // 싱글턴 인스턴스

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
    }

    // Start 메서드는 게임 시작 시 호출됨
    private void Start()
    {
        LoadAmount();

        _button.onClick.AddListener(Clicked); // 버튼 클릭 시 Clicked 메서드 호출
    }

    // 버튼 클릭 시 호출되는 메서드
    private void Clicked()
    {
        _instance = this; // 싱글톤 인스턴스 초기화

        if (isGold)
        {
            if (UI_Main.instance._gold < currency)
            {
                UI_Caution.instance.SetRStatus(true);
                return;
            }
        }
        if (isEssence)
        {
            if (UI_Main.instance._essence < currency)
            {
                UI_Caution.instance.SetRStatus(true);
                return;
            }
        }
        if (isRefinedEssence)
        {
            if (UI_Main.instance._refinedEssence < currency)
            {
                UI_Caution.instance.SetRStatus(true);
                return;
            }
        }

        UI_Shop.instance.SetStatus(false); // 상점 UI 비활성화
        UI_Main.instance.SetStatus(true); // 메인 UI 활성화

        Vector3 position = Vector3.zero; // 초기 위치 설정

        // 프리팹 인덱스에 해당하는 프리팹을 복사하여 생성
        Building building = Instantiate(UI_Main.instance._buildingPrefabs[_prefabIndex], position, Quaternion.identity);

        building.PlacedOnGrid(20, 20); // 그리드에 건물 배치

        Building.buildInstance = building; // 현재 건물 인스턴스 설정
        CameraController.instance.isPlacingBuilding = true; // 건물 배치 상태 설정

        UI_Build.instance.SetStatus(true); // 빌딩 UI 활성화
    }

    public void SaveAmount()
    {
        PlayerPrefs.SetInt($"BuildingAmount_{_prefabIndex}", amount);
        PlayerPrefs.Save(); // 데이터 저장
    }

    public void LoadAmount()
    {
        amount = PlayerPrefs.GetInt($"BuildingAmount_{_prefabIndex}", 0); // 기본값 0
        _limitsText.text = amount + " / " + limits; // UI 업데이트
        if (limits == amount)
        {
            _button.interactable = false;
        }
    }
}
