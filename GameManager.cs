using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null; public static GameManager instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("LastSaveTime"))
        {
            Debug.Log("LastSaveTime exists.");
        }
        else
        {
            Debug.Log("LastSaveTime does not exist.");
        }
        LoadGameData(); // ���� ���� �� ������ �ε�
        if (!UI_Equipment.instance.weapon1_status && !UI_Equipment.instance.weapon2_status && !UI_Equipment.instance.weapon3_status && !UI_Equipment.instance.weapon4_status && !UI_Equipment.instance.weapon5_status)
        {
            UI_Equipment.instance.weapon1_status = true;
        }
    }

    public void SaveGameData()
    {
        // �÷��̾� ���� ����
        PlayerPrefs.SetString("PlayerName", UI_Main.instance._playername);
        PlayerPrefs.SetInt("PlayerLevel", UI_Main.instance._playerlv);
        PlayerPrefs.SetFloat("PlayerAtkPow", UI_Main.instance._playeratkpow);
        PlayerPrefs.SetFloat("PlayerAtkSpd", UI_Main.instance._playeratkspd);
        PlayerPrefs.SetInt("PlayerLvUpIngredient1", UI_Enhancement.instance.LvUpIngredient1);
        PlayerPrefs.SetInt("PlayerLvUpIngredient2", UI_Enhancement.instance.LvUpIngredient2);
        PlayerPrefs.SetInt("PlayerGetEssence", UI_Main.instance._getEssence);

        // ���, ���� ����, �� ����
        PlayerPrefs.SetInt("Gold", UI_Main.instance._gold);
        PlayerPrefs.SetInt("Essence", UI_Main.instance._essence);
        PlayerPrefs.SetInt("Gem", UI_Main.instance._gem);
        PlayerPrefs.SetInt("RefinedEssence", UI_Main.instance._refinedEssence);

        // ��ġ�� �ǹ� ����
        PlayerPrefs.SetInt("BuildingCount", BuildGrid.instance.buildings.Count); // ��ġ�� �ǹ� �� ����
        for (int i = 0; i < BuildGrid.instance.buildings.Count; i++)
        {
            PlayerPrefs.SetString("Building_" + i + "_Name", BuildGrid.instance.buildings[i]._buildingName); // �ǹ� �̸� ����
            PlayerPrefs.SetInt("Building_" + i + "_Level", BuildGrid.instance.buildings[i].CurrentLevel.level); // �ǹ� ���� ����
            PlayerPrefs.SetInt("Building_" + i + "_X", BuildGrid.instance.buildings[i].currentX); // X ��ǥ ����
            PlayerPrefs.SetInt("Building_" + i + "_Y", BuildGrid.instance.buildings[i].currentY); // Y ��ǥ ����
            PlayerPrefs.SetInt("Building_" + i + "_CurrentLevelIndex", BuildGrid.instance.buildings[i]._currentLevelIndex);
        }

        PlayerPrefs.SetInt("Weapon1_Status", UI_Equipment.instance.weapon1_status ? 1 : 0);
        PlayerPrefs.SetInt("Weapon2_Status", UI_Equipment.instance.weapon2_status ? 1 : 0);
        PlayerPrefs.SetInt("Weapon3_Status", UI_Equipment.instance.weapon3_status ? 1 : 0);
        PlayerPrefs.SetInt("Weapon4_Status", UI_Equipment.instance.weapon4_status ? 1 : 0);
        PlayerPrefs.SetInt("Weapon5_Status", UI_Equipment.instance.weapon5_status ? 1 : 0);

        PlayerPrefs.SetString("LastSaveTime", System.DateTime.Now.ToString());

        PlayerPrefs.Save(); // ���� ���� ����
    }

    public void LoadGameData()
    {
        // �÷��̾� ���� �ε�
        UI_Main.instance._playername = PlayerPrefs.GetString("PlayerName", UI_Main.instance._playername);
        Debug.Log(UI_Main.instance._playername);
        UI_Main.instance._playerlv = PlayerPrefs.GetInt("PlayerLevel", UI_Main.instance._playerlv);
        UI_Main.instance._playeratkpow = PlayerPrefs.GetFloat("PlayerAtkPow", UI_Main.instance._playeratkpow);
        UI_Main.instance._playeratkspd = PlayerPrefs.GetFloat("PlayerAtkSpd", UI_Main.instance._playeratkspd);
        UI_Enhancement.instance.LvUpIngredient1 = PlayerPrefs.GetInt("PlayerLvUpIngredient1", UI_Enhancement.instance.LvUpIngredient1);
        UI_Enhancement.instance.LvUpIngredient2 = PlayerPrefs.GetInt("PlayerLvUpIngredient2", UI_Enhancement.instance.LvUpIngredient2);
        UI_Main.instance._getEssence = PlayerPrefs.GetInt("PlayerGetEssence", UI_Main.instance._getEssence);

        // ���, ���� ����, �� �ε�
        UI_Main.instance._gold = PlayerPrefs.GetInt("Gold", UI_Main.instance._gold);
        UI_Main.instance._essence = PlayerPrefs.GetInt("Essence", UI_Main.instance._essence); 
        UI_Main.instance._gem = PlayerPrefs.GetInt("Gem", UI_Main.instance._gem);
        UI_Main.instance._refinedEssence = PlayerPrefs.GetInt("RefinedEssence", UI_Main.instance._refinedEssence);

        // ��ġ�� �ǹ� ���� �ε�
        int buildingCount = PlayerPrefs.GetInt("BuildingCount", 0); // ��ġ�� �ǹ� �� �ε�

        for (int i = 0; i < buildingCount; i++)
        {
            string buildingName = PlayerPrefs.GetString("Building_" + i + "_Name");
            int level = PlayerPrefs.GetInt("Building_" + i + "_Level");
            int x = PlayerPrefs.GetInt("Building_" + i + "_X");
            int y = PlayerPrefs.GetInt("Building_" + i + "_Y");
            int currentLevelIndex = PlayerPrefs.GetInt("Building_" + i + "_CurrentLevelIndex", 0);

            // �����տ��� �ǹ� ����
            Building buildingPrefab = Array.Find(UI_Main.instance._buildingPrefabs, b => b._buildingName == buildingName);
            if (buildingPrefab != null)
            {
                Building building = Instantiate(buildingPrefab, Vector3.zero, Quaternion.identity);
                building.PlacedOnGrid(x, y); // �׸��忡 �ǹ� ��ġ
                building._currentLevelIndex = currentLevelIndex;
                building.CurrentLevel.level = level; // ���� ����

                // �ٴ� ��Ȱ��ȭ
                if (building._baseArea != null)
                {
                    building._baseArea.gameObject.SetActive(false);
                }

                if (building._buildingName == "�ݱ���")
                {
                    building._canIncreaseGold = true;
                }

                AddBuilding(building); // ���� ����Ʈ�� �߰�
            }
        }

        if (PlayerPrefs.HasKey("LastSaveTime"))
        {
            string lastSaveTimeString = PlayerPrefs.GetString("LastSaveTime");
            System.DateTime lastSaveTime = System.DateTime.Parse(lastSaveTimeString);
            System.TimeSpan timeElapsed = System.DateTime.Now - lastSaveTime;
            Debug.Log(lastSaveTimeString);
            Debug.Log(lastSaveTime);
            Debug.Log(timeElapsed);

            // ����� �ʸ� ���
            float secondsElapsed = (float)timeElapsed.TotalSeconds;

            Debug.Log("Total Buildings: " + BuildGrid.instance.buildings.Count);

            // ����� �ð��� ���� ������ ��� ���
            foreach (var building in BuildGrid.instance.buildings)
            {
                Debug.Log(building._buildingName);
                if (building._buildingName == "�ݱ���")
                {
                    int goldToAdd = (int)(secondsElapsed / building.goldIncreaseInterval) * building.CurrentLevel.genGold;
                    Debug.Log(goldToAdd);
                    UI_Main.instance.IncreaseGold(goldToAdd);
                }
            }
        }

        UI_Equipment.instance.weapon1_status = PlayerPrefs.GetInt("Weapon1_Status", 0) == 1;
        UI_Equipment.instance.weapon2_status = PlayerPrefs.GetInt("Weapon2_Status", 0) == 1;
        UI_Equipment.instance.weapon3_status = PlayerPrefs.GetInt("Weapon3_Status", 0) == 1;
        UI_Equipment.instance.weapon4_status = PlayerPrefs.GetInt("Weapon4_Status", 0) == 1;
        UI_Equipment.instance.weapon5_status = PlayerPrefs.GetInt("Weapon5_Status", 0) == 1;
    }

    public void AddBuilding(Building building)
    {
        if (!BuildGrid.instance.buildings.Contains(building))
        {
            BuildGrid.instance.buildings.Add(building); // ��Ͽ� �ǹ� �߰�
            SaveGameData(); // �ǹ� �߰� �� ������ ����
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameData(); // ���� ���� �� ������ ����
    }
}
