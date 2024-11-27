using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_EnhancementBuildingOptions : MonoBehaviour // �÷��̾� ��ȭ �ǹ��� �ɼ� UI�� ����
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _enhancementButton = null;  // �÷��̾� ��ȭ ��ư
    [SerializeField] private Button _upgradeButton = null;  // �ǹ� ���׷��̵� ��ư
    [SerializeField] private Button _infoButton = null;     // �ǹ� ���� ��ư
    [SerializeField] public Button _replaceButton = null;   // �ǹ� ���ġ ��ư
    [SerializeField] public TextMeshProUGUI _buildingText = null;
    private static UI_EnhancementBuildingOptions _instance = null; public static UI_EnhancementBuildingOptions instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _isReplacing = false;   // ���ġ ����

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _enhancementButton.onClick.AddListener(enhancement);    // �÷��̾� ��ȭ ��ư Ŭ�� �� enhancement �޼��� ȣ��
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
    private void enhancement()
    {
        UI_Build.instance.Cancel(); // ���� �ǹ� ��ġ ���
        UI_Enhancement.instance.SetStatus(true); // ��ȭ UI Ȱ��ȭ
        UI_Enhancement.instance._statElements.SetActive(true);
        UI_Enhancement.instance._skillElements.SetActive(false);
        UI_Enhancement.instance._statButton.image.color = UI_Enhancement.instance._baseTabColor;
        UI_Enhancement.instance._skillButton.image.color = UI_Enhancement.instance._offTabColor;
        UI_Enhancement.instance._statStatus = true;
        UI_Enhancement.instance._skillStatus = false;

        UI_Enhancement.instance._currentLevelText.text = "Level" + UI_Main.instance._playerLv.ToString();
        UI_Enhancement.instance._nextLevelText.text = "Level" + (UI_Main.instance._playerLv + 1).ToString();
        UI_Enhancement.instance._LvUpIngredient1.text = UI_Main.instance._gold + " / " + UI_Enhancement.instance.LvUpIngredient1;
        UI_Enhancement.instance._LvUpIngredient2.text = UI_Main.instance._essence + " / " + UI_Enhancement.instance.LvUpIngredient2;

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
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
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
        UI_RefineryBuildingOptions.instance.SetStatus(false);
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
        UI_RefineryBuildingOptions.instance.SetStatus(false);
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
