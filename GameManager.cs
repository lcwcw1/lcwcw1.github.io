using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null; public static GameManager instance { get { return _instance; } } // 싱글턴 인스턴스

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
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
        LoadGameData(); // 게임 시작 시 데이터 로드
        if (!UI_Equipment.instance.weapon1_status && !UI_Equipment.instance.weapon2_status && !UI_Equipment.instance.weapon3_status && !UI_Equipment.instance.weapon4_status && !UI_Equipment.instance.weapon5_status)
        {
            UI_Equipment.instance.weapon1_status = true;
        }
    }

    public void SaveGameData()
    {
        // 플레이어 정보 저장
        PlayerPrefs.SetString("PlayerName", UI_Main.instance._playername);
        PlayerPrefs.SetInt("PlayerLevel", UI_Main.instance._playerlv);
        PlayerPrefs.SetFloat("PlayerAtkPow", UI_Main.instance._playeratkpow);
        PlayerPrefs.SetFloat("PlayerAtkSpd", UI_Main.instance._playeratkspd);
        PlayerPrefs.SetInt("PlayerLvUpIngredient1", UI_Enhancement.instance.LvUpIngredient1);
        PlayerPrefs.SetInt("PlayerLvUpIngredient2", UI_Enhancement.instance.LvUpIngredient2);
        PlayerPrefs.SetInt("PlayerGetEssence", UI_Main.instance._getEssence);

        // 골드, 몬스터 정수, 젬 저장
        PlayerPrefs.SetInt("Gold", UI_Main.instance._gold);
        PlayerPrefs.SetInt("Essence", UI_Main.instance._essence);
        PlayerPrefs.SetInt("Gem", UI_Main.instance._gem);
        PlayerPrefs.SetInt("RefinedEssence", UI_Main.instance._refinedEssence);

        // 설치된 건물 저장
        PlayerPrefs.SetInt("BuildingCount", BuildGrid.instance.buildings.Count); // 설치된 건물 수 저장
        for (int i = 0; i < BuildGrid.instance.buildings.Count; i++)
        {
            PlayerPrefs.SetString("Building_" + i + "_Name", BuildGrid.instance.buildings[i]._buildingName); // 건물 이름 저장
            PlayerPrefs.SetInt("Building_" + i + "_Level", BuildGrid.instance.buildings[i].CurrentLevel.level); // 건물 레벨 저장
            PlayerPrefs.SetInt("Building_" + i + "_X", BuildGrid.instance.buildings[i].currentX); // X 좌표 저장
            PlayerPrefs.SetInt("Building_" + i + "_Y", BuildGrid.instance.buildings[i].currentY); // Y 좌표 저장
            PlayerPrefs.SetInt("Building_" + i + "_CurrentLevelIndex", BuildGrid.instance.buildings[i]._currentLevelIndex);
        }

        PlayerPrefs.SetInt("Weapon1_Status", UI_Equipment.instance.weapon1_status ? 1 : 0);
        PlayerPrefs.SetInt("Weapon2_Status", UI_Equipment.instance.weapon2_status ? 1 : 0);
        PlayerPrefs.SetInt("Weapon3_Status", UI_Equipment.instance.weapon3_status ? 1 : 0);
        PlayerPrefs.SetInt("Weapon4_Status", UI_Equipment.instance.weapon4_status ? 1 : 0);
        PlayerPrefs.SetInt("Weapon5_Status", UI_Equipment.instance.weapon5_status ? 1 : 0);

        PlayerPrefs.SetString("LastSaveTime", System.DateTime.Now.ToString());

        PlayerPrefs.Save(); // 변경 사항 저장
    }

    public void LoadGameData()
    {
        // 플레이어 정보 로드
        UI_Main.instance._playername = PlayerPrefs.GetString("PlayerName", UI_Main.instance._playername);
        Debug.Log(UI_Main.instance._playername);
        UI_Main.instance._playerlv = PlayerPrefs.GetInt("PlayerLevel", UI_Main.instance._playerlv);
        UI_Main.instance._playeratkpow = PlayerPrefs.GetFloat("PlayerAtkPow", UI_Main.instance._playeratkpow);
        UI_Main.instance._playeratkspd = PlayerPrefs.GetFloat("PlayerAtkSpd", UI_Main.instance._playeratkspd);
        UI_Enhancement.instance.LvUpIngredient1 = PlayerPrefs.GetInt("PlayerLvUpIngredient1", UI_Enhancement.instance.LvUpIngredient1);
        UI_Enhancement.instance.LvUpIngredient2 = PlayerPrefs.GetInt("PlayerLvUpIngredient2", UI_Enhancement.instance.LvUpIngredient2);
        UI_Main.instance._getEssence = PlayerPrefs.GetInt("PlayerGetEssence", UI_Main.instance._getEssence);

        // 골드, 몬스터 정수, 젬 로드
        UI_Main.instance._gold = PlayerPrefs.GetInt("Gold", UI_Main.instance._gold);
        UI_Main.instance._essence = PlayerPrefs.GetInt("Essence", UI_Main.instance._essence); 
        UI_Main.instance._gem = PlayerPrefs.GetInt("Gem", UI_Main.instance._gem);
        UI_Main.instance._refinedEssence = PlayerPrefs.GetInt("RefinedEssence", UI_Main.instance._refinedEssence);

        // 설치한 건물 정보 로드
        int buildingCount = PlayerPrefs.GetInt("BuildingCount", 0); // 설치된 건물 수 로드

        for (int i = 0; i < buildingCount; i++)
        {
            string buildingName = PlayerPrefs.GetString("Building_" + i + "_Name");
            int level = PlayerPrefs.GetInt("Building_" + i + "_Level");
            int x = PlayerPrefs.GetInt("Building_" + i + "_X");
            int y = PlayerPrefs.GetInt("Building_" + i + "_Y");
            int currentLevelIndex = PlayerPrefs.GetInt("Building_" + i + "_CurrentLevelIndex", 0);

            // 프리팹에서 건물 생성
            Building buildingPrefab = Array.Find(UI_Main.instance._buildingPrefabs, b => b._buildingName == buildingName);
            if (buildingPrefab != null)
            {
                Building building = Instantiate(buildingPrefab, Vector3.zero, Quaternion.identity);
                building.PlacedOnGrid(x, y); // 그리드에 건물 배치
                building._currentLevelIndex = currentLevelIndex;
                building.CurrentLevel.level = level; // 레벨 설정

                // 바닥 비활성화
                if (building._baseArea != null)
                {
                    building._baseArea.gameObject.SetActive(false);
                }

                if (building._buildingName == "금광산")
                {
                    building._canIncreaseGold = true;
                }

                AddBuilding(building); // 빌딩 리스트에 추가
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

            // 경과된 초를 계산
            float secondsElapsed = (float)timeElapsed.TotalSeconds;

            Debug.Log("Total Buildings: " + BuildGrid.instance.buildings.Count);

            // 경과된 시간에 따라 증가할 골드 계산
            foreach (var building in BuildGrid.instance.buildings)
            {
                Debug.Log(building._buildingName);
                if (building._buildingName == "금광산")
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
            BuildGrid.instance.buildings.Add(building); // 목록에 건물 추가
            SaveGameData(); // 건물 추가 후 데이터 저장
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameData(); // 게임 종료 시 데이터 저장
    }
}
