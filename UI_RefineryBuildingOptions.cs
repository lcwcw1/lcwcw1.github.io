using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RefineryBuildingOptions : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _refineButton = null;  // �÷��̾� ��ȭ ��ư
    [SerializeField] private Button _upgradeButton = null;  // �ǹ� ���׷��̵� ��ư
    [SerializeField] private Button _infoButton = null;     // �ǹ� ���� ��ư
    [SerializeField] public Button _replaceButton = null;   // �ǹ� ���ġ ��ư
    [SerializeField] public TextMeshProUGUI _buildingText = null;
    private static UI_RefineryBuildingOptions _instance = null; public static UI_RefineryBuildingOptions instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _isReplacing = false;   // ���ġ ����

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _refineButton.onClick.AddListener(refine);    // �÷��̾� ��ȭ ��ư Ŭ�� �� refine �޼��� ȣ��
        _upgradeButton.onClick.AddListener(upgrade);    // �ǹ� ���׷��̵� ��ư Ŭ�� �� upgrade �޼��� ȣ��
        _infoButton.onClick.AddListener(info);  // �ǹ� ���� ��ư Ŭ�� �� info �޼��� ȣ��
        _replaceButton.onClick.AddListener(replace);    // �ǹ� ���ġ ��ư Ŭ�� �� replace �޼��� ȣ��
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // �÷��̾� ��ȭ ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void refine()
    {
        UI_Build.instance.Cancel(); // ���� �ǹ� ��ġ ���
        UI_Refinery.instance.SetStatus(true); // �ǹ� ���� UI Ȱ��ȭ
        UI_Refinery.instance._refineIngredient.text = UI_Main.instance._essence + "/" + Building.selectedInstance.CurrentLevel.Amount1;

        // ���� UI�� ���� �־� ȭ���� �����̰� ��ư�� �����⿡ �̸� �����ϱ� ���� �ڵ�
        UI_Main.instance.isActive = false;
        if (UI_PlayerInfo.instance._status)
        {
            UI_Main.instance._shopButton.interactable = false;
            UI_Main.instance._mapButton.interactable = false;
            UI_Main.instance._playerInfoButton.interactable = false;
            UI_Main.instance._menuButton.interactable = false;
        }

        // ��� �ǹ��� �ɼ� UI ��Ȱ��ȭ
        SetStatus(false);
        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);
    }

    // �ǹ� ���׷��̵� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void upgrade()
    {
        UI_Build.instance.Cancel(); // ���� �ǹ� ��ġ ���
        UI_BuildingLvUp.instance.SetStatus(true); // �ǹ� ������ UI Ȱ��ȭ

        // ���� UI�� ���� �־� ȭ���� �����̰� ��ư�� �����⿡ �̸� �����ϱ� ���� �ڵ�
        UI_Main.instance.isActive = false;
        if (UI_PlayerInfo.instance._status)
        {
            UI_Main.instance._shopButton.interactable = false;
            UI_Main.instance._mapButton.interactable = false;
            UI_Main.instance._playerInfoButton.interactable = false;
            UI_Main.instance._menuButton.interactable = false;
        }
        
        // ���� ���õ� �ǹ� ���� UI�� ����
        if (Building.selectedInstance != null)
        {
            UI_BuildingLvUp.instance.SetBuildingInfo(Building.selectedInstance);
        }

        // ��� �ǹ��� �ɼ� UI ��Ȱ��ȭ
        SetStatus(false);
        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);
    }

    // �ǹ� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void info()
    {
        UI_Build.instance.Cancel(); // ���� �ǹ� ��ġ ���
        UI_BuildingInfo.instance.SetStatus(true); // �ǹ� ���� UI Ȱ��ȭ

        // ���� UI�� ���� �־� ȭ���� �����̰� ��ư�� �����⿡ �̸� �����ϱ� ���� �ڵ�
        UI_Main.instance.isActive = false;
        if (UI_PlayerInfo.instance._status)
        {
            UI_Main.instance._shopButton.interactable = false;
            UI_Main.instance._mapButton.interactable = false;
            UI_Main.instance._playerInfoButton.interactable = false;
            UI_Main.instance._menuButton.interactable = false;
        }

        // ���� ���õ� �ǹ� ���� UI�� ����
        if (Building.selectedInstance != null)
        {
            UI_BuildingInfo.instance.SetBuildingInfo(Building.selectedInstance);
        }

        // ��� �ǹ��� �ɼ� UI ��Ȱ��ȭ
        SetStatus(false);
        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);
    }

    // �ǹ� ���ġ ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void replace()
    {
        _isReplacing = !_isReplacing;   // ���ġ ���� ���
        if (!_isReplacing)
        {
            Building.selectedInstance._baseArea.gameObject.SetActive(false);    // ���ġ ���� �� ���ġ ���� ���� �Ǵ� �ٴ� ���� ��Ȱ��ȭ
        }
        else
        {
            Building.selectedInstance._baseArea.gameObject.SetActive(true);     // ���ġ ���� �� ���ġ ���� ���� �Ǵ� �ٴ� ���� Ȱ��ȭ
        }

        // ���� �������� : ���ġ ��ư�� ���� �� UI�� �����鼭 ó�� �ǹ��� �Ǽ����� ��ó�� Ȯ�� ��ư�� ������ �ϱ�

    }
}
