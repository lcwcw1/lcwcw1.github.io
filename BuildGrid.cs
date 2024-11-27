using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildGrid : MonoBehaviour
{
    private int _rows = 45; // 그리드의 행 수
    private int _columns = 45; // 그리드의 열 수
    private float _cellSize = 1f; // 그리드 셀의 크기
    public float cellSize { get { return _cellSize; } } // 셀 크기 프로퍼티

    public List<Building> buildings = new List<Building>(); // 현재 그리드에 배치된 건물 목록

    private static BuildGrid _instance = null; public static BuildGrid instance { get { return _instance; } } // 싱글턴 인스턴스

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
    }

    // 주어진 x, y 좌표에 대한 시작 위치를 계산
    public Vector3 GetStartPosition(int x, int y)
    {
        Vector3 position = transform.position; // 그리드의 현재 위치를 가져옴
        position += (transform.right.normalized * x * _cellSize) + (transform.forward.normalized * y * _cellSize); // x와 y에 따라 위치를 조정
        return position; // 계산된 위치를 반환
    }

    // 주어진 x, y 좌표 및 지정된 행과 열에 대한 중심 위치를 계산
    public Vector3 GetCenterPosition(int x, int y, int rows, int columns)
    {
        Vector3 position = GetStartPosition(x, y); // 시작 위치 가져오기
        position += (transform.right.normalized * columns * _cellSize / 2f) + (transform.forward.normalized * rows * _cellSize / 2f); // 중심으로 오프셋
        return position; // 중심 위치 반환
    }

    // 주어진 x, y 좌표 및 지정된 행과 열에 대한 끝 위치를 계산
    public Vector3 GetEndPosition(int x, int y, int rows, int columns)
    {
        Vector3 position = GetStartPosition(x, y); // 시작 위치 가져오기
        position += (transform.right.normalized * columns * _cellSize) + (transform.forward.normalized * rows * _cellSize); // 끝 위치로 이동
        return position; // 끝 위치 반환
    }

    // 특정 건물의 끝 위치를 계산
    public Vector3 GetEndPosition(Building building)
    {
        return GetEndPosition(building.currentX, building.currentY, building.columns, building.rows); // 건물의 속성을 사용하여 끝 위치 가져오기
    }

    // 주어진 월드 좌표가 그리드 평면 내에 있는지 확인하는 메서드
    public bool IsWorldPositionIsOnPlane(Vector3 position, int x, int y, int rows, int columns)
    {
        position = transform.InverseTransformPoint(position); // 월드 좌표계를 로컬 좌표계로 변환
        Rect rect = new Rect(x, y, columns, rows); // x, y, 행 및 열에 따라 직사각형 정의
        return rect.Contains(new Vector2(position.x, position.z)); // 주어진 좌표가 직사각형 안에 포함되어 있는지 확인
    }

    // 건물이 특정 위치에 배치 가능한지 확인
    public bool CanPlaceBuilding(Building building, int x, int y)
    {
        // 기본적으로 그리드의 범위를 벗어나는 경우 false 반환
        if (x < 0 || y < 0 || x + building.columns > _columns || y + building.rows > _rows)
        {
            return false; // 그리드 범위를 벗어남
        }

        // 기존에 설치된 건물과의 충돌 검사
        for (int i = 0; i < buildings.Count; i++)
        {
            if (buildings[i] != building) // 현재 건물이 아닌 경우
            {
                Rect rect1 = new Rect(buildings[i].currentX, buildings[i].currentY, buildings[i].columns, buildings[i].rows);   // 기존 건물의 영역
                Rect rect2 = new Rect(building.currentX, building.currentY, building.columns, building.rows);   // 새 건물의 영역
                if (rect2.Overlaps(rect1)) // 겹치는 부분이 있는지 확인
                {
                    return false; // 배치 불가
                }
            }
        }

        return true; // 충돌이 없으면 true 반환
    }

    // 건물을 그리드에 추가
    public void AddBuilding(Building building)
    {
        if (!buildings.Contains(building))
        {
            buildings.Add(building); // 목록에 건물을 추가
        }
    }

    // 건물을 그리드에서 제거
    public void RemoveBuilding(Building building)
    {
        if (buildings.Contains(building))
        {
            buildings.Remove(building); // 목록에서 건물 제거
        }
    }


#if UNITY_EDITOR
    // 에디터에서 그리드를 시각적으로 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white; // 그리드 선의 색상 설정
        for (int i = 0; i <= _rows; i++)
        {
            Vector3 point = transform.position + transform.forward.normalized * _cellSize * (float)i; // 수직 그리드 선 계산
            Gizmos.DrawLine(point, point + transform.right.normalized * _cellSize * (float)_columns); // 수직 선 그리기
        }
        for (int i = 0; i <= _columns; i++)
        {
            Vector3 point = transform.position + transform.right.normalized * _cellSize * (float)i; // 수평 그리드 선 계산
            Gizmos.DrawLine(point, point + transform.forward.normalized * _cellSize * (float)_rows); // 수평 선 그리기
        }
    }
#endif
}
