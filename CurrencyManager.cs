using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public int currency; // 현재 재화 수치
    public Text currencyText; // GUI에 표시할 재화 텍스트

    private void Start()
    {
        currency = PlayerPrefs.GetInt("Currency", 0); // 재화 수치를 PlayerPrefs에서 불러오기
        UpdateCurrencyUI();
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        PlayerPrefs.SetInt("Currency", currency); // 재화 수치를 PlayerPrefs에 저장
        UpdateCurrencyUI();
    }

    void UpdateCurrencyUI()
    {
        if (currencyText != null)
    {
        currencyText.text = currency.ToString(); // UI에 재화 표시
    }
    }
    public int GetCurrency()
    {
        return currency;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // 씬 변경에도 유지
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }
    
}
