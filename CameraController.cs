using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    // 싱글톤 인스턴스 설정 (CameraController의 단일 인스턴스를 유지)
    private static CameraController _instance = null;
    public static CameraController instance { get { return _instance; } }

    // 카메라와 관련된 속성 설정
    [SerializeField] private Camera _camera = null;   // 카메라 오브젝트
    [SerializeField] private float _moveSpeed = 50;   // 카메라 이동 속도
    [SerializeField] private float _moveSmooth = 5;   // 카메라 이동 부드러움

    [SerializeField] private float _zoomSpeed = 5f;   // 카메라 줌 속도
    [SerializeField] private float _zoomSmooth = 5;   // 줌 부드러움

    private Controls _inputs = null;    // 사용자 입력 처리 클래스

    // 카메라 동작 관련 상태 변수
    private bool _zooming = false;      // 줌을 하고 있는지 여부
    private bool _moving = false;       // 이동 중인지 여부
    private Vector3 _center = Vector3.zero;   // 카메라의 중심점
    private float _right = 10, _left = 10, _up = 10, _down = 10; // 화면의 경계 설정
    private float _angle = 45;          // 카메라 각도
    private float _zoom = 5;            // 현재 줌 레벨
    private float _zoomMax = 10, _zoomMin = 1;  // 줌의 최소 및 최대 값
    private Vector2 _zoomPositionOnScreen = Vector2.zero;   // 스크린에서의 줌 위치
    private Vector3 _zoomPositionInWorld = Vector3.zero;    // 월드에서의 줌 위치
    private float _zoomBaseValue = 0;   // 기본 줌 값
    private float _zoomBaseDistance = 0;    // 기본 줌 거리

    private Transform _root = null, _pivot = null, _target = null;  // 카메라 이동 및 회전을 위한 트랜스폼

    // 건물 배치 관련 상태
    private bool _building = false; public bool isPlacingBuilding { get { return _building; } set { _building = value; } } // 건물 배치 유무
    private Vector3 _buildBasePosition = Vector3.zero;   // 건물 배치 시 기본 위치
    private bool _movingBuilding = false;    // 건물을 이동 중인지 여부

    private bool _replacing = false; public bool isReplacingBuilding { get { return _replacing; } set { _replacing = value; } } // 건물 재배치 유무
    private Vector3 _replaceBasePosition = Vector3.zero;   // 건물 재배치 시 기본 위치
    private bool _replacingBuilding = false;    // 건물을 재배치 중인지 여부

    // 초기화 및 입력 설정
    private void Awake()
    {
        _instance = this;               // 인스턴스 설정
        _inputs = new Controls();       // 새로운 입력 객체 생성
        _root = new GameObject("CameraHelper").transform;    // 카메라 움직임 조정을 위한 루트 생성
        _pivot = new GameObject("CameraPivot").transform;    // 카메라 피벗 생성
        _target = new GameObject("CameraTarget").transform;  // 카메라 타깃 생성
        _camera.orthographic = true;    // 카메라를 직교 투영 모드로 설정
        _camera.nearClipPlane = 0;      // 가까운 클립 플레인 설정
    }

    // 카메라 초기 설정
    private void Start()
    {
        Initialize(Vector3.zero, 50, 50, 40, 40, 45, 10, 5, 25);  // 기본값으로 초기화
    }

    // 카메라 설정 초기화 메서드
    public void Initialize(Vector3 center, float right, float left, float up, float down, float angle, float zoom, float zoomMin, float zoomMax)
    {
        _center = center;
        _right = right;
        _left = left;
        _up = up;
        _down = down;
        _angle = angle;
        _zoom = zoom;
        _zoomMin = zoomMin;
        _zoomMax = zoomMax;

        _camera.orthographicSize = _zoom;   // 카메라의 orthographicSize 설정

        _zooming = false;
        _moving = false;

        // 피벗과 타깃를 루트에 연결
        _pivot.SetParent(_root);    
        _target.SetParent(_pivot);

        _root.position = _center;   // 루트 위치 설정
        _root.localEulerAngles = Vector3.zero;  // 루트 회전 초기화

        _pivot.localPosition = Vector3.zero;    // 피벗 위치 초기화
        _pivot.localEulerAngles = new Vector3(_angle, 0, 0);    // 피벗 회전 설정

        _target.localPosition = new Vector3(0, 0, -100);    // 타깃 위치 설정
        _target.localEulerAngles = Vector3.zero;    // 타깃 회전 초기화
    }

    private void OnEnable()
    {
        _inputs.Enable();   // 입력 활성화
        _inputs.Main.Move.started += _ => MoveStarted();    // 이동 시작
        _inputs.Main.Move.canceled += _ => MoveCanceled();  // 이동 취소
        _inputs.Main.TouchZoom.started += _ => ZoomStarted();   // 줌 시작
        _inputs.Main.TouchZoom.canceled += _ => ZoomCanceled(); // 줌 취소
        _inputs.Main.PointerClick.performed += _ => ScreenClicked();    // 화면 클릭 처리
    }

    private void OnDisable()
    {
        _inputs.Main.Move.started -= _ => MoveStarted();    // 이동 시작 이벤트 제거
        _inputs.Main.Move.canceled -= _ => MoveCanceled();  // 이동 취소 이벤트 제거
        _inputs.Main.TouchZoom.started -= _ => ZoomStarted();   // 줌 시작 이벤트 제거
        _inputs.Main.TouchZoom.canceled -= _ => ZoomCanceled(); // 줌 취소 이벤트 제거
        _inputs.Main.PointerClick.performed -= _ => ScreenClicked();    // 화면 클릭 처리 이벤트 제거
        _inputs.Disable();  // 입력 비활성화
    }

    // 화면 클릭 처리 메서드
    private void ScreenClicked()
    {
        // 현재 건물을 배치 중이면 클릭 처리 중단
        if (CameraController.instance.isPlacingBuilding)
        {
            return;
        }
        Vector2 position = _inputs.Main.PointerPosition.ReadValue<Vector2>();   // 클릭 위치 가져오기
        if (IsScreenPointOverUI(position) == false)     // 화면 상에서 UI가 아닌 곳이 클릭되었을 때
        {
            bool found = false; // 건물 발견 여부 초기화
            Vector3 planePosition = CameraScreenPositionToPlanePosition(position);  // 스크린 좌표를 월드 평면 좌표로 변환
            // 현재 배치된 건물 목록을 순회하며 클릭된 위치에 건물이 있는지 확인
            for (int i = 0; i < UI_Main.instance._grid.buildings.Count; i++)
            {
                // 클릭된 위치가 해당 건물의 영역에 포함되는지 검사
                if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(planePosition, UI_Main.instance._grid.buildings[i].currentX, 
                    UI_Main.instance._grid.buildings[i].currentY, UI_Main.instance._grid.buildings[i].rows, UI_Main.instance._grid.buildings[i].columns))
                {
                    found = true;   // 건물 발견
                    UI_Main.instance._grid.buildings[i].Selected(); // 건물 선택

                    // 모든 건물 재배치 상태 초기화
                    UI_BuildingOptions.instance._isReplacing = false;
                    UI_EnhancementBuildingOptions.instance._isReplacing = false;
                    UI_RefineryBuildingOptions.instance._isReplacing = false;
                    UI_BlacksmithBuildingOptions.instance._isReplacing = false;

                    Building.selectedInstance._baseArea.gameObject.SetActive(false);    // 선택된 건물의 배치 혹은 재배치 가능 여부 판단 바닥 비활성화
                    break; // 루프 종료
                }
            }
            // 클릭된 위치에 건물이 없을 경우
            if (!found && Building.selectedInstance != null)
            {
                // 모든 건물 재배치 상태 초기화
                UI_BuildingOptions.instance._isReplacing = false;
                UI_EnhancementBuildingOptions.instance._isReplacing = false;
                UI_RefineryBuildingOptions.instance._isReplacing = false;
                UI_BlacksmithBuildingOptions.instance._isReplacing = false;

                Building.selectedInstance._baseArea.gameObject.SetActive(false);    // 선택된 건물의 배치 혹은 재배치 가능 여부 판단 바닥 비활성화
                Building.selectedInstance.Deselected(); // 선택 해제
            }
        }
    }

    // 화면 좌표가 UI 위에 있는지 검사하는 메서드
    public bool IsScreenPointOverUI(Vector2 position)
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);  // UI 충돌 검사
        return results.Count > 0;   // UI 위에 있으면 true 반환
    }

    // 이동 시작 시 호출되는 메서드
    private void MoveStarted()
    {
        if (UI_Main.instance.isActive) // UI가 활성화되어 있을 경우만 이동 시작
        {
            // 건물 배치 중일 경우
            if (_building)
            {
                // 현재 터치 위치를 평면상의 좌표로 변환하여 저장
                _buildBasePosition = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());
                // 변환된 평면 좌표에 건물이 있는지 확인
                if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(_buildBasePosition, Building.buildInstance.currentX, Building.buildInstance.currentY, Building.buildInstance.rows, Building.buildInstance.columns))
                {
                    Building.buildInstance.StartMovingOnGrid(); // 선택된 건물을 그리드 위에서 이동 시작
                    _movingBuilding = true;  // 건물을 움직이는 상태로 설정
                }
            }

            // 선택된 기본 건물이 있고 재배치 옵션이 활성화된 경우
            if (Building.selectedInstance != null && UI_BuildingOptions.instance._isReplacing) 
            {
                // 현재 터치 위치를 평면상의 좌표로 변환하여 저장
                _replaceBasePosition = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());
                // 변환된 평면 좌표에 선택된 건물이 있는지 확인
                if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(_replaceBasePosition, Building.selectedInstance.currentX, Building.selectedInstance.currentY, Building.selectedInstance.rows, Building.selectedInstance.columns))
                {
                    // 현재 재배치 상태가 아닐 경우
                    if (!_replacing)
                    {
                        _replacing = true;  // 재배치 상태로 변경
                        Building.selectedInstance._baseArea.gameObject.SetActive(true); // 선택된 건물의 배치 혹은 재배치 여부 판단 바닥 활성화
                    }
                    Building.selectedInstance.StartMovingOnGrid();  // 선택된 건물을 그리드 위에서 이동 시작
                    _replacingBuilding = true;  // 건물을 재배치하는 상태로 설정
                }
            }

            // 다른 건물 옵션들에 대해서도 동일한 방식으로 처리
            // 강화소, 대장간, 정제소 스킬상점에서 재배치 상태 확인 후 처리
            if (Building.selectedInstance != null && UI_EnhancementBuildingOptions.instance._isReplacing)
            {
                _replaceBasePosition = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());
                if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(_replaceBasePosition, Building.selectedInstance.currentX, Building.selectedInstance.currentY, Building.selectedInstance.rows, Building.selectedInstance.columns))
                {
                    if (!_replacing)
                    {
                        _replacing = true;
                        Building.selectedInstance._baseArea.gameObject.SetActive(true);
                    }
                    Building.selectedInstance.StartMovingOnGrid();
                    _replacingBuilding = true; 
                }
            }
            if (Building.selectedInstance != null && UI_RefineryBuildingOptions.instance._isReplacing)
            {
                _replaceBasePosition = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());
                if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(_replaceBasePosition, Building.selectedInstance.currentX, Building.selectedInstance.currentY, Building.selectedInstance.rows, Building.selectedInstance.columns))
                {
                    if (!_replacing)
                    {
                        _replacing = true;
                        Building.selectedInstance._baseArea.gameObject.SetActive(true);
                    }
                    Building.selectedInstance.StartMovingOnGrid();
                    _replacingBuilding = true;
                }
            }
            if (Building.selectedInstance != null && UI_BlacksmithBuildingOptions.instance._isReplacing)
            {
                _replaceBasePosition = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());
                if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(_replaceBasePosition, Building.selectedInstance.currentX, Building.selectedInstance.currentY, Building.selectedInstance.rows, Building.selectedInstance.columns))
                {
                    if (!_replacing)
                    {
                        _replacing = true;
                        Building.selectedInstance._baseArea.gameObject.SetActive(true);
                    }
                    Building.selectedInstance.StartMovingOnGrid();
                    _replacingBuilding = true; 
                }
            }
            //if (Building.selectedInstance != null && UI_SkillBuildingOptions.instance._isReplacing)
            //{
            //    _replaceBasePosition = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());
            //    if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(_replaceBasePosition, Building.selectedInstance.currentX, Building.selectedInstance.currentY, Building.selectedInstance.rows, Building.selectedInstance.columns))
            //    {
            //        if (!_replacing)
            //        {
            //            _replacing = true;
            //            Building.selectedInstance._baseArea.gameObject.SetActive(true);
            //        }
            //        Building.selectedInstance.StartMovingOnGrid();
            //        _replacingBuilding = true;
            //    }
            //}

            // 건물 이동 또는 재배치 중이 아닐 경우 카메라 이동 가능 상태로 설정
            if (_movingBuilding == false && _replacingBuilding == false)
            {
                _moving = true;
            }
        }
    }

    // 이동 취소 시 호출되는 메서드
    private void MoveCanceled()
    {
        _moving = false;    // 카메라 이동 상태 해제
        _movingBuilding = false;    // 건물 이동 상태 해제
        _replacingBuilding = false; // 건물 재배치 상태 해제
    }

    // 줌 시작 시 호출되는 메서드
    private void ZoomStarted()
    {
        if (UI_Main.instance.isActive)  // UI가 활성화되어 있을 경우만 줌 시작
        {
            Vector2 touch0 = _inputs.Main.TouchPosition0.ReadValue<Vector2>();  // 첫 번째 터치 위치
            Vector2 touch1 = _inputs.Main.TouchPosition1.ReadValue<Vector2>();  // 두 번째 터치 위치
            _zoomPositionOnScreen = Vector2.Lerp(touch0, touch1, 0.5f);  // 터치 사이의 중간점 계산
            _zoomPositionInWorld = CameraScreenPositionToPlanePosition(_zoomPositionOnScreen);  // 줌 위치 계산
            _zoomBaseValue = _zoom; // 현재 줌 값 저장

            // 화면 비율에 맞춰 터치 위치 보정
            touch0.x /= Screen.width;
            touch1.x /= Screen.width;
            touch0.y /= Screen.height;
            touch1.y /= Screen.height;

            _zoomBaseDistance = Vector2.Distance(touch0, touch1);  // 터치 간의 거리
            _zooming = true;    // 줌 상태 활성화
        }
    }

    // 줌 취소 시 호출되는 메서드
    private void ZoomCanceled()
    {
        _zooming = false;   // 줌 상태 비활성화
    }

    private void Update()
    {
        // 마우스 휠로 줌 조정
        if (Input.touchSupported == false)  // 터치가 지원되지 않는 경우
        {
            float mouseScroll = _inputs.Main.MouseScroll.ReadValue<float>();    // 마우스 스크롤 입력 받기
            if (mouseScroll > 0)    // 스크롤 업
            {
                _zoom -= 50f * Time.deltaTime;  // 줌 축소
            }
            else if (mouseScroll < 0)   // 스크롤 다운
            {
                _zoom += 50f * Time.deltaTime;  // 줌 확대
            }
        }

        // 줌 상태일 경우 처리
        if (_zooming)
        {
            Vector2 touch0 = _inputs.Main.TouchPosition0.ReadValue<Vector2>();  // 첫 번째 터치 위치
            Vector2 touch1 = _inputs.Main.TouchPosition1.ReadValue<Vector2>();  // 두 번째 터치 위치

            // 화면 비율에 맞춰 터치 위치 보정
            touch0.x /= Screen.width;
            touch1.x /= Screen.width;
            touch0.y /= Screen.height;
            touch1.y /= Screen.height;

            float currentDistance = Vector2.Distance(touch0, touch1);  // 현재 터치 간 거리 계산
            float deltaDistance = currentDistance - _zoomBaseDistance;  // 터치 변화량
            _zoom = _zoomBaseValue - (deltaDistance * _zoomSpeed);  // 줌 업데이트

            Vector3 zoomCenter = CameraScreenPositionToPlanePosition(_zoomPositionOnScreen);  // 줌 중심점 계산
            _root.position += (_zoomPositionInWorld - zoomCenter);  // 카메라 위치 업데이트
        }
        // 이동 상태일 경우 처리
        else if (_moving)
        {
            Vector2 move = _inputs.Main.MoveDelta.ReadValue<Vector2>(); // 이동 입력 받기
            if (move != Vector2.zero)   // 이동 입력이 있을 경우
            {
                // 화면 비율에 맞춰 이동 계산
                move.x /= Screen.width;
                move.y /= Screen.height;
                _root.position -= _root.right.normalized * move.x * _moveSpeed; // 오른쪽 방향으로 이동
                _root.position -= _root.forward.normalized * move.y * _moveSpeed;   // 앞쪽 방향으로 이동
            }
        }

        // 카메라의 경계를 조정
        AdjustBounds();

        // 카메라의 orthographicSize를 줌 값에 맞춰 조정
        if (_camera.orthographicSize != _zoom)
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoom, _zoomSmooth * Time.deltaTime);   // 부드럽게 줌 조정
        }

        // 카메라 위치와 회전을 타깃 위치로 이동
        if (_camera.transform.position != _target.position)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _target.position, _moveSmooth * Time.deltaTime);  // 위치 보간
        }
        if (_camera.transform.rotation != _target.rotation)
        {
            _camera.transform.rotation = _target.rotation;  // 회전 보간
        }

        // 건물 배치 중일 경우 그리드 상의 위치를 업데이트
        if (_building && _movingBuilding)
        {
            Vector3 pos = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());   // 현재 위치 계산
            Building.buildInstance.UpdateGridPosition(_buildBasePosition, pos); // 그리드 위치 업데이트
        }

        // 건물 재배치 중일 경우 그리드 상의 위치를 업데이트
        if (_replacing && _replacingBuilding)
        {
            Vector3 pos = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());   // 현재 위치 계산
            Building.selectedInstance.UpdateGridPosition(_replaceBasePosition, pos);    // 재배치 위치 업데이트
        }
    }

    // 카메라 경계를 조정하는 메서드
    private void AdjustBounds()
    {
        // 줌 값이 최소값보다 작을 경우 조정
        if (_zoom < _zoomMin)
        {
            _zoom = _zoomMin;
        }
        // 줌 값이 최대값보다 클 경우 조정
        if (_zoom > _zoomMax)
        {
            _zoom = _zoomMax;
        }

        float h = PlaneOrtographicSize();  // 화면 세로 크기 계산
        float w = h * _camera.aspect;  // 화면 가로 크기 계산

        // 줌이 너무 클 경우 상하 경계를 조정
        if (h > (_up + _down) / 2f)
        {
            float n = (_up + _down) / 2f;
            _zoom = n * Mathf.Sin(_angle * Mathf.Deg2Rad);
        }

        // 줌이 너무 클 경우 좌우 경계를 조정
        if (w > (_right + _left) / 2f)
        {
            float n = (_right + _left) / 2f;
            _zoom = n * Mathf.Sin(_angle * Mathf.Deg2Rad) / _camera.aspect;
        }

        // 다시 화면 크기를 계산
        h = PlaneOrtographicSize();
        w = h * _camera.aspect;

        // 카메라 경계 설정 (오른쪽 위, 왼쪽 위, 오른쪽 아래, 왼쪽 아래)
        Vector3 tr = _root.position + _root.right.normalized * w + _root.forward.normalized * h;
        Vector3 tl = _root.position - _root.right.normalized * w + _root.forward.normalized * h;
        Vector3 br = _root.position + _root.right.normalized * w - _root.forward.normalized * h;
        Vector3 bl = _root.position - _root.right.normalized * w - _root.forward.normalized * h;

        // 경계 체크 및 위치 조정
        if (tr.x > _center.x + _right)  // 오른쪽 경계 초과
        {
            _root.position += Vector3.left * Mathf.Abs(tr.x - (_center.x + _right));    // 왼쪽으로 이동
        }
        if (tl.x < _center.x - _left)   // 왼쪽 경계 초과
        {
            _root.position += Vector3.right * Mathf.Abs((_center.x - _left) - tl.x);    // 오른쪽으로 이동
        }

        if (tr.z > _center.z + _up) // 위쪽 경계 초과
        {
            _root.position += Vector3.back * Mathf.Abs(tr.z - (_center.z + _up));   // 뒤로 이동
        }
        if (bl.z < _center.z - _down)   // 아래쪽 경계 초과
        {
            _root.position += Vector3.forward * Mathf.Abs((_center.z - _down) - bl.z);  // 뒤로 이동
        }
    }

    // 화면의 세로 크기 계산
    private float PlaneOrtographicSize()
    {
        float h = _zoom * 2f;   // 줌에 따른 세로 크기
        return h / Mathf.Sin(_angle * Mathf.Deg2Rad) / 2f;  // 각도에 따른 계산
    }

    // 스크린 좌표를 월드 좌표로 변환하는 메서드
    private Vector3 CameraScreenPositionToWorldPosition(Vector2 position)
    {
        float h = _camera.orthographicSize * 2f;    // 화면 세로 길이
        float w = _camera.aspect * h;               // 화면 가로 길이
        // 기준점 계산
        Vector3 ancher = _camera.transform.position - (_camera.transform.right.normalized * w / 2f) - (_camera.transform.up.normalized * h / 2f);
        // 월드 좌표로 변환
        return ancher + (_camera.transform.right.normalized * position.x / Screen.width * w) + (_camera.transform.up.normalized * position.y / Screen.height * h);
    }

    // 스크린 좌표를 평면상의 월드 좌표로 변환하는 메서드
    public Vector3 CameraScreenPositionToPlanePosition(Vector2 position)
    {
        Vector3 point = CameraScreenPositionToWorldPosition(position);  // 월드 좌표로 변환
        float h = point.y - _root.position.y;   // 현재 높이 계산
        float x = h / Mathf.Sin(_angle * Mathf.Deg2Rad);    // 각도에 따른 x 위치 계산
        return point + _camera.transform.forward.normalized * x;    // 최종 평면 좌표 반환
    }
}