using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // 싱글턴 패턴으로 건물 인스턴스 관리
    private static Building _buildInstance = null; public static Building buildInstance { get { return _buildInstance; } set { _buildInstance = value; } }

    // 싱글턴 패턴으로 선택된 건물 인스턴스 관리
    private static Building _selectedInstance = null; public static Building selectedInstance { get { return _selectedInstance; } set { _selectedInstance = value; } } 

    [System.Serializable]
    public class Level // 건물의 레벨 관련 데이터
    {
        public int level = 1; // 레벨
        public Sprite icon = null; // 아이콘
        public GameObject mesh = null; // 건물 오브젝트
        public int genGold = 1;     // 금광산의 골드 증가량
        public int Amount1 = 0;     // 건물의 현재 특성 값 ( 예시] 금광산의 골드 획득량, 정제소의 정수 필요량 등 )
        public int Amount2 = 0;     // 특성이 2개인 건물에 사용 ( 예시] 저장소 )
        public int NextAmount1 = 0;     // 건물의 다음 특성 값
        public int NextAmount2 = 0;     // 건물의 다음 특성 값
        public int ingredient1 = 0;     // 건물의 강화 재료1
        public int ingredient2 = 0;     // 건물의 강화 재료2
    }

    private BuildGrid _grid = null; // 빌딩이 배치될 그리드

    [SerializeField] public string _buildingName = null;    // 건물 이름
    [SerializeField] private int _rows = 1; public int rows { get { return _rows; } } // 행 수
    [SerializeField] private int _columns = 1; public int columns { get { return _columns; } } // 열 수
    [SerializeField] public MeshRenderer _baseArea = null; // 건물의 기반 영역
    [SerializeField] private Level[] _levels = null; // 건물 레벨 배열

    public int currentIndex = 0;

    // 현재 레벨 인덱스를 저장하는 필드
    public int _currentLevelIndex = 0;

    // 현재 레벨을 반환하는 속성
    public Level CurrentLevel => _levels[_currentLevelIndex];


    private int _currentX = 0; public int currentX { get { return _currentX; } } // 현재 X 좌표
    private int _currentY = 0; public int currentY { get { return _currentY; } } // 현재 Y 좌표
    private int _X = 0; // 초기 X 좌표
    private int _Y = 0; // 초기 Y 좌표
    private int _originalX = 0; // 원래 X 좌표
    private int _originalY = 0; // 원래 Y 좌표

    // 골드 증가 가능 여부
    public bool _canIncreaseGold = true;

    // 재화의 증가량과 주기를 설정합니다.
    public float goldIncreaseInterval = 10f; // 골드 증가 주기 (초)

    private void Start()
    {
        // 건물 종류에 따라 골드 혹은 자원 증가 코루틴 시작
        if (_buildingName == "금광산")
        {
            StartCoroutine(IncreaseGoldRoutine());
        }
    }

    // 골드 증가 코루틴
    private IEnumerator IncreaseGoldRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(goldIncreaseInterval);
            // 골드 증가 가능 여부 확인
            if (_canIncreaseGold)
            {
                UI_Main.instance.IncreaseGold(CurrentLevel.genGold); // 골드 증가
            }
        }
    }

    public void LevelUp()
    {
        currentIndex = CurrentLevel.level - 1;
        Debug.Log("currentIndex: " + currentIndex);

        // 최대 레벨일 때는 레벨을 올리지 않습니다.
        if (currentIndex < _levels.Length - 1)
        {
            _currentLevelIndex++; // 다음 레벨로 이동
            Debug.Log("currentLevelIndex: " + _currentLevelIndex);
            UI_BuildingLvUp.instance._buildingLevelText.text = "Lv " + CurrentLevel.level;
            UI_BuildingLvUp.instance._buildingNextLevelText.text = "Lv " + (CurrentLevel.level + 1);

            if (selectedInstance._buildingName == "금광산")
            {
                UI_BuildingLvUp.instance._gmCurrentAmount.text = selectedInstance.CurrentLevel.Amount1.ToString();
                UI_BuildingLvUp.instance._gmNextAmount.text = selectedInstance.CurrentLevel.NextAmount1.ToString();
                UI_BuildingLvUp.instance._gmIngredient1.text = UI_Main.instance._gold + " / " + selectedInstance.CurrentLevel.ingredient1;
                UI_BuildingLvUp.instance._gmIngredient2.text = UI_Main.instance._essence + " / " + selectedInstance.CurrentLevel.ingredient2;
            }
            else if (selectedInstance._buildingName == "정제소")
            {
                UI_BuildingLvUp.instance._erCurrentAmount.text = selectedInstance.CurrentLevel.Amount1.ToString();
                UI_BuildingLvUp.instance._erNextAmount.text = selectedInstance.CurrentLevel.NextAmount1.ToString();
                UI_BuildingLvUp.instance._erIngredient1.text = UI_Main.instance._gold + " / " + selectedInstance.CurrentLevel.ingredient1;
                UI_BuildingLvUp.instance._erIngredient2.text = UI_Main.instance._essence + " / " + selectedInstance.CurrentLevel.ingredient2;
            }
            else
            {
                UI_BuildingLvUp.instance._stgCurrentAmount.text =selectedInstance.CurrentLevel.Amount1.ToString();
                UI_BuildingLvUp.instance._stgNextAmount.text = selectedInstance.CurrentLevel.NextAmount1.ToString();
                UI_BuildingLvUp.instance._steCurrentAmount.text = selectedInstance.CurrentLevel.Amount2.ToString();
                UI_BuildingLvUp.instance._steNextAmount.text = selectedInstance.CurrentLevel.NextAmount2.ToString();
                UI_BuildingLvUp.instance._stIngredient1.text = UI_Main.instance._gold + " / " + selectedInstance.CurrentLevel.ingredient1;
                UI_BuildingLvUp.instance._stIngredient2.text = UI_Main.instance._refinedEssence + " / " + selectedInstance.CurrentLevel.ingredient2;
            }

            UI_BuildingLvUp.instance._gmLvUpButton.interactable = true;
        }

        // 최대 레벨에 도달하면 레벨업 버튼을 비활성화
        if (currentIndex == _levels.Length - 1)
        {
            UI_Caution.instance.SetLStatus(true);
            UI_BuildingLvUp.instance._gmLvUpButton.interactable = false;
        }
    }


    // 그리드에 건물이 배치될 때 호출
    public void PlacedOnGrid(int x, int y)
    {
        _currentX = x;  // 현재 X 좌표 설정
        _currentY = y;  // 현재 Y 좌표 설정
        _X = x; // 초기 X 좌표 저장
        _Y = y; // 초기 Y 좌표 저장
        Vector3 position = UI_Main.instance._grid.GetCenterPosition(x, y, _rows, _columns); // 그리드의 중앙 위치 계산
        transform.position = position; // 위치 설정
        SetBaseColor(); // 기반 색상 설정
        UI_Main.instance._grid.AddBuilding(this); // 그리드에 건물 추가
    }

    // 그리드에서 이동을 시작할 때 호출
    public void StartMovingOnGrid()
    {
        _X = _currentX; // 현재 X 좌표 저장
        _Y = _currentY; // 현재 Y 좌표 저장
    }

    // 그리드에서 제거될 때 호출
    public void RemovedFromGrid()
    {
        UI_Main.instance._grid.RemoveBuilding(this);    // 그리드에서 건물 제거
        _buildInstance = null; // 인스턴스 초기화
        UI_Build.instance.SetStatus(false); // 빌드 UI 비활성화
        CameraController.instance.isPlacingBuilding = false; // 건물 배치 상태 해제
        Destroy(gameObject); // 현재 건물 게임 오브젝트 삭제
    }

    // 그리드 상에서 위치 업데이트
    public void UpdateGridPosition(Vector3 basePosition, Vector3 currentPosition)
    {
        // 이동 방향 계산
        Vector3 dir = UI_Main.instance._grid.transform.TransformPoint(currentPosition) - UI_Main.instance._grid.transform.TransformPoint(basePosition);

        int xDis = Mathf.RoundToInt(dir.z / UI_Main.instance._grid.cellSize); // X 방향 거리 계산
        int yDis = Mathf.RoundToInt(-dir.x / UI_Main.instance._grid.cellSize); // Y 방향 거리 계산

        _currentX = _X + xDis; // 새로운 X 좌표 계산
        _currentY = _Y + yDis; // 새로운 Y 좌표 계산

        Vector3 position = UI_Main.instance._grid.GetCenterPosition(_currentX, _currentY, _rows, _columns);
        transform.position = position; // 위치 업데이트

        SetBaseColor(); // 기반 색상 업데이트 (배치가 가능하다면 초록색, 불가능하다면 빨간색)
    }

    // 바닥이 건물 배치 혹은 재배치의 여부를 판단하는 색상을 설정하는 메서드 (가능: 초록, 불가능: 빨강)
    private void SetBaseColor()
    {
        // 현재 위치에 건물을 배치할 수 있는지 확인
        if (UI_Main.instance._grid.CanPlaceBuilding(this, currentX, currentY))
        {
            UI_Build.instance.clickConfirmButton.interactable = true;   // 확인 버튼 활성화

            // 모든 건물의 재배치 버튼 활성화
            UI_BuildingOptions.instance._replaceButton.interactable = true; 
            UI_EnhancementBuildingOptions.instance._replaceButton.interactable = true;
            UI_RefineryBuildingOptions.instance._replaceButton.interactable = true;
            UI_BlacksmithBuildingOptions.instance._replaceButton.interactable = true;

            _baseArea.sharedMaterial.color = Color.green; // 배치 가능 시 초록색
        }
        else
        {
            UI_Build.instance.clickConfirmButton.interactable = false; // 확인 버튼 비활성화

            // 모든 건물의 재배치 버튼 비활성화
            UI_BuildingOptions.instance._replaceButton.interactable = false;
            UI_EnhancementBuildingOptions.instance._replaceButton.interactable = false;
            UI_RefineryBuildingOptions.instance._replaceButton.interactable = false;
            UI_BlacksmithBuildingOptions.instance._replaceButton.interactable = false;

            _baseArea.sharedMaterial.color = Color.red; // 배치 불가 시 빨간색
        }
    }

    public void RemoveBaseArea()
    {
        // 설치 후 설치 가능 여부를 나타내는 바닥 오브젝트를 비활성화
        if (_baseArea != null)
        {
            _baseArea.gameObject.SetActive(false); // 비활성화
        }
    }

    public void Selected() // 화면상의 건물을 선택
    {
        if (selectedInstance != null) // 이미 선택된 건물이 있을 경우
        {
            if (selectedInstance == this)   // 같은 건물이 선택된 경우
            {
                return; // 아무 작업도 하지 않음
            }
            else
            {
                selectedInstance.Deselected();  // 이전 건물 선택 해제
            }
        }
        _originalX = currentX;  // 원래 X 좌표 저장
        _originalY = currentY;  // 원래 Y 좌표 저장
        selectedInstance = this;    // 현재 선택된 건물 설정

        // 선택된 건물 종류에 따라 해당 건물의 옵션 UI 활성화
        if (selectedInstance._buildingName == "강화소")
        {
            UI_EnhancementBuildingOptions.instance._buildingText.text = selectedInstance._buildingName + " Lv " + selectedInstance.CurrentLevel.level;
            UI_EnhancementBuildingOptions.instance.SetStatus(true);
        }
        else if (selectedInstance._buildingName == "정제소")
        {
            UI_RefineryBuildingOptions.instance._buildingText.text = selectedInstance._buildingName + " Lv " + selectedInstance.CurrentLevel.level;
            UI_RefineryBuildingOptions.instance.SetStatus(true);
        }
        else if (selectedInstance._buildingName == "대장간")
        {
            UI_BlacksmithBuildingOptions.instance._buildingText.text = selectedInstance._buildingName + " Lv " + selectedInstance.CurrentLevel.level;
            UI_BlacksmithBuildingOptions.instance.SetStatus(true);
        }
        //else if (selectedInstance._buildingName == "SkillStore")
        //{
        //    UI_SkillBuildingOptions.instance.SetStatus(true);
        //}
        else
        {
            UI_BuildingOptions.instance._buildingText.text = selectedInstance._buildingName + " Lv " + selectedInstance.CurrentLevel.level;
            UI_BuildingOptions.instance.SetStatus(true);
        }
    }

    public void Deselected() // 화면상의 건물 선택 해제
    {
        // 모든 건물의 옵션 UI 비활성화
        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);

        CameraController.instance.isReplacingBuilding = false;  // 건물 재배치 상태 해제
        if (_originalX != currentX || _originalY != currentY) // 선택 해제 시 건물의 원래 위치와 다를 경우
        {
            if (UI_Main.instance._grid.CanPlaceBuilding(this, _currentX, _currentY))    // 변경된 위치가 건물이 있을 수 있는 위치일 경우
            {
                _baseArea.gameObject.SetActive(false);  // 바닥 오브젝트 비활성화
            }
            else    // 변경된 위치가 건물이 있을 수 없는 위치일 경우
            {
                PlacedOnGrid(_originalX, _originalY);   // 원래 위치로 되돌리기
                _baseArea.gameObject.SetActive(false);  // 바닥 오브젝트 비활성화
            }
        }
        selectedInstance = null;    // 선택된 인스턴스 초기화
    }

}


