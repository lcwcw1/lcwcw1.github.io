using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour // 상점 UI에 연결
{
    [SerializeField] public GameObject _elements = null; // 상점 UI 요소를 포함하는 게임 오브젝트
    [SerializeField] private Button _closeButton = null; // 닫기 버튼
    [SerializeField] public TextMeshProUGUI _goldText = null;
    [SerializeField] public TextMeshProUGUI _essenceText = null;
    [SerializeField] public TextMeshProUGUI _gemText = null;
    [SerializeField] public TextMeshProUGUI _refinedEssenceText = null;

    [SerializeField] public GameObject _buildingElements = null;
    [SerializeField] public GameObject _currencyElements = null;
    [SerializeField] private Button _buildingButton = null;    // 건물 버튼
    [SerializeField] private Button _currencyButton = null;    // 재화 버튼

    private static UI_Shop _instance = null; public static UI_Shop instance { get { return _instance; } } // 싱글턴 인스턴스

    public bool _buildingStatus = true;
    public bool _currencyStatus = false;
    Color _baseTabColor = new Color32(234, 234, 226, 255);
    Color _offTabColor = new Color32(143, 138, 124, 255);

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
        _elements.SetActive(false); // 상점 UI 요소 비활성화
    }

    private void Start()
    {
        _goldText.text = UI_Main.instance._gold.ToString();
        _essenceText.text = UI_Main.instance._essence.ToString();
        _gemText.text = UI_Main.instance._gem.ToString();
        _refinedEssenceText.text = UI_Main.instance._refinedEssence.ToString();

        _closeButton.onClick.AddListener(CloseShop); // 닫기 버튼 클릭 리스너 추가
        _buildingButton.onClick.AddListener(BuildingTab);
        _currencyButton.onClick.AddListener(CurrencyTab);
    }

    // UI 활성화 상태를 설정하는 메서드
    public void SetStatus(bool status)
    {
        _elements.SetActive(status); // 상점 UI 요소 활성화 또는 비활성화
    }

    // 닫기 버튼 클릭 시 호출되는 메서드
    private void CloseShop()
    {
        SetStatus(false); // 상점 UI 비활성화
        UI_Main.instance.SetStatus(true); // 메인 UI 활성화
    }
    private void BuildingTab()
    {
        if (_buildingStatus == true)
        {
            return;
        }
        _buildingElements.SetActive(true);
        _currencyElements.SetActive(false);
        _buildingButton.image.color = _baseTabColor;
        _currencyButton.image.color = _offTabColor;
        _buildingStatus = true;
        _currencyStatus = false;
    }
    private void CurrencyTab()
    {
        if (_currencyStatus == true)
        {
            return;
        }
        _currencyElements.SetActive(true);
        _buildingElements.SetActive(false);
        _currencyButton.image.color = _baseTabColor;
        _buildingButton.image.color = _offTabColor;
        _currencyStatus = true;
        _buildingStatus = false;
    }
}
