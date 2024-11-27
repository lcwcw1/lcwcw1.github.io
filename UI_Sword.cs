using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Sword : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;

    [System.Serializable]
    public class WeaponData
    {
        public int level;
        public float power;
        public int lvUpIngredient1;
        public int lvUpIngredient2;
    }

    [SerializeField] public TextMeshProUGUI _swordText = null;
    [SerializeField] public TextMeshProUGUI _swordLevelText = null;
    [SerializeField] public TextMeshProUGUI _swordPowerText = null;
    [SerializeField] public WeaponData[] weaponDataArray; // 무기 데이터를 배열로 관리
    public int currentSwordIndex = 0; // 현재 무기 인덱스

    [SerializeField] public TextMeshProUGUI _swordLvUpIngredient1 = null;
    [SerializeField] public TextMeshProUGUI _swordLvUpIngredient2 = null;
    [SerializeField] private Button _swordUpgradeButton = null;
    [SerializeField] private Button _nextSwordButton = null;
    [SerializeField] public GameObject _nextSwordElements = null;

    private static UI_Sword _instance = null; public static UI_Sword instance { get { return _instance; } } // 싱글턴 인스턴스

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
    }

    private void Start()
    {
        currentSwordIndex = PlayerPrefs.GetInt("SwordLevel", currentSwordIndex);
        
        // UI 업데이트
        UpdateSwordUI();

        _swordUpgradeButton.onClick.AddListener(Sword_Upgrade);
        _nextSwordButton.onClick.AddListener(NextSword);
    }

    private void UpdateSwordUI()
    {
        WeaponData currentWeaponData = weaponDataArray[currentSwordIndex];

        _swordLevelText.text = "+" + currentWeaponData.level;
        _swordPowerText.text = currentWeaponData.power.ToString();
        _swordLvUpIngredient1.text = UI_Main.instance._gold + "/" + currentWeaponData.lvUpIngredient1;
        _swordLvUpIngredient2.text = UI_Main.instance._refinedEssence + "/" + currentWeaponData.lvUpIngredient2;

        if (currentWeaponData.level == 5)
        {
            _swordUpgradeButton.gameObject.SetActive(false);
            _nextSwordButton.gameObject.SetActive(true);
        }
    }

    private void Sword_Upgrade()
    {
        WeaponData currentWeaponData = weaponDataArray[currentSwordIndex];
        Debug.Log(currentSwordIndex);

        if (UI_Main.instance._gold < currentWeaponData.lvUpIngredient1 || UI_Main.instance._refinedEssence < currentWeaponData.lvUpIngredient2)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }

        UI_Main.instance._gold -= currentWeaponData.lvUpIngredient1;
        UI_Main.instance._refinedEssence -= currentWeaponData.lvUpIngredient2;

        currentSwordIndex++;

        UpdateSwordUI();

        UI_Main.instance._goldText.text = UI_Main.instance._gold.ToString();
        UI_Main.instance._refinedEssenceText.text = UI_Main.instance._refinedEssence.ToString();

        if (currentWeaponData.level == 5)
        {
            _swordUpgradeButton.gameObject.SetActive(false);
            _nextSwordButton.gameObject.SetActive(true);
            currentSwordIndex--;

            if (_elements.name == "Sword5")
            {
                _nextSwordButton.interactable = false;
            }
        }

        PlayerPrefs.SetInt("SwordLevel", currentSwordIndex);
    }

    private void NextSword()
    {
        WeaponData currentWeaponData = weaponDataArray[currentSwordIndex];

        if (UI_Main.instance._gold < currentWeaponData.lvUpIngredient1 || UI_Main.instance._refinedEssence < currentWeaponData.lvUpIngredient2)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }

        if (_elements.name == "Sword5")
        {
            UI_Caution.instance.SetLStatus(true);
            _nextSwordButton.interactable = false;
            return;
        }

        UI_Main.instance._gold -= currentWeaponData.lvUpIngredient1;
        UI_Main.instance._refinedEssence -= currentWeaponData.lvUpIngredient2;

        if (_nextSwordElements.name == "Sword2")
        {
            UI_Equipment.instance._weaponIconImage.sprite = UI_Equipment.instance._weaponIcon2;
            UI_Equipment.instance.weapon1_status = false;
            UI_Equipment.instance.weapon2_status = true;
        }
        else if (_nextSwordElements.name == "Sword3")
        {
            UI_Equipment.instance._weaponIconImage.sprite = UI_Equipment.instance._weaponIcon3;
            UI_Equipment.instance.weapon2_status = false;
            UI_Equipment.instance.weapon3_status = true;
        }
        else if (_nextSwordElements.name == "Sword4")
        {
            UI_Equipment.instance._weaponIconImage.sprite = UI_Equipment.instance._weaponIcon4;
            UI_Equipment.instance.weapon3_status = false;
            UI_Equipment.instance.weapon4_status = true;
        }
        else
        {
            UI_Equipment.instance._weaponIconImage.sprite = UI_Equipment.instance._weaponIcon5;
            UI_Equipment.instance.weapon4_status = false;
            UI_Equipment.instance.weapon5_status = true;
        }

        currentSwordIndex = 0;

        PlayerPrefs.SetInt("SwordLevel", currentSwordIndex);

        _elements.SetActive(false);
        _nextSwordElements.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("SwordLevel", currentSwordIndex);
    }
}
