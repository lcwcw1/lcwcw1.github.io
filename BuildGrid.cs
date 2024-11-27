using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildGrid : MonoBehaviour
{
    private int _rows = 45; // �׸����� �� ��
    private int _columns = 45; // �׸����� �� ��
    private float _cellSize = 1f; // �׸��� ���� ũ��
    public float cellSize { get { return _cellSize; } } // �� ũ�� ������Ƽ

    public List<Building> buildings = new List<Building>(); // ���� �׸��忡 ��ġ�� �ǹ� ���

    private static BuildGrid _instance = null; public static BuildGrid instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
    }

    // �־��� x, y ��ǥ�� ���� ���� ��ġ�� ���
    public Vector3 GetStartPosition(int x, int y)
    {
        Vector3 position = transform.position; // �׸����� ���� ��ġ�� ������
        position += (transform.right.normalized * x * _cellSize) + (transform.forward.normalized * y * _cellSize); // x�� y�� ���� ��ġ�� ����
        return position; // ���� ��ġ�� ��ȯ
    }

    // �־��� x, y ��ǥ �� ������ ��� ���� ���� �߽� ��ġ�� ���
    public Vector3 GetCenterPosition(int x, int y, int rows, int columns)
    {
        Vector3 position = GetStartPosition(x, y); // ���� ��ġ ��������
        position += (transform.right.normalized * columns * _cellSize / 2f) + (transform.forward.normalized * rows * _cellSize / 2f); // �߽����� ������
        return position; // �߽� ��ġ ��ȯ
    }

    // �־��� x, y ��ǥ �� ������ ��� ���� ���� �� ��ġ�� ���
    public Vector3 GetEndPosition(int x, int y, int rows, int columns)
    {
        Vector3 position = GetStartPosition(x, y); // ���� ��ġ ��������
        position += (transform.right.normalized * columns * _cellSize) + (transform.forward.normalized * rows * _cellSize); // �� ��ġ�� �̵�
        return position; // �� ��ġ ��ȯ
    }

    // Ư�� �ǹ��� �� ��ġ�� ���
    public Vector3 GetEndPosition(Building building)
    {
        return GetEndPosition(building.currentX, building.currentY, building.columns, building.rows); // �ǹ��� �Ӽ��� ����Ͽ� �� ��ġ ��������
    }

    // �־��� ���� ��ǥ�� �׸��� ��� ���� �ִ��� Ȯ���ϴ� �޼���
    public bool IsWorldPositionIsOnPlane(Vector3 position, int x, int y, int rows, int columns)
    {
        position = transform.InverseTransformPoint(position); // ���� ��ǥ�踦 ���� ��ǥ��� ��ȯ
        Rect rect = new Rect(x, y, columns, rows); // x, y, �� �� ���� ���� ���簢�� ����
        return rect.Contains(new Vector2(position.x, position.z)); // �־��� ��ǥ�� ���簢�� �ȿ� ���ԵǾ� �ִ��� Ȯ��
    }

    // �ǹ��� Ư�� ��ġ�� ��ġ �������� Ȯ��
    public bool CanPlaceBuilding(Building building, int x, int y)
    {
        // �⺻������ �׸����� ������ ����� ��� false ��ȯ
        if (x < 0 || y < 0 || x + building.columns > _columns || y + building.rows > _rows)
        {
            return false; // �׸��� ������ ���
        }

        // ������ ��ġ�� �ǹ����� �浹 �˻�
        for (int i = 0; i < buildings.Count; i++)
        {
            if (buildings[i] != building) // ���� �ǹ��� �ƴ� ���
            {
                Rect rect1 = new Rect(buildings[i].currentX, buildings[i].currentY, buildings[i].columns, buildings[i].rows);   // ���� �ǹ��� ����
                Rect rect2 = new Rect(building.currentX, building.currentY, building.columns, building.rows);   // �� �ǹ��� ����
                if (rect2.Overlaps(rect1)) // ��ġ�� �κ��� �ִ��� Ȯ��
                {
                    return false; // ��ġ �Ұ�
                }
            }
        }

        return true; // �浹�� ������ true ��ȯ
    }

    // �ǹ��� �׸��忡 �߰�
    public void AddBuilding(Building building)
    {
        if (!buildings.Contains(building))
        {
            buildings.Add(building); // ��Ͽ� �ǹ��� �߰�
        }
    }

    // �ǹ��� �׸��忡�� ����
    public void RemoveBuilding(Building building)
    {
        if (buildings.Contains(building))
        {
            buildings.Remove(building); // ��Ͽ��� �ǹ� ����
        }
    }


#if UNITY_EDITOR
    // �����Ϳ��� �׸��带 �ð������� ǥ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white; // �׸��� ���� ���� ����
        for (int i = 0; i <= _rows; i++)
        {
            Vector3 point = transform.position + transform.forward.normalized * _cellSize * (float)i; // ���� �׸��� �� ���
            Gizmos.DrawLine(point, point + transform.right.normalized * _cellSize * (float)_columns); // ���� �� �׸���
        }
        for (int i = 0; i <= _columns; i++)
        {
            Vector3 point = transform.position + transform.right.normalized * _cellSize * (float)i; // ���� �׸��� �� ���
            Gizmos.DrawLine(point, point + transform.forward.normalized * _cellSize * (float)_rows); // ���� �� �׸���
        }
    }
#endif
}
