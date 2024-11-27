using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Enhancement : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] public GameObject _statElements = null;
    [SerializeField] public GameObject _skillElements = null;
    [SerializeField] private Button _closeButton = null;    // �ݱ� ��ư
    [SerializeField] public Button _statButton = null;    // ���� ��ư
    [SerializeField] private Button _LvUpButton = null;
    [SerializeField] public TextMeshProUGUI _currentLevelText = null;
    [SerializeField] public TextMeshProUGUI _nextLevelText = null;
    [SerializeField] public TextMeshProUGUI _LvUpIngredient1 = null;
    [SerializeField] public TextMeshProUGUI _LvUpIngredient2 = null;
    [SerializeField] public int LvUpIngredient1 = 100;
    [SerializeField] public int LvUpIngredient2 = 50;

    [SerializeField] public Button _skillButton = null;    // ��ų ��ư
    [SerializeField] private Button _UpgradeButton = null;
    [SerializeField] public TextMeshProUGUI _skillText = null;

    private static UI_Enhancement _instance = null; public static UI_Enhancement instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _status = false;
    public bool _statStatus = true;
    public bool _skillStatus = false;
    public Color _baseTabColor = new Color32(99, 85, 79, 255);
    public Color _offTabColor = new Color32(60, 54, 51, 255);

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close); 
        _statButton.onClick.AddListener(Stat);
        _LvUpButton.onClick.AddListener(LevelUp);
        _skillButton.onClick.AddListener(Skill);
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    private void Stat()
    {
        if (_statStatus == true)
        {
            return;
        }
        _statElements.SetActive(true);
        _skillElements.SetActive(false);
        _statButton.image.color = _baseTabColor;
        _skillButton.image.color = _offTabColor;
        _statStatus = true;
        _skillStatus = false;
    }

    private void Skill()
    {
        if (_skillStatus)
        {
            return;
        }
        _skillElements.SetActive(true);
        _statElements.SetActive(false);
        _statButton.image.color = _offTabColor;
        _skillButton.image.color = _baseTabColor;
        _skillStatus = true;
        _statStatus = false;

        // ��ų1 ���� �ε�
        UI_Skill.instance._skill1_Power = PlayerPrefs.GetFloat("Skill1_Power", UI_Skill.instance._skill1_Power);
        UI_Skill.instance.skill1_Level = PlayerPrefs.GetInt("Skill1_Lv", UI_Skill.instance.skill1_Level);
        UI_Skill.instance.skill1_LvUpIngredient1 = PlayerPrefs.GetInt("Skill1_LvUpIngredient1", UI_Skill.instance.skill1_LvUpIngredient1);
        UI_Skill.instance.skill1_LvUpIngredient2 = PlayerPrefs.GetInt("Skill1_LvUpIngredient2", UI_Skill.instance.skill1_LvUpIngredient2);

        // ��ų2 ���� �ε�
        UI_Skill.instance._skill2_Power = PlayerPrefs.GetFloat("Skill2_Power", UI_Skill.instance._skill2_Power);
        UI_Skill.instance.skill2_Level = PlayerPrefs.GetInt("Skill2_Lv", UI_Skill.instance.skill2_Level);
        UI_Skill.instance.skill2_LvUpIngredient1 = PlayerPrefs.GetInt("Skill2_LvUpIngredient1", UI_Skill.instance.skill2_LvUpIngredient1);
        UI_Skill.instance.skill2_LvUpIngredient2 = PlayerPrefs.GetInt("Skill2_LvUpIngredient2", UI_Skill.instance.skill2_LvUpIngredient2);

        UI_Skill.instance._skill1_LevelText.text = "+" + UI_Skill.instance.skill1_Level;
        UI_Skill.instance._skill1_PowerText.text = (UI_Skill.instance._skill1_Power * 3).ToString();
        UI_Skill.instance._skill1_LvUpIngredient1.text = UI_Main.instance._gold + "/" + UI_Skill.instance.skill1_LvUpIngredient1;
        UI_Skill.instance._skill1_LvUpIngredient2.text = UI_Main.instance._refinedEssence + "/" + UI_Skill.instance.skill1_LvUpIngredient2;

        UI_Skill.instance._skill2_LevelText.text = "+" + UI_Skill.instance.skill2_Level;
        UI_Skill.instance._skill2_PowerText.text = (UI_Skill.instance._skill2_Power * 3).ToString();
        UI_Skill.instance._skill2_LvUpIngredient1.text = UI_Main.instance._gold + "/" + UI_Skill.instance.skill2_LvUpIngredient1;
        UI_Skill.instance._skill2_LvUpIngredient2.text = UI_Main.instance._refinedEssence + "/" + UI_Skill.instance.skill2_LvUpIngredient2;
    }

    
    private void Close()
    {
        // ���� UI�� �ִ� ��� ��ư Ȱ��ȭ
        UI_Main.instance._shopButton.interactable = true;
        UI_Main.instance._mapButton.interactable = true;
        UI_Main.instance._playerInfoButton.interactable = true;
        UI_Main.instance._menuButton.interactable = true;

        _status = false;   
        UI_Main.instance.isActive = true;   // ���� UI Ȱ��ȭ
        SetStatus(false); 
        Building.selectedInstance = null;

        // ��ų1 ���� ����
        PlayerPrefs.SetFloat("Skill1_Power", UI_Skill.instance._skill1_Power);
        PlayerPrefs.SetInt("Skill1_Lv", UI_Skill.instance.skill1_Level);
        PlayerPrefs.SetInt("Skill1_LvUpIngredient1", UI_Skill.instance.skill1_LvUpIngredient1);
        PlayerPrefs.SetInt("Skill1_LvUpIngredient2", UI_Skill.instance.skill1_LvUpIngredient2);

        // ��ų2 ���� ����
        PlayerPrefs.SetFloat("Skill2_Power", UI_Skill.instance._skill2_Power);
        PlayerPrefs.SetInt("Skill2_Lv", UI_Skill.instance.skill2_Level);
        PlayerPrefs.SetInt("Skill2_LvUpIngredient1", UI_Skill.instance.skill2_LvUpIngredient1);
        PlayerPrefs.SetInt("Skill2_LvUpIngredient2", UI_Skill.instance.skill2_LvUpIngredient2);

        PlayerPrefs.Save();
    }

    public void LevelUp()
    {
        if (UI_Main.instance._gold < LvUpIngredient1 || UI_Main.instance._essence < LvUpIngredient2)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }
        UI_Main.instance._playerLv += 1;
        UI_Main.instance._levelText.text = "Lv" + UI_Main.instance._playerLv.ToString();
        _currentLevelText.text = "Level" + UI_Main.instance._playerLv.ToString();
        _nextLevelText.text = "Level" + (UI_Main.instance._playerLv + 1).ToString();
        UI_Main.instance._playeratkpow += 50f;
        UI_Main.instance._playeratkspd /= 1.005f;
        UI_Main.instance._gold -= LvUpIngredient1;
        UI_Main.instance._essence -= LvUpIngredient2;
        LvUpIngredient1 = (LvUpIngredient1 * 11) / 10;
        LvUpIngredient2 = (LvUpIngredient2 * 11) / 10;
        _LvUpIngredient1.text = UI_Main.instance._gold + " / " + LvUpIngredient1;
        _LvUpIngredient2.text = UI_Main.instance._essence + " / " + LvUpIngredient2;
        UI_Main.instance._goldText.text = UI_Main.instance._gold.ToString();
        UI_Main.instance._essenceText.text = UI_Main.instance._essence.ToString();
        if (UI_Main.instance._playerLv % 5 == 0)
        {
            UI_Main.instance._getEssence += 5;
        }
    }
}
