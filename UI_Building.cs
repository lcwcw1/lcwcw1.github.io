using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Building : MonoBehaviour // ���� UI �� �ǹ� ��ư�鿡 ����
{
    [SerializeField] private int _prefabIndex = 0; // ���� �ǹ� �������� �ε���
    [SerializeField] public Button _button = null; // ��ư ��ü
    [SerializeField] public TextMeshProUGUI _limitsText = null;
    [SerializeField] public bool isGold = false;
    [SerializeField] public bool isEssence = false;
    [SerializeField] public bool isRefinedEssence = false;
    [SerializeField] public int limits = 1;
    [SerializeField] public int currency = 0;
    [SerializeField] public int amount = 0;
    private static UI_Building _instance = null; public static UI_Building instance { get { return _instance; } set { _instance = value; } } // �̱��� �ν��Ͻ�

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
    }

    // Start �޼���� ���� ���� �� ȣ���
    private void Start()
    {
        LoadAmount();

        _button.onClick.AddListener(Clicked); // ��ư Ŭ�� �� Clicked �޼��� ȣ��
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void Clicked()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ

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

        UI_Shop.instance.SetStatus(false); // ���� UI ��Ȱ��ȭ
        UI_Main.instance.SetStatus(true); // ���� UI Ȱ��ȭ

        Vector3 position = Vector3.zero; // �ʱ� ��ġ ����

        // ������ �ε����� �ش��ϴ� �������� �����Ͽ� ����
        Building building = Instantiate(UI_Main.instance._buildingPrefabs[_prefabIndex], position, Quaternion.identity);

        building.PlacedOnGrid(20, 20); // �׸��忡 �ǹ� ��ġ

        Building.buildInstance = building; // ���� �ǹ� �ν��Ͻ� ����
        CameraController.instance.isPlacingBuilding = true; // �ǹ� ��ġ ���� ����

        UI_Build.instance.SetStatus(true); // ���� UI Ȱ��ȭ
    }

    public void SaveAmount()
    {
        PlayerPrefs.SetInt($"BuildingAmount_{_prefabIndex}", amount);
        PlayerPrefs.Save(); // ������ ����
    }

    public void LoadAmount()
    {
        amount = PlayerPrefs.GetInt($"BuildingAmount_{_prefabIndex}", 0); // �⺻�� 0
        _limitsText.text = amount + " / " + limits; // UI ������Ʈ
        if (limits == amount)
        {
            _button.interactable = false;
        }
    }
}
