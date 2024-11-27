using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Skill : MonoBehaviour
{
    [SerializeField] public GameObject _skill1_Elements = null;
    [SerializeField] public GameObject _skill2_Elements = null;
    [SerializeField] private Button _skill1_Button = null;    // 스킬 버튼
    [SerializeField] private Button _skill2_Button = null;

    [SerializeField] public float _skill1_Power = 500f;
    [SerializeField] public int skill1_Level = 0;
    [SerializeField] private Button _skill1_UpgradeButton = null;
    [SerializeField] public TextMeshProUGUI _skill1_PowerText = null;
    [SerializeField] public TextMeshProUGUI _skill1_LevelText = null;
    [SerializeField] public TextMeshProUGUI _skill1_LvUpIngredient1 = null;
    [SerializeField] public TextMeshProUGUI _skill1_LvUpIngredient2 = null;
    [SerializeField] public int skill1_LvUpIngredient1 = 5000;
    [SerializeField] public int skill1_LvUpIngredient2 = 5;

    [SerializeField] public float _skill2_Power = 1000f;
    [SerializeField] public int skill2_Level = 0;
    [SerializeField] private Button _skill2_UpgradeButton = null;
    [SerializeField] public TextMeshProUGUI _skill2_PowerText = null;
    [SerializeField] public TextMeshProUGUI _skill2_LevelText = null;
    [SerializeField] public TextMeshProUGUI _skill2_LvUpIngredient1 = null;
    [SerializeField] public TextMeshProUGUI _skill2_LvUpIngredient2 = null;
    [SerializeField] public int skill2_LvUpIngredient1 = 10000;
    [SerializeField] public int skill2_LvUpIngredient2 = 10;

    private static UI_Skill _instance = null; public static UI_Skill instance { get { return _instance; } } // 싱글턴 인스턴스

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
    }

    private void Start()
    {
        _skill1_Button.onClick.AddListener(Skill1);
        _skill1_UpgradeButton.onClick.AddListener(Skill1_Upgrade);
        _skill2_Button.onClick.AddListener(Skill2);
        _skill2_UpgradeButton.onClick.AddListener(Skill2_Upgrade);
    }

    private void Skill1()
    {
        _skill1_Elements.SetActive(true);
        _skill2_Elements.SetActive(false);

        _skill1_LevelText.text = "+" + skill1_Level;
        _skill1_PowerText.text = (_skill1_Power * 3).ToString();
        _skill1_LvUpIngredient1.text = UI_Main.instance._gold + "/" + skill1_LvUpIngredient1;
        _skill1_LvUpIngredient2.text = UI_Main.instance._refinedEssence + "/" + skill1_LvUpIngredient2;
    }

    private void Skill2()
    {
        _skill2_Elements.SetActive(true);
        _skill1_Elements.SetActive(false);

        _skill2_LevelText.text = "+" + skill2_Level;
        _skill2_PowerText.text = (_skill2_Power * 3).ToString();
        _skill2_LvUpIngredient1.text = UI_Main.instance._gold + "/" + skill2_LvUpIngredient1;
        _skill2_LvUpIngredient2.text = UI_Main.instance._refinedEssence + "/" + skill2_LvUpIngredient2;
    }

    private void Skill1_Upgrade()
    {
        if (UI_Main.instance._gold < skill1_LvUpIngredient1 || UI_Main.instance._refinedEssence < skill1_LvUpIngredient2)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }
        UI_Main.instance._gold -= skill1_LvUpIngredient1;
        UI_Main.instance._refinedEssence -= skill1_LvUpIngredient2;
        skill1_Level += 1;
        _skill1_Power += 250f;
        _skill1_LevelText.text = "+" + skill1_Level;
        _skill1_PowerText.text = (_skill1_Power * 3).ToString();
        skill1_LvUpIngredient1 = (skill1_LvUpIngredient1 * 11) / 10;
        skill1_LvUpIngredient2 = (skill1_LvUpIngredient2 * 11) / 10;
        _skill1_LvUpIngredient1.text = UI_Main.instance._gold + "/" + skill1_LvUpIngredient1;
        _skill1_LvUpIngredient2.text = UI_Main.instance._refinedEssence + "/" + skill1_LvUpIngredient2;
        UI_Main.instance._goldText.text = UI_Main.instance._gold.ToString();
        UI_Main.instance._refinedEssenceText.text = UI_Main.instance._refinedEssence.ToString();
    }

    private void Skill2_Upgrade()
    {
        if (UI_Main.instance._gold < skill2_LvUpIngredient1 || UI_Main.instance._refinedEssence < skill2_LvUpIngredient2)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }
        UI_Main.instance._gold -= skill2_LvUpIngredient1;
        UI_Main.instance._refinedEssence -= skill2_LvUpIngredient2;
        skill2_Level += 1;
        _skill2_Power += 500f;
        _skill2_LevelText.text = "+" + skill2_Level;
        _skill2_PowerText.text = (_skill2_Power * 3).ToString();
        skill2_LvUpIngredient1 = (skill2_LvUpIngredient1 * 12) / 10;
        skill2_LvUpIngredient2 = (skill2_LvUpIngredient2 * 12) / 10;
        _skill2_LvUpIngredient1.text = UI_Main.instance._gold + "/" + skill2_LvUpIngredient1;
        _skill2_LvUpIngredient2.text = UI_Main.instance._refinedEssence + "/" + skill2_LvUpIngredient2;
        UI_Main.instance._goldText.text = UI_Main.instance._gold.ToString();
        UI_Main.instance._refinedEssenceText.text = UI_Main.instance._refinedEssence.ToString();
    }

    private void OnApplicationQuit()
    {
        // 스킬1 정보 저장
        PlayerPrefs.SetFloat("Skill1_Power", _skill1_Power);
        PlayerPrefs.SetInt("Skill1_Lv", skill1_Level);
        PlayerPrefs.SetInt("Skill1_LvUpIngredient1", skill1_LvUpIngredient1);
        PlayerPrefs.SetInt("Skill1_LvUpIngredient2", skill1_LvUpIngredient2);

        // 스킬2 정보 저장
        PlayerPrefs.SetFloat("Skill2_Power", _skill2_Power);
        PlayerPrefs.SetInt("Skill2_Lv", skill2_Level);
        PlayerPrefs.SetInt("Skill2_LvUpIngredient1", skill2_LvUpIngredient1);
        PlayerPrefs.SetInt("Skill2_LvUpIngredient2", skill2_LvUpIngredient2);

        PlayerPrefs.Save();
    }
}
