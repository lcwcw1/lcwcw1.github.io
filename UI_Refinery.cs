using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Refinery : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _closeButton = null;    // �ݱ� ��ư
    [SerializeField] private Button _refineButton = null;    // ���� ��ư
    [SerializeField] public TextMeshProUGUI _refineIngredient = null;

    private static UI_Refinery _instance = null; public static UI_Refinery instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _status = false; // �ǹ� ���� UI Ȱ��ȭ ���� 

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);    // �ǹ� ���� �ݱ� ��ư Ŭ�� �� Close �޼��� ȣ��
        _refineButton.onClick.AddListener(refine);    // ���� ��ư Ŭ�� �� refine �޼��� ȣ��
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    private void refine()
    {
        if (UI_Main.instance._essence < Building.selectedInstance.CurrentLevel.Amount1)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }
        UI_Main.instance._essence -= Building.selectedInstance.CurrentLevel.Amount1;
        UI_Main.instance._refinedEssence += 1;
        _refineIngredient.text = UI_Main.instance._essence + "/" + Building.selectedInstance.CurrentLevel.Amount1;
        UI_Main.instance._essenceText.text = UI_Main.instance._essence.ToString();
        UI_Main.instance._refinedEssenceText.text = UI_Main.instance._refinedEssence.ToString();
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
        SetStatus(false); // �ǹ� ���� UI ��Ȱ��ȭ
        Building.selectedInstance = null;
    }

}
