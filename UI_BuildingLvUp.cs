using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildingLvUp : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] public Button _gmLvUpButton = null;    // �ݱ��� ��ȭ ��ư
    [SerializeField] public Button _erLvUpButton = null;    // ������ ��ȭ ��ư
    [SerializeField] public Button _stLvUpButton = null;    // ����� �ڿ� ���� ��ư
    [SerializeField] private Button _closeButton = null;    // �ݱ� ��ư
    [SerializeField] public TextMeshProUGUI _buildingNameText = null;
    [SerializeField] public TextMeshProUGUI _buildingLevelText = null;
    [SerializeField] public TextMeshProUGUI _buildingNextLevelText = null;
    [SerializeField] public Image _iconImage = null; // �������� ǥ���� Image ������Ʈ

    [SerializeField] public GameObject _gmElements = null;  // �ݱ��� �ǹ�
    [SerializeField] public TextMeshProUGUI _gmCurrentAmount = null;  // �ݱ��� ���� ��� ȹ�淮
    [SerializeField] public TextMeshProUGUI _gmNextAmount = null;  // �ݱ��� ���� ��� ȹ�淮
    [SerializeField] public TextMeshProUGUI _gmIngredient1 = null;  // �ݱ��� ��ȭ ���1
    [SerializeField] public TextMeshProUGUI _gmIngredient2 = null;  // �ݱ��� ��ȭ ���2

    [SerializeField] public GameObject _erElements = null;  // ������ �ǹ�
    [SerializeField] public TextMeshProUGUI _erCurrentAmount = null;  // ������ ���� ���� �ʿ䷮
    [SerializeField] public TextMeshProUGUI _erNextAmount = null;  // ������ ���� ���� �ʿ䷮
    [SerializeField] public TextMeshProUGUI _erIngredient1 = null;  // ������ ��ȭ ���1
    [SerializeField] public TextMeshProUGUI _erIngredient2 = null;  // ������ ��ȭ ���2

    [SerializeField] public GameObject _stElements = null;  // ����� �ǹ�
    [SerializeField] public TextMeshProUGUI _stgCurrentAmount = null;  // ����� ���� ��� �ʿ䷮
    [SerializeField] public TextMeshProUGUI _stgNextAmount = null;  // ����� ���� ��� �ʿ䷮
    [SerializeField] public TextMeshProUGUI _steCurrentAmount = null;  // ����� ���� ������ ���� �ʿ䷮
    [SerializeField] public TextMeshProUGUI _steNextAmount = null;  // ����� ���� ������ ���� �ʿ䷮
    [SerializeField] public TextMeshProUGUI _stIngredient1 = null;  // ����� ��ȭ ���1
    [SerializeField] public TextMeshProUGUI _stIngredient2 = null;  // ����� ��ȭ ���2
    private static UI_BuildingLvUp _instance = null; public static UI_BuildingLvUp instance { get { return _instance; } set { _instance = value; } } // �̱��� �ν��Ͻ�

    public bool _status = false; // �ǹ� ������ UI Ȱ��ȭ ���� 

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _gmLvUpButton.onClick.AddListener(gmLevelUp);    // �ݱ��� ��ȭ ��ư Ŭ�� �� gmLevelUp �޼��� ȣ��
        _erLvUpButton.onClick.AddListener(erLevelUp);    // ������ ��ȭ ��ư Ŭ�� �� erLevelUp �޼��� ȣ��
        _stLvUpButton.onClick.AddListener(stLevelUp);    // ����� �ڿ� ���� ��ư Ŭ�� �� stLevelUp �޼��� ȣ��
        _closeButton.onClick.AddListener(Close);    // �ǹ� ������ UI �ݱ� ��ư Ŭ�� �� Close �޼��� ȣ��
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // �ǹ� ������ �޼���
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


    // �ǹ� ������ UI �ݱ� �޼���
    private void Close()
    {
        // ���� UI�� �ִ� ��� ��ư Ȱ��ȭ
        UI_Main.instance._shopButton.interactable = true;
        UI_Main.instance._mapButton.interactable = true;
        UI_Main.instance._playerInfoButton.interactable = true;
        UI_Main.instance._menuButton.interactable = true;

        _status = false;    // �ǹ� ������ UI ���� ��Ȱ��ȭ
        UI_Main.instance.isActive = true;   // ���� UI Ȱ��ȭ
        _gmElements.SetActive(false);
        _erElements.SetActive(false);
        _stElements.SetActive(false);
        SetStatus(false); // �ǹ� ������ UI ��Ȱ��ȭ
        Building.selectedInstance = null;
    }

    public void SetBuildingInfo(Building building)
    {
        _buildingNameText.text = building._buildingName; // �ǹ� �̸� ����
        _buildingLevelText.text = "Lv " + building.CurrentLevel.level; // �ǹ� ���� ����
        _buildingNextLevelText.text = "Lv " + (building.CurrentLevel.level + 1);
        _iconImage.sprite = building.CurrentLevel.icon; // �ǹ� �̹��� ����
        building.currentIndex = 0;

        if (building._buildingName == "�ݱ���")
        {
            _gmCurrentAmount.text = building.CurrentLevel.Amount1.ToString();
            _gmNextAmount.text = building.CurrentLevel.NextAmount1.ToString();
            _gmIngredient1.text = UI_Main.instance._gold + " / " + building.CurrentLevel.ingredient1;
            _gmIngredient2.text = UI_Main.instance._essence + " / " + building.CurrentLevel.ingredient2;
            _gmLvUpButton.interactable = true;
            _gmElements.SetActive(true);
        }
        else if (building._buildingName == "������")
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

        SetStatus(true); // UI Ȱ��ȭ
    }
}