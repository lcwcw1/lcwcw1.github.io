using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BuildingInfo : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _closeButton = null;    // �ݱ� ��ư
    [SerializeField] public TextMeshProUGUI _buildingNameText = null;
    [SerializeField] public TextMeshProUGUI _buildingLevelText = null;
    [SerializeField] public Image _iconImage = null; // �������� ǥ���� Image ������Ʈ
    [SerializeField] public GameObject _gmElements = null;  // �ݱ��� �ǹ�
    [SerializeField] public TextMeshProUGUI _gmCurrentAmount = null;  // �ݱ��� ���� ��� ȹ�淮
    [SerializeField] public GameObject _ecElements = null;  // ��ȭ�� �ǹ�
    [SerializeField] public GameObject _bsElements = null;  // ���尣 �ǹ�
    [SerializeField] public GameObject _erElements = null;  // ������ �ǹ�
    [SerializeField] public TextMeshProUGUI _erCurrentAmount = null;  // �ݱ��� ���� ���� ���� �ʿ䷮
    [SerializeField] public GameObject _stElements = null;  // ����� �ǹ�
    [SerializeField] public TextMeshProUGUI _stgCurrentAmount = null;  // ����� ���� ��� �ʿ䷮
    [SerializeField] public TextMeshProUGUI _steCurrentAmount = null;  // ����� ���� ������ ���� �ʿ䷮

    private static UI_BuildingInfo _instance = null; public static UI_BuildingInfo instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _status = false; // �ǹ� ���� UI Ȱ��ȭ ���� 

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);    // �ǹ� ���� �ݱ� ��ư Ŭ�� �� Close �޼��� ȣ��
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

        _status = false;    // �ǹ� ���� UI ���� ��Ȱ��ȭ
        UI_Main.instance.isActive = true;   // ���� UI Ȱ��ȭ
        _gmElements.SetActive(false);
        _ecElements.SetActive(false);
        _bsElements.SetActive(false);
        _erElements.SetActive(false);
        _stElements.SetActive(false);
        SetStatus(false); // �ǹ� ���� UI ��Ȱ��ȭ
        Building.selectedInstance = null;
    }

    public void SetBuildingInfo(Building building)
    {
        _buildingNameText.text = building._buildingName; // �ǹ� �̸� ����
        _buildingLevelText.text = "Lv " + building.CurrentLevel.level; // �ǹ� ���� ����
        _iconImage.sprite = building.CurrentLevel.icon; // �ǹ� �̹��� ����

        if (building._buildingName == "�ݱ���")
        {
            _gmCurrentAmount.text = Building.selectedInstance.CurrentLevel.Amount1.ToString();
            _gmElements.SetActive(true);
        }
        else if (building._buildingName == "��ȭ��")
        {
            _ecElements.SetActive(true);
        }
        else if (building._buildingName == "���尣")
        {
            _bsElements.SetActive(true);
        }
        else if (building._buildingName == "������")
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

        SetStatus(true); // UI Ȱ��ȭ
    }

}
