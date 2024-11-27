using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Build : MonoBehaviour // 빌드 UI에 연결
{
    [SerializeField] public GameObject _elements = null; // UI 요소
    public RectTransform buttonConfirm = null; // 확인 버튼
    public RectTransform buttonCancel = null; // 취소 버튼
    [HideInInspector] public Button clickConfirmButton = null;  // 확인 버튼 컴포넌트

    private static UI_Build _instance = null; public static UI_Build instance { get { return _instance; } } // 싱글턴 인스턴스

    private void Awake()
    {
        _instance = this;   // 싱글톤 인스턴스 초기화
        _elements.SetActive(false); // UI 비활성화
        clickConfirmButton = buttonConfirm.gameObject.GetComponent<Button>();   // 확인 버튼 컴포넌트 가져오기
    }

    private void Start()
    {
        // 버튼 클릭 이벤트 등록
        buttonConfirm.gameObject.GetComponent<Button>().onClick.AddListener(Confirm);   // 확인 버튼 클릭 시 Confirm 메서드 호출
        buttonCancel.gameObject.GetComponent<Button>().onClick.AddListener(Cancel);     // 취소 버튼 클릭 시 Cancel 메서드 호출
        // 버튼 위치 초기화
        buttonConfirm.anchorMin = Vector3.zero;
        buttonConfirm.anchorMax = Vector3.zero;
        buttonCancel.anchorMin = Vector3.zero;
        buttonCancel.anchorMax = Vector3.zero;
    }

    private void Update()
    {
        // 건물 설치 중일 때 버튼 위치 업데이트
        if (Building.buildInstance != null && CameraController.instance.isPlacingBuilding)
        {
            Vector3 end = UI_Main.instance._grid.GetEndPosition(Building.buildInstance); // 건물의 끝 위치

            // 화면의 그리드 좌표 계산
            Vector3 planBottomLeft = CameraController.instance.CameraScreenPositionToPlanePosition(Vector2.zero);
            Vector3 planTopRight = CameraController.instance.CameraScreenPositionToPlanePosition(new Vector2(Screen.width, Screen.height));

            float w = planTopRight.x - planBottomLeft.x; // 그리드 너비
            float h = planTopRight.z - planBottomLeft.z; // 그리드 높이

            float endW = end.x - planBottomLeft.x; // 끝 위치의 상대적 X 좌표
            float endH = end.z - planBottomLeft.z; // 끝 위치의 상대적 Z 좌표

            Vector2 screenPoint = new Vector2(endW / w * Screen.width, endH / h * Screen.height); // 화면 비율에 맞춘 좌표

            // 확인 버튼 위치 계산
            Vector2 confirmPoint = screenPoint;
            confirmPoint.x -= (buttonConfirm.rect.width + 8f); // 확인 버튼 위치 조정
            confirmPoint.y -= (buttonConfirm.rect.height + 100f);
            buttonConfirm.anchoredPosition = confirmPoint;  // 확인 버튼 위치 설정

            // 취소 버튼 위치 계산
            Vector2 cancelPoint = screenPoint;
            cancelPoint.x += (buttonCancel.rect.width + 8f); // 취소 버튼 위치 조정
            cancelPoint.y -= (buttonConfirm.rect.height + 100f);
            buttonCancel.anchoredPosition = cancelPoint;    // 취소 버튼 위치 설정
        }

        // 선택된 건물의 재배치 중일 때 버튼 위치 업데이트
        if (Building.selectedInstance != null && CameraController.instance.isPlacingBuilding)
        {
            Vector3 end = UI_Main.instance._grid.GetEndPosition(Building.selectedInstance); // 건물의 끝 위치

            // 화면의 그리드 좌표 계산
            Vector3 planBottomLeft = CameraController.instance.CameraScreenPositionToPlanePosition(Vector2.zero);
            Vector3 planTopRight = CameraController.instance.CameraScreenPositionToPlanePosition(new Vector2(Screen.width, Screen.height));

            float w = planTopRight.x - planBottomLeft.x; // 그리드 너비
            float h = planTopRight.z - planBottomLeft.z; // 그리드 높이

            float endW = end.x - planBottomLeft.x; // 끝 위치의 상대적 X 좌표
            float endH = end.z - planBottomLeft.z; // 끝 위치의 상대적 Z 좌표

            Vector2 screenPoint = new Vector2(endW / w * Screen.width, endH / h * Screen.height); // 화면 비율에 맞춘 좌표 계산

            // 확인 버튼 위치 계산
            Vector2 confirmPoint = screenPoint;
            confirmPoint.x -= (buttonConfirm.rect.width + 8f); // 확인 버튼 위치 조정
            confirmPoint.y -= (buttonConfirm.rect.height + 100f);
            buttonConfirm.anchoredPosition = confirmPoint;  // 확인 버튼 위치 설정

            // 취소 버튼 위치 계산
            Vector2 cancelPoint = screenPoint;
            cancelPoint.x += (buttonCancel.rect.width + 8f); // 취소 버튼 위치 조정
            cancelPoint.y -= (buttonConfirm.rect.height + 100f);
            buttonCancel.anchoredPosition = cancelPoint;    // 취소 버튼 위치 설정
        }
    }

    // UI 상태 설정 메서드
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // 확인 버튼 클릭 시 호출되는 메서드
    private void Confirm()
    {
        if (Building.buildInstance != null)
        {
            // 그리드에 건물을 배치할 수 있는지 확인
            if (UI_Main.instance._grid.CanPlaceBuilding(Building.buildInstance, Building.buildInstance.currentX, Building.buildInstance.currentY))
            {
                // 건물 배치 확정
                CameraController.instance.isPlacingBuilding = false;

                // 건물의 위치를 고정시키고 추가 동작을 중지
                Building.buildInstance.PlacedOnGrid(Building.buildInstance.currentX, Building.buildInstance.currentY);

                // 바닥의 색을 나타내는 오브젝트 제거
                Building.buildInstance.RemoveBaseArea();

                // 골드 증가 가능 여부 설정
                Building.buildInstance._canIncreaseGold = true; // 골드 증가 시작

                // UI 업데이트 (설치 후에는 UI_Build 비활성화)
                SetStatus(false);

                // instance를 null로 해제하여 더 이상 참조되지 않도록 처리
                Building.buildInstance = null;

                if (UI_Building.instance.isGold)
                {
                    UI_Main.instance._gold -= UI_Building.instance.currency;
                    UI_Main.instance._goldText.text = UI_Main.instance._gold.ToString();
                }
                if (UI_Building.instance.isEssence)
                {
                    UI_Main.instance._essence -= UI_Building.instance.currency;
                    UI_Main.instance._essenceText.text = UI_Main.instance._essence.ToString();
                }
                if (UI_Building.instance.isRefinedEssence)
                {
                    UI_Main.instance._refinedEssence -= UI_Building.instance.currency;
                    UI_Main.instance._refinedEssenceText.text = UI_Main.instance._refinedEssence.ToString();
                }

                UI_Building.instance.amount += 1;
                UI_Building.instance._limitsText.text = UI_Building.instance.amount + " / " + UI_Building.instance.limits;
                if (UI_Building.instance.limits == UI_Building.instance.amount)
                {
                    UI_Building.instance._button.interactable = false;
                }
                UI_Building.instance.SaveAmount();

                // instance를 null로 해제하여 더 이상 참조되지 않도록 처리
                UI_Building.instance = null;

                // 건물을 배치할 경우 배치 완료 메시지 출력
                Debug.Log("Building placed successfully.");
            }
            else
            {
                // 건물을 배치할 수 없는 경우 경고 메시지 출력
                Debug.Log("Cannot place building here!");
            }
        }
    }

    // 취소 버튼 클릭 시 호출되는 메서드
    public void Cancel()
    {
        if (Building.buildInstance != null)
        {
            CameraController.instance.isPlacingBuilding = false; // 건물 배치 모드 종료
            Building.buildInstance.RemovedFromGrid(); // 그리드에서 건물 제거

            // instance를 null로 해제하여 더 이상 참조되지 않도록 처리
            Building.buildInstance = null;
            UI_Building.instance = null;

            // 선택된 건물의 UI 상태를 초기화
            if (Building.selectedInstance != null)
            {
                Building.selectedInstance.Deselected(); // 선택 해제
            }

            // 모든 건물의 재배치 버튼 활성화
            UI_BuildingOptions.instance._replaceButton.interactable = true;
            UI_EnhancementBuildingOptions.instance._replaceButton.interactable = true;
            UI_RefineryBuildingOptions.instance._replaceButton.interactable = true;
            UI_BlacksmithBuildingOptions.instance._replaceButton.interactable = true;
            // UI_SkillBuildingOptions.instance._replaceButton.interactable = true;
        }
    }
}

