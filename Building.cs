using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // �̱��� �������� �ǹ� �ν��Ͻ� ����
    private static Building _buildInstance = null; public static Building buildInstance { get { return _buildInstance; } set { _buildInstance = value; } }

    // �̱��� �������� ���õ� �ǹ� �ν��Ͻ� ����
    private static Building _selectedInstance = null; public static Building selectedInstance { get { return _selectedInstance; } set { _selectedInstance = value; } } 

    [System.Serializable]
    public class Level // �ǹ��� ���� ���� ������
    {
        public int level = 1; // ����
        public Sprite icon = null; // ������
        public GameObject mesh = null; // �ǹ� ������Ʈ
        public int genGold = 1;     // �ݱ����� ��� ������
        public int Amount1 = 0;     // �ǹ��� ���� Ư�� �� ( ����] �ݱ����� ��� ȹ�淮, �������� ���� �ʿ䷮ �� )
        public int Amount2 = 0;     // Ư���� 2���� �ǹ��� ��� ( ����] ����� )
        public int NextAmount1 = 0;     // �ǹ��� ���� Ư�� ��
        public int NextAmount2 = 0;     // �ǹ��� ���� Ư�� ��
        public int ingredient1 = 0;     // �ǹ��� ��ȭ ���1
        public int ingredient2 = 0;     // �ǹ��� ��ȭ ���2
    }

    private BuildGrid _grid = null; // ������ ��ġ�� �׸���

    [SerializeField] public string _buildingName = null;    // �ǹ� �̸�
    [SerializeField] private int _rows = 1; public int rows { get { return _rows; } } // �� ��
    [SerializeField] private int _columns = 1; public int columns { get { return _columns; } } // �� ��
    [SerializeField] public MeshRenderer _baseArea = null; // �ǹ��� ��� ����
    [SerializeField] private Level[] _levels = null; // �ǹ� ���� �迭

    public int currentIndex = 0;

    // ���� ���� �ε����� �����ϴ� �ʵ�
    public int _currentLevelIndex = 0;

    // ���� ������ ��ȯ�ϴ� �Ӽ�
    public Level CurrentLevel => _levels[_currentLevelIndex];


    private int _currentX = 0; public int currentX { get { return _currentX; } } // ���� X ��ǥ
    private int _currentY = 0; public int currentY { get { return _currentY; } } // ���� Y ��ǥ
    private int _X = 0; // �ʱ� X ��ǥ
    private int _Y = 0; // �ʱ� Y ��ǥ
    private int _originalX = 0; // ���� X ��ǥ
    private int _originalY = 0; // ���� Y ��ǥ

    // ��� ���� ���� ����
    public bool _canIncreaseGold = true;

    // ��ȭ�� �������� �ֱ⸦ �����մϴ�.
    public float goldIncreaseInterval = 10f; // ��� ���� �ֱ� (��)

    private void Start()
    {
        // �ǹ� ������ ���� ��� Ȥ�� �ڿ� ���� �ڷ�ƾ ����
        if (_buildingName == "�ݱ���")
        {
            StartCoroutine(IncreaseGoldRoutine());
        }
    }

    // ��� ���� �ڷ�ƾ
    private IEnumerator IncreaseGoldRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(goldIncreaseInterval);
            // ��� ���� ���� ���� Ȯ��
            if (_canIncreaseGold)
            {
                UI_Main.instance.IncreaseGold(CurrentLevel.genGold); // ��� ����
            }
        }
    }

    public void LevelUp()
    {
        currentIndex = CurrentLevel.level - 1;
        Debug.Log("currentIndex: " + currentIndex);

        // �ִ� ������ ���� ������ �ø��� �ʽ��ϴ�.
        if (currentIndex < _levels.Length - 1)
        {
            _currentLevelIndex++; // ���� ������ �̵�
            Debug.Log("currentLevelIndex: " + _currentLevelIndex);
            UI_BuildingLvUp.instance._buildingLevelText.text = "Lv " + CurrentLevel.level;
            UI_BuildingLvUp.instance._buildingNextLevelText.text = "Lv " + (CurrentLevel.level + 1);

            if (selectedInstance._buildingName == "�ݱ���")
            {
                UI_BuildingLvUp.instance._gmCurrentAmount.text = selectedInstance.CurrentLevel.Amount1.ToString();
                UI_BuildingLvUp.instance._gmNextAmount.text = selectedInstance.CurrentLevel.NextAmount1.ToString();
                UI_BuildingLvUp.instance._gmIngredient1.text = UI_Main.instance._gold + " / " + selectedInstance.CurrentLevel.ingredient1;
                UI_BuildingLvUp.instance._gmIngredient2.text = UI_Main.instance._essence + " / " + selectedInstance.CurrentLevel.ingredient2;
            }
            else if (selectedInstance._buildingName == "������")
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

        // �ִ� ������ �����ϸ� ������ ��ư�� ��Ȱ��ȭ
        if (currentIndex == _levels.Length - 1)
        {
            UI_Caution.instance.SetLStatus(true);
            UI_BuildingLvUp.instance._gmLvUpButton.interactable = false;
        }
    }


    // �׸��忡 �ǹ��� ��ġ�� �� ȣ��
    public void PlacedOnGrid(int x, int y)
    {
        _currentX = x;  // ���� X ��ǥ ����
        _currentY = y;  // ���� Y ��ǥ ����
        _X = x; // �ʱ� X ��ǥ ����
        _Y = y; // �ʱ� Y ��ǥ ����
        Vector3 position = UI_Main.instance._grid.GetCenterPosition(x, y, _rows, _columns); // �׸����� �߾� ��ġ ���
        transform.position = position; // ��ġ ����
        SetBaseColor(); // ��� ���� ����
        UI_Main.instance._grid.AddBuilding(this); // �׸��忡 �ǹ� �߰�
    }

    // �׸��忡�� �̵��� ������ �� ȣ��
    public void StartMovingOnGrid()
    {
        _X = _currentX; // ���� X ��ǥ ����
        _Y = _currentY; // ���� Y ��ǥ ����
    }

    // �׸��忡�� ���ŵ� �� ȣ��
    public void RemovedFromGrid()
    {
        UI_Main.instance._grid.RemoveBuilding(this);    // �׸��忡�� �ǹ� ����
        _buildInstance = null; // �ν��Ͻ� �ʱ�ȭ
        UI_Build.instance.SetStatus(false); // ���� UI ��Ȱ��ȭ
        CameraController.instance.isPlacingBuilding = false; // �ǹ� ��ġ ���� ����
        Destroy(gameObject); // ���� �ǹ� ���� ������Ʈ ����
    }

    // �׸��� �󿡼� ��ġ ������Ʈ
    public void UpdateGridPosition(Vector3 basePosition, Vector3 currentPosition)
    {
        // �̵� ���� ���
        Vector3 dir = UI_Main.instance._grid.transform.TransformPoint(currentPosition) - UI_Main.instance._grid.transform.TransformPoint(basePosition);

        int xDis = Mathf.RoundToInt(dir.z / UI_Main.instance._grid.cellSize); // X ���� �Ÿ� ���
        int yDis = Mathf.RoundToInt(-dir.x / UI_Main.instance._grid.cellSize); // Y ���� �Ÿ� ���

        _currentX = _X + xDis; // ���ο� X ��ǥ ���
        _currentY = _Y + yDis; // ���ο� Y ��ǥ ���

        Vector3 position = UI_Main.instance._grid.GetCenterPosition(_currentX, _currentY, _rows, _columns);
        transform.position = position; // ��ġ ������Ʈ

        SetBaseColor(); // ��� ���� ������Ʈ (��ġ�� �����ϴٸ� �ʷϻ�, �Ұ����ϴٸ� ������)
    }

    // �ٴ��� �ǹ� ��ġ Ȥ�� ���ġ�� ���θ� �Ǵ��ϴ� ������ �����ϴ� �޼��� (����: �ʷ�, �Ұ���: ����)
    private void SetBaseColor()
    {
        // ���� ��ġ�� �ǹ��� ��ġ�� �� �ִ��� Ȯ��
        if (UI_Main.instance._grid.CanPlaceBuilding(this, currentX, currentY))
        {
            UI_Build.instance.clickConfirmButton.interactable = true;   // Ȯ�� ��ư Ȱ��ȭ

            // ��� �ǹ��� ���ġ ��ư Ȱ��ȭ
            UI_BuildingOptions.instance._replaceButton.interactable = true; 
            UI_EnhancementBuildingOptions.instance._replaceButton.interactable = true;
            UI_RefineryBuildingOptions.instance._replaceButton.interactable = true;
            UI_BlacksmithBuildingOptions.instance._replaceButton.interactable = true;

            _baseArea.sharedMaterial.color = Color.green; // ��ġ ���� �� �ʷϻ�
        }
        else
        {
            UI_Build.instance.clickConfirmButton.interactable = false; // Ȯ�� ��ư ��Ȱ��ȭ

            // ��� �ǹ��� ���ġ ��ư ��Ȱ��ȭ
            UI_BuildingOptions.instance._replaceButton.interactable = false;
            UI_EnhancementBuildingOptions.instance._replaceButton.interactable = false;
            UI_RefineryBuildingOptions.instance._replaceButton.interactable = false;
            UI_BlacksmithBuildingOptions.instance._replaceButton.interactable = false;

            _baseArea.sharedMaterial.color = Color.red; // ��ġ �Ұ� �� ������
        }
    }

    public void RemoveBaseArea()
    {
        // ��ġ �� ��ġ ���� ���θ� ��Ÿ���� �ٴ� ������Ʈ�� ��Ȱ��ȭ
        if (_baseArea != null)
        {
            _baseArea.gameObject.SetActive(false); // ��Ȱ��ȭ
        }
    }

    public void Selected() // ȭ����� �ǹ��� ����
    {
        if (selectedInstance != null) // �̹� ���õ� �ǹ��� ���� ���
        {
            if (selectedInstance == this)   // ���� �ǹ��� ���õ� ���
            {
                return; // �ƹ� �۾��� ���� ����
            }
            else
            {
                selectedInstance.Deselected();  // ���� �ǹ� ���� ����
            }
        }
        _originalX = currentX;  // ���� X ��ǥ ����
        _originalY = currentY;  // ���� Y ��ǥ ����
        selectedInstance = this;    // ���� ���õ� �ǹ� ����

        // ���õ� �ǹ� ������ ���� �ش� �ǹ��� �ɼ� UI Ȱ��ȭ
        if (selectedInstance._buildingName == "��ȭ��")
        {
            UI_EnhancementBuildingOptions.instance._buildingText.text = selectedInstance._buildingName + " Lv " + selectedInstance.CurrentLevel.level;
            UI_EnhancementBuildingOptions.instance.SetStatus(true);
        }
        else if (selectedInstance._buildingName == "������")
        {
            UI_RefineryBuildingOptions.instance._buildingText.text = selectedInstance._buildingName + " Lv " + selectedInstance.CurrentLevel.level;
            UI_RefineryBuildingOptions.instance.SetStatus(true);
        }
        else if (selectedInstance._buildingName == "���尣")
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

    public void Deselected() // ȭ����� �ǹ� ���� ����
    {
        // ��� �ǹ��� �ɼ� UI ��Ȱ��ȭ
        UI_BuildingOptions.instance.SetStatus(false);
        UI_EnhancementBuildingOptions.instance.SetStatus(false);
        UI_RefineryBuildingOptions.instance.SetStatus(false);
        UI_BlacksmithBuildingOptions.instance.SetStatus(false);
        // UI_SkillBuildingOptions.instance.SetStatus(false);

        CameraController.instance.isReplacingBuilding = false;  // �ǹ� ���ġ ���� ����
        if (_originalX != currentX || _originalY != currentY) // ���� ���� �� �ǹ��� ���� ��ġ�� �ٸ� ���
        {
            if (UI_Main.instance._grid.CanPlaceBuilding(this, _currentX, _currentY))    // ����� ��ġ�� �ǹ��� ���� �� �ִ� ��ġ�� ���
            {
                _baseArea.gameObject.SetActive(false);  // �ٴ� ������Ʈ ��Ȱ��ȭ
            }
            else    // ����� ��ġ�� �ǹ��� ���� �� ���� ��ġ�� ���
            {
                PlacedOnGrid(_originalX, _originalY);   // ���� ��ġ�� �ǵ�����
                _baseArea.gameObject.SetActive(false);  // �ٴ� ������Ʈ ��Ȱ��ȭ
            }
        }
        selectedInstance = null;    // ���õ� �ν��Ͻ� �ʱ�ȭ
    }

}


