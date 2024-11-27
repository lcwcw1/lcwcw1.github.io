using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    // �̱��� �ν��Ͻ� ���� (CameraController�� ���� �ν��Ͻ��� ����)
    private static CameraController _instance = null;
    public static CameraController instance { get { return _instance; } }

    // ī�޶�� ���õ� �Ӽ� ����
    [SerializeField] private Camera _camera = null;   // ī�޶� ������Ʈ
    [SerializeField] private float _moveSpeed = 50;   // ī�޶� �̵� �ӵ�
    [SerializeField] private float _moveSmooth = 5;   // ī�޶� �̵� �ε巯��

    [SerializeField] private float _zoomSpeed = 5f;   // ī�޶� �� �ӵ�
    [SerializeField] private float _zoomSmooth = 5;   // �� �ε巯��

    private Controls _inputs = null;    // ����� �Է� ó�� Ŭ����

    // ī�޶� ���� ���� ���� ����
    private bool _zooming = false;      // ���� �ϰ� �ִ��� ����
    private bool _moving = false;       // �̵� ������ ����
    private Vector3 _center = Vector3.zero;   // ī�޶��� �߽���
    private float _right = 10, _left = 10, _up = 10, _down = 10; // ȭ���� ��� ����
    private float _angle = 45;          // ī�޶� ����
    private float _zoom = 5;            // ���� �� ����
    private float _zoomMax = 10, _zoomMin = 1;  // ���� �ּ� �� �ִ� ��
    private Vector2 _zoomPositionOnScreen = Vector2.zero;   // ��ũ�������� �� ��ġ
    private Vector3 _zoomPositionInWorld = Vector3.zero;    // ���忡���� �� ��ġ
    private float _zoomBaseValue = 0;   // �⺻ �� ��
    private float _zoomBaseDistance = 0;    // �⺻ �� �Ÿ�

    private Transform _root = null, _pivot = null, _target = null;  // ī�޶� �̵� �� ȸ���� ���� Ʈ������

    // �ǹ� ��ġ ���� ����
    private bool _building = false; public bool isPlacingBuilding { get { return _building; } set { _building = value; } } // �ǹ� ��ġ ����
    private Vector3 _buildBasePosition = Vector3.zero;   // �ǹ� ��ġ �� �⺻ ��ġ
    private bool _movingBuilding = false;    // �ǹ��� �̵� ������ ����

    private bool _replacing = false; public bool isReplacingBuilding { get { return _replacing; } set { _replacing = value; } } // �ǹ� ���ġ ����
    private Vector3 _replaceBasePosition = Vector3.zero;   // �ǹ� ���ġ �� �⺻ ��ġ
    private bool _replacingBuilding = false;    // �ǹ��� ���ġ ������ ����

    // �ʱ�ȭ �� �Է� ����
    private void Awake()
    {
        _instance = this;               // �ν��Ͻ� ����
        _inputs = new Controls();       // ���ο� �Է� ��ü ����
        _root = new GameObject("CameraHelper").transform;    // ī�޶� ������ ������ ���� ��Ʈ ����
        _pivot = new GameObject("CameraPivot").transform;    // ī�޶� �ǹ� ����
        _target = new GameObject("CameraTarget").transform;  // ī�޶� Ÿ�� ����
        _camera.orthographic = true;    // ī�޶� ���� ���� ���� ����
        _camera.nearClipPlane = 0;      // ����� Ŭ�� �÷��� ����
    }

    // ī�޶� �ʱ� ����
    private void Start()
    {
        Initialize(Vector3.zero, 50, 50, 40, 40, 45, 10, 5, 25);  // �⺻������ �ʱ�ȭ
    }

    // ī�޶� ���� �ʱ�ȭ �޼���
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

        _camera.orthographicSize = _zoom;   // ī�޶��� orthographicSize ����

        _zooming = false;
        _moving = false;

        // �ǹ��� Ÿ�긦 ��Ʈ�� ����
        _pivot.SetParent(_root);    
        _target.SetParent(_pivot);

        _root.position = _center;   // ��Ʈ ��ġ ����
        _root.localEulerAngles = Vector3.zero;  // ��Ʈ ȸ�� �ʱ�ȭ

        _pivot.localPosition = Vector3.zero;    // �ǹ� ��ġ �ʱ�ȭ
        _pivot.localEulerAngles = new Vector3(_angle, 0, 0);    // �ǹ� ȸ�� ����

        _target.localPosition = new Vector3(0, 0, -100);    // Ÿ�� ��ġ ����
        _target.localEulerAngles = Vector3.zero;    // Ÿ�� ȸ�� �ʱ�ȭ
    }

    private void OnEnable()
    {
        _inputs.Enable();   // �Է� Ȱ��ȭ
        _inputs.Main.Move.started += _ => MoveStarted();    // �̵� ����
        _inputs.Main.Move.canceled += _ => MoveCanceled();  // �̵� ���
        _inputs.Main.TouchZoom.started += _ => ZoomStarted();   // �� ����
        _inputs.Main.TouchZoom.canceled += _ => ZoomCanceled(); // �� ���
        _inputs.Main.PointerClick.performed += _ => ScreenClicked();    // ȭ�� Ŭ�� ó��
    }

    private void OnDisable()
    {
        _inputs.Main.Move.started -= _ => MoveStarted();    // �̵� ���� �̺�Ʈ ����
        _inputs.Main.Move.canceled -= _ => MoveCanceled();  // �̵� ��� �̺�Ʈ ����
        _inputs.Main.TouchZoom.started -= _ => ZoomStarted();   // �� ���� �̺�Ʈ ����
        _inputs.Main.TouchZoom.canceled -= _ => ZoomCanceled(); // �� ��� �̺�Ʈ ����
        _inputs.Main.PointerClick.performed -= _ => ScreenClicked();    // ȭ�� Ŭ�� ó�� �̺�Ʈ ����
        _inputs.Disable();  // �Է� ��Ȱ��ȭ
    }

    // ȭ�� Ŭ�� ó�� �޼���
    private void ScreenClicked()
    {
        // ���� �ǹ��� ��ġ ���̸� Ŭ�� ó�� �ߴ�
        if (CameraController.instance.isPlacingBuilding)
        {
            return;
        }
        Vector2 position = _inputs.Main.PointerPosition.ReadValue<Vector2>();   // Ŭ�� ��ġ ��������
        if (IsScreenPointOverUI(position) == false)     // ȭ�� �󿡼� UI�� �ƴ� ���� Ŭ���Ǿ��� ��
        {
            bool found = false; // �ǹ� �߰� ���� �ʱ�ȭ
            Vector3 planePosition = CameraScreenPositionToPlanePosition(position);  // ��ũ�� ��ǥ�� ���� ��� ��ǥ�� ��ȯ
            // ���� ��ġ�� �ǹ� ����� ��ȸ�ϸ� Ŭ���� ��ġ�� �ǹ��� �ִ��� Ȯ��
            for (int i = 0; i < UI_Main.instance._grid.buildings.Count; i++)
            {
                // Ŭ���� ��ġ�� �ش� �ǹ��� ������ ���ԵǴ��� �˻�
                if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(planePosition, UI_Main.instance._grid.buildings[i].currentX, 
                    UI_Main.instance._grid.buildings[i].currentY, UI_Main.instance._grid.buildings[i].rows, UI_Main.instance._grid.buildings[i].columns))
                {
                    found = true;   // �ǹ� �߰�
                    UI_Main.instance._grid.buildings[i].Selected(); // �ǹ� ����

                    // ��� �ǹ� ���ġ ���� �ʱ�ȭ
                    UI_BuildingOptions.instance._isReplacing = false;
                    UI_EnhancementBuildingOptions.instance._isReplacing = false;
                    UI_RefineryBuildingOptions.instance._isReplacing = false;
                    UI_BlacksmithBuildingOptions.instance._isReplacing = false;

                    Building.selectedInstance._baseArea.gameObject.SetActive(false);    // ���õ� �ǹ��� ��ġ Ȥ�� ���ġ ���� ���� �Ǵ� �ٴ� ��Ȱ��ȭ
                    break; // ���� ����
                }
            }
            // Ŭ���� ��ġ�� �ǹ��� ���� ���
            if (!found && Building.selectedInstance != null)
            {
                // ��� �ǹ� ���ġ ���� �ʱ�ȭ
                UI_BuildingOptions.instance._isReplacing = false;
                UI_EnhancementBuildingOptions.instance._isReplacing = false;
                UI_RefineryBuildingOptions.instance._isReplacing = false;
                UI_BlacksmithBuildingOptions.instance._isReplacing = false;

                Building.selectedInstance._baseArea.gameObject.SetActive(false);    // ���õ� �ǹ��� ��ġ Ȥ�� ���ġ ���� ���� �Ǵ� �ٴ� ��Ȱ��ȭ
                Building.selectedInstance.Deselected(); // ���� ����
            }
        }
    }

    // ȭ�� ��ǥ�� UI ���� �ִ��� �˻��ϴ� �޼���
    public bool IsScreenPointOverUI(Vector2 position)
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);  // UI �浹 �˻�
        return results.Count > 0;   // UI ���� ������ true ��ȯ
    }

    // �̵� ���� �� ȣ��Ǵ� �޼���
    private void MoveStarted()
    {
        if (UI_Main.instance.isActive) // UI�� Ȱ��ȭ�Ǿ� ���� ��츸 �̵� ����
        {
            // �ǹ� ��ġ ���� ���
            if (_building)
            {
                // ���� ��ġ ��ġ�� ������ ��ǥ�� ��ȯ�Ͽ� ����
                _buildBasePosition = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());
                // ��ȯ�� ��� ��ǥ�� �ǹ��� �ִ��� Ȯ��
                if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(_buildBasePosition, Building.buildInstance.currentX, Building.buildInstance.currentY, Building.buildInstance.rows, Building.buildInstance.columns))
                {
                    Building.buildInstance.StartMovingOnGrid(); // ���õ� �ǹ��� �׸��� ������ �̵� ����
                    _movingBuilding = true;  // �ǹ��� �����̴� ���·� ����
                }
            }

            // ���õ� �⺻ �ǹ��� �ְ� ���ġ �ɼ��� Ȱ��ȭ�� ���
            if (Building.selectedInstance != null && UI_BuildingOptions.instance._isReplacing) 
            {
                // ���� ��ġ ��ġ�� ������ ��ǥ�� ��ȯ�Ͽ� ����
                _replaceBasePosition = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());
                // ��ȯ�� ��� ��ǥ�� ���õ� �ǹ��� �ִ��� Ȯ��
                if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(_replaceBasePosition, Building.selectedInstance.currentX, Building.selectedInstance.currentY, Building.selectedInstance.rows, Building.selectedInstance.columns))
                {
                    // ���� ���ġ ���°� �ƴ� ���
                    if (!_replacing)
                    {
                        _replacing = true;  // ���ġ ���·� ����
                        Building.selectedInstance._baseArea.gameObject.SetActive(true); // ���õ� �ǹ��� ��ġ Ȥ�� ���ġ ���� �Ǵ� �ٴ� Ȱ��ȭ
                    }
                    Building.selectedInstance.StartMovingOnGrid();  // ���õ� �ǹ��� �׸��� ������ �̵� ����
                    _replacingBuilding = true;  // �ǹ��� ���ġ�ϴ� ���·� ����
                }
            }

            // �ٸ� �ǹ� �ɼǵ鿡 ���ؼ��� ������ ������� ó��
            // ��ȭ��, ���尣, ������ ��ų�������� ���ġ ���� Ȯ�� �� ó��
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

            // �ǹ� �̵� �Ǵ� ���ġ ���� �ƴ� ��� ī�޶� �̵� ���� ���·� ����
            if (_movingBuilding == false && _replacingBuilding == false)
            {
                _moving = true;
            }
        }
    }

    // �̵� ��� �� ȣ��Ǵ� �޼���
    private void MoveCanceled()
    {
        _moving = false;    // ī�޶� �̵� ���� ����
        _movingBuilding = false;    // �ǹ� �̵� ���� ����
        _replacingBuilding = false; // �ǹ� ���ġ ���� ����
    }

    // �� ���� �� ȣ��Ǵ� �޼���
    private void ZoomStarted()
    {
        if (UI_Main.instance.isActive)  // UI�� Ȱ��ȭ�Ǿ� ���� ��츸 �� ����
        {
            Vector2 touch0 = _inputs.Main.TouchPosition0.ReadValue<Vector2>();  // ù ��° ��ġ ��ġ
            Vector2 touch1 = _inputs.Main.TouchPosition1.ReadValue<Vector2>();  // �� ��° ��ġ ��ġ
            _zoomPositionOnScreen = Vector2.Lerp(touch0, touch1, 0.5f);  // ��ġ ������ �߰��� ���
            _zoomPositionInWorld = CameraScreenPositionToPlanePosition(_zoomPositionOnScreen);  // �� ��ġ ���
            _zoomBaseValue = _zoom; // ���� �� �� ����

            // ȭ�� ������ ���� ��ġ ��ġ ����
            touch0.x /= Screen.width;
            touch1.x /= Screen.width;
            touch0.y /= Screen.height;
            touch1.y /= Screen.height;

            _zoomBaseDistance = Vector2.Distance(touch0, touch1);  // ��ġ ���� �Ÿ�
            _zooming = true;    // �� ���� Ȱ��ȭ
        }
    }

    // �� ��� �� ȣ��Ǵ� �޼���
    private void ZoomCanceled()
    {
        _zooming = false;   // �� ���� ��Ȱ��ȭ
    }

    private void Update()
    {
        // ���콺 �ٷ� �� ����
        if (Input.touchSupported == false)  // ��ġ�� �������� �ʴ� ���
        {
            float mouseScroll = _inputs.Main.MouseScroll.ReadValue<float>();    // ���콺 ��ũ�� �Է� �ޱ�
            if (mouseScroll > 0)    // ��ũ�� ��
            {
                _zoom -= 50f * Time.deltaTime;  // �� ���
            }
            else if (mouseScroll < 0)   // ��ũ�� �ٿ�
            {
                _zoom += 50f * Time.deltaTime;  // �� Ȯ��
            }
        }

        // �� ������ ��� ó��
        if (_zooming)
        {
            Vector2 touch0 = _inputs.Main.TouchPosition0.ReadValue<Vector2>();  // ù ��° ��ġ ��ġ
            Vector2 touch1 = _inputs.Main.TouchPosition1.ReadValue<Vector2>();  // �� ��° ��ġ ��ġ

            // ȭ�� ������ ���� ��ġ ��ġ ����
            touch0.x /= Screen.width;
            touch1.x /= Screen.width;
            touch0.y /= Screen.height;
            touch1.y /= Screen.height;

            float currentDistance = Vector2.Distance(touch0, touch1);  // ���� ��ġ �� �Ÿ� ���
            float deltaDistance = currentDistance - _zoomBaseDistance;  // ��ġ ��ȭ��
            _zoom = _zoomBaseValue - (deltaDistance * _zoomSpeed);  // �� ������Ʈ

            Vector3 zoomCenter = CameraScreenPositionToPlanePosition(_zoomPositionOnScreen);  // �� �߽��� ���
            _root.position += (_zoomPositionInWorld - zoomCenter);  // ī�޶� ��ġ ������Ʈ
        }
        // �̵� ������ ��� ó��
        else if (_moving)
        {
            Vector2 move = _inputs.Main.MoveDelta.ReadValue<Vector2>(); // �̵� �Է� �ޱ�
            if (move != Vector2.zero)   // �̵� �Է��� ���� ���
            {
                // ȭ�� ������ ���� �̵� ���
                move.x /= Screen.width;
                move.y /= Screen.height;
                _root.position -= _root.right.normalized * move.x * _moveSpeed; // ������ �������� �̵�
                _root.position -= _root.forward.normalized * move.y * _moveSpeed;   // ���� �������� �̵�
            }
        }

        // ī�޶��� ��踦 ����
        AdjustBounds();

        // ī�޶��� orthographicSize�� �� ���� ���� ����
        if (_camera.orthographicSize != _zoom)
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoom, _zoomSmooth * Time.deltaTime);   // �ε巴�� �� ����
        }

        // ī�޶� ��ġ�� ȸ���� Ÿ�� ��ġ�� �̵�
        if (_camera.transform.position != _target.position)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _target.position, _moveSmooth * Time.deltaTime);  // ��ġ ����
        }
        if (_camera.transform.rotation != _target.rotation)
        {
            _camera.transform.rotation = _target.rotation;  // ȸ�� ����
        }

        // �ǹ� ��ġ ���� ��� �׸��� ���� ��ġ�� ������Ʈ
        if (_building && _movingBuilding)
        {
            Vector3 pos = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());   // ���� ��ġ ���
            Building.buildInstance.UpdateGridPosition(_buildBasePosition, pos); // �׸��� ��ġ ������Ʈ
        }

        // �ǹ� ���ġ ���� ��� �׸��� ���� ��ġ�� ������Ʈ
        if (_replacing && _replacingBuilding)
        {
            Vector3 pos = CameraScreenPositionToPlanePosition(_inputs.Main.PointerPosition.ReadValue<Vector2>());   // ���� ��ġ ���
            Building.selectedInstance.UpdateGridPosition(_replaceBasePosition, pos);    // ���ġ ��ġ ������Ʈ
        }
    }

    // ī�޶� ��踦 �����ϴ� �޼���
    private void AdjustBounds()
    {
        // �� ���� �ּҰ����� ���� ��� ����
        if (_zoom < _zoomMin)
        {
            _zoom = _zoomMin;
        }
        // �� ���� �ִ밪���� Ŭ ��� ����
        if (_zoom > _zoomMax)
        {
            _zoom = _zoomMax;
        }

        float h = PlaneOrtographicSize();  // ȭ�� ���� ũ�� ���
        float w = h * _camera.aspect;  // ȭ�� ���� ũ�� ���

        // ���� �ʹ� Ŭ ��� ���� ��踦 ����
        if (h > (_up + _down) / 2f)
        {
            float n = (_up + _down) / 2f;
            _zoom = n * Mathf.Sin(_angle * Mathf.Deg2Rad);
        }

        // ���� �ʹ� Ŭ ��� �¿� ��踦 ����
        if (w > (_right + _left) / 2f)
        {
            float n = (_right + _left) / 2f;
            _zoom = n * Mathf.Sin(_angle * Mathf.Deg2Rad) / _camera.aspect;
        }

        // �ٽ� ȭ�� ũ�⸦ ���
        h = PlaneOrtographicSize();
        w = h * _camera.aspect;

        // ī�޶� ��� ���� (������ ��, ���� ��, ������ �Ʒ�, ���� �Ʒ�)
        Vector3 tr = _root.position + _root.right.normalized * w + _root.forward.normalized * h;
        Vector3 tl = _root.position - _root.right.normalized * w + _root.forward.normalized * h;
        Vector3 br = _root.position + _root.right.normalized * w - _root.forward.normalized * h;
        Vector3 bl = _root.position - _root.right.normalized * w - _root.forward.normalized * h;

        // ��� üũ �� ��ġ ����
        if (tr.x > _center.x + _right)  // ������ ��� �ʰ�
        {
            _root.position += Vector3.left * Mathf.Abs(tr.x - (_center.x + _right));    // �������� �̵�
        }
        if (tl.x < _center.x - _left)   // ���� ��� �ʰ�
        {
            _root.position += Vector3.right * Mathf.Abs((_center.x - _left) - tl.x);    // ���������� �̵�
        }

        if (tr.z > _center.z + _up) // ���� ��� �ʰ�
        {
            _root.position += Vector3.back * Mathf.Abs(tr.z - (_center.z + _up));   // �ڷ� �̵�
        }
        if (bl.z < _center.z - _down)   // �Ʒ��� ��� �ʰ�
        {
            _root.position += Vector3.forward * Mathf.Abs((_center.z - _down) - bl.z);  // �ڷ� �̵�
        }
    }

    // ȭ���� ���� ũ�� ���
    private float PlaneOrtographicSize()
    {
        float h = _zoom * 2f;   // �ܿ� ���� ���� ũ��
        return h / Mathf.Sin(_angle * Mathf.Deg2Rad) / 2f;  // ������ ���� ���
    }

    // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ�ϴ� �޼���
    private Vector3 CameraScreenPositionToWorldPosition(Vector2 position)
    {
        float h = _camera.orthographicSize * 2f;    // ȭ�� ���� ����
        float w = _camera.aspect * h;               // ȭ�� ���� ����
        // ������ ���
        Vector3 ancher = _camera.transform.position - (_camera.transform.right.normalized * w / 2f) - (_camera.transform.up.normalized * h / 2f);
        // ���� ��ǥ�� ��ȯ
        return ancher + (_camera.transform.right.normalized * position.x / Screen.width * w) + (_camera.transform.up.normalized * position.y / Screen.height * h);
    }

    // ��ũ�� ��ǥ�� ������ ���� ��ǥ�� ��ȯ�ϴ� �޼���
    public Vector3 CameraScreenPositionToPlanePosition(Vector2 position)
    {
        Vector3 point = CameraScreenPositionToWorldPosition(position);  // ���� ��ǥ�� ��ȯ
        float h = point.y - _root.position.y;   // ���� ���� ���
        float x = h / Mathf.Sin(_angle * Mathf.Deg2Rad);    // ������ ���� x ��ġ ���
        return point + _camera.transform.forward.normalized * x;    // ���� ��� ��ǥ ��ȯ
    }
}