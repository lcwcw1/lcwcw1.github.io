using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Currency : MonoBehaviour
{
    [SerializeField] public Button _button = null; // ��ư ��ü
    [SerializeField] public int currency = 0;
    [SerializeField] public int getCurrency = 0;
    [SerializeField] public bool isGem = false;
    [SerializeField] public bool isMoney = false;
    [SerializeField] public bool isGold = false;
    [SerializeField] public bool isEssence = false;

    // Start �޼���� ���� ���� �� ȣ���
    private void Start()
    {
        _button.onClick.AddListener(Clicked); // ��ư Ŭ�� �� Clicked �޼��� ȣ��
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void Clicked()
    {
        if (isGem)
        {
            if (UI_Main.instance._gem < currency)
            {
                UI_Caution.instance.SetRStatus(true);
                return;
            }

            UI_Main.instance._gem -= currency;
            UI_Main.instance._gemText.text = UI_Main.instance._gem.ToString();
            UI_Shop.instance._gemText.text = UI_Main.instance._gem.ToString();

            if (isGold)
            {
                UI_Main.instance._gold += getCurrency;
                UI_Main.instance._goldText.text = UI_Main.instance._gold.ToString();
                UI_Shop.instance._goldText.text = UI_Main.instance._gold.ToString();
            }
            if (isEssence)
            {
                UI_Main.instance._essence += getCurrency;
                UI_Main.instance._essenceText.text = UI_Main.instance._essence.ToString();
                UI_Shop.instance._essenceText.text = UI_Main.instance._essence.ToString();
            }
        }
        if (isMoney)
        {
            // ���� ���ٸ� �ڿ� ���� ��� ������ �ϱ�

            UI_Main.instance._gem += getCurrency;
            UI_Main.instance._gemText.text = UI_Main.instance._gem.ToString();
            UI_Shop.instance._gemText.text = UI_Main.instance._gem.ToString();
        }

    }
}
