using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BlacksmithBuildingOptions : MonoBehaviour // ���尣 �ǹ��� �ɼ� UI�� ����
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _weaponButton = null;    // ��� ��ư
    [SerializeField] private Button _upgradeButton = null;  // �ǹ� ���׷��̵� ��ư
    [SerializeField] private Button _infoButton = null;     // �ǹ� ���� ��ư
    [SerializeField] public Button _replaceButton = null;   // �ǹ� ���ġ ��ư
    [SerializeField] public TextMeshProUGUI _buildingText = null;
    private static UI_BlacksmithBuildingOptions _instance = null; public static UI_BlacksmithBuildingOptions instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _isReplacing = false;   // ���ġ ����

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _weaponButton.onClick.AddListener(weapon);    // ��� ��ȭ ��ư Ŭ�� �� reinforce �޼��� ȣ��
        _upgradeButton.onClick.AddListener(upgrade);    // �ǹ� ���׷��̵� ��ư Ŭ�� �� upgrade �޼��� ȣ��
        _infoButton.onClick.AddListener(info);  // �ǹ� ���� ��ư Ŭ�� �� info �޼��� ȣ��
        _replaceButton.onClick.AddListener(replace);    // �ǹ� ���ġ ��ư Ŭ�� �� replace �޼��� ȣ��
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // ��� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void weapon()
    {
        UI_Build.instance.Cancel(); // ���� �ǹ� ��ġ ���
        UI_Equipment.instance.SetStatus(true); // ��� UI Ȱ��ȭ

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
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
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
        UI_RefineryBuildingOptions.instance.SetStatus(false);
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
