using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour // ���� UI�� ����
{
    [SerializeField] public GameObject _elements = null; // ���� UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _closeButton = null; // �ݱ� ��ư
    [SerializeField] public TextMeshProUGUI _goldText = null;
    [SerializeField] public TextMeshProUGUI _essenceText = null;
    [SerializeField] public TextMeshProUGUI _gemText = null;
    [SerializeField] public TextMeshProUGUI _refinedEssenceText = null;

    [SerializeField] public GameObject _buildingElements = null;
    [SerializeField] public GameObject _currencyElements = null;
    [SerializeField] private Button _buildingButton = null;    // �ǹ� ��ư
    [SerializeField] private Button _currencyButton = null;    // ��ȭ ��ư

    private static UI_Shop _instance = null; public static UI_Shop instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _buildingStatus = true;
    public bool _currencyStatus = false;
    Color _baseTabColor = new Color32(234, 234, 226, 255);
    Color _offTabColor = new Color32(143, 138, 124, 255);

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // ���� UI ��� ��Ȱ��ȭ
    }

    private void Start()
    {
        _goldText.text = UI_Main.instance._gold.ToString();
        _essenceText.text = UI_Main.instance._essence.ToString();
        _gemText.text = UI_Main.instance._gem.ToString();
        _refinedEssenceText.text = UI_Main.instance._refinedEssence.ToString();

        _closeButton.onClick.AddListener(CloseShop); // �ݱ� ��ư Ŭ�� ������ �߰�
        _buildingButton.onClick.AddListener(BuildingTab);
        _currencyButton.onClick.AddListener(CurrencyTab);
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status); // ���� UI ��� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
    }

    // �ݱ� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void CloseShop()
    {
        SetStatus(false); // ���� UI ��Ȱ��ȭ
        UI_Main.instance.SetStatus(true); // ���� UI Ȱ��ȭ
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
