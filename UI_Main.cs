using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour 
{
    [SerializeField] public GameObject _elements = null; 
    [SerializeField] public TextMeshProUGUI _nameText = null; 
    [SerializeField] public TextMeshProUGUI _levelText = null; 
    [SerializeField] public TextMeshProUGUI _goldText = null; 
    [SerializeField] public TextMeshProUGUI _essenceText = null; 
    [SerializeField] public TextMeshProUGUI _gemText = null;
    [SerializeField] public TextMeshProUGUI _refinedEssenceText = null;
    [SerializeField] public Button _shopButton = null; 
    [SerializeField] public Button _playerInfoButton = null; 
    [SerializeField] public Button _mapButton = null; 
    [SerializeField] public Button _menuButton = null; 

    [SerializeField] public BuildGrid _grid = null;
    [SerializeField] public Building[] _buildingPrefabs = null; 
    private static UI_Main _instance = null; public static UI_Main instance { get { return _instance; } } 

    private bool _active = true; public bool isActive { get { return _active; } set { _active = value; } } 


    public string _playerName = "Player"; public string _playername { get { return _playerName; } set { _playerName = value; } }
    public int _playerLv = 1; public int _playerlv { get { return _playerLv; } set { _playerLv = value; } }
    public float _playerAttackPower = 50f; public float _playeratkpow { get { return _playerAttackPower; } set { _playerAttackPower = value; } }
    public float _playerAttackSpeed = 1f; public float _playeratkspd { get { return _playerAttackSpeed; } set { _playerAttackSpeed = value; } }
    public int _GetEssence = 0; public int _getEssence { get { return _GetEssence; } set { _GetEssence = value; } } // 전투를 통해 얻는 정수의 획득량을 증가시킨다. ((전투를 통해 얻은 정수 * _GetEssence값) / 100)을 더해주는 형식으로 사용한다.


    private int defaultGold = 1000000; public int _gold { get { return defaultGold; } set { defaultGold = value; } }   
    private int defaultEssence = 1000000; public int _essence { get { return defaultEssence; } set { defaultEssence = value; } }  
    private int defaultGem = 100; public int _gem { get { return defaultGem; } set { defaultGem = value; } }
    private int defaultRefinedEssence = 10000; public int _refinedEssence { get { return defaultRefinedEssence; } set { defaultRefinedEssence = value; } }

    private void Awake()
    {
        _instance = this;
        _elements.SetActive(true); 
    }

    private void Start()
    {
        Debug.Log(_playerName);
        _nameText.text = _playerName;  
        _levelText.text = "Lv " + _playerLv.ToString(); 
        _goldText.text = defaultGold.ToString(); 
        _essenceText.text = defaultEssence.ToString(); 
        _gemText.text = defaultGem.ToString();
        _refinedEssenceText.text = defaultRefinedEssence.ToString();

        _shopButton.onClick.AddListener(ShopButtonClicked); 
        _playerInfoButton.onClick.AddListener(PlayerInfoButtonClicked); 
        _mapButton.onClick.AddListener(MapButtonClicked); 
        _menuButton.onClick.AddListener(MenuButtonClicked);
    }

    
    private void ShopButtonClicked()
    {
        UI_Build.instance.Cancel(); 
        UI_Shop.instance.SetStatus(true); 
        SetStatus(false);

        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);

        if (Building.selectedInstance != null)
        {
            Building.selectedInstance.Deselected();
        }
    }

    private void PlayerInfoButtonClicked()
    {
        UI_Build.instance.Cancel(); 
        UI_PlayerInfo.instance.SetStatus(true);
        UI_PlayerInfo.instance._playerNameText.text = _playerName;
        UI_PlayerInfo.instance._playerLevelText.text = "Lv " + _playerLv.ToString();
        UI_PlayerInfo.instance._AtkPowText.text = _playerAttackPower.ToString();
        UI_PlayerInfo.instance._AtkSpdText.text = (1f + (_playerLv - 1) * 0.005f).ToString();
        UI_PlayerInfo.instance._GetEssenceText.text = _GetEssence.ToString();

        _active = false; 
        if (UI_PlayerInfo.instance._status)
        {
            _shopButton.interactable = false;
            _mapButton.interactable = false;
            _playerInfoButton.interactable = false;
            _menuButton.interactable = false;
        }

        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);
    }

    
    private void MapButtonClicked()
    {
        UI_Build.instance.Cancel(); 
        UI_Map.instance.SetStatus(true); 

        _active = false;
        if (UI_PlayerInfo.instance._status)
        {
            _shopButton.interactable = false;
            _mapButton.interactable = false;
            _playerInfoButton.interactable = false;
            _menuButton.interactable = false;
        }

        
        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);
    }

    
    private void MenuButtonClicked()
    {
        UI_Build.instance.Cancel();
        UI_Menu.instance.SetStatus(true); 

        _active = false;
        if (UI_PlayerInfo.instance._status)
        {
            _shopButton.interactable = false;
            _mapButton.interactable = false;
            _playerInfoButton.interactable = false;
            _menuButton.interactable = false;
        }

        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);
    }

   
    public void IncreaseGold(int amount)
    {
        defaultGold += amount;
        _goldText.text = defaultGold.ToString();
        UI_Shop.instance._goldText.text = defaultGold.ToString();
    }

    public void SetStatus(bool status)
    {
        _active = status;
        _elements.SetActive(status); 
    }
}

