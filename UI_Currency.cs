using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Currency : MonoBehaviour
{
    [SerializeField] public Button _button = null; // 버튼 객체
    [SerializeField] public int currency = 0;
    [SerializeField] public int getCurrency = 0;
    [SerializeField] public bool isGem = false;
    [SerializeField] public bool isMoney = false;
    [SerializeField] public bool isGold = false;
    [SerializeField] public bool isEssence = false;

    // Start 메서드는 게임 시작 시 호출됨
    private void Start()
    {
        _button.onClick.AddListener(Clicked); // 버튼 클릭 시 Clicked 메서드 호출
    }

    // 버튼 클릭 시 호출되는 메서드
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
            // 돈이 없다면 자원 부족 경고 나오게 하기

            UI_Main.instance._gem += getCurrency;
            UI_Main.instance._gemText.text = UI_Main.instance._gem.ToString();
            UI_Shop.instance._gemText.text = UI_Main.instance._gem.ToString();
        }

    }
}
