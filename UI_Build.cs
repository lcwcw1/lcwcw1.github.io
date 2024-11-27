using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Build : MonoBehaviour // ���� UI�� ����
{
    [SerializeField] public GameObject _elements = null; // UI ���
    public RectTransform buttonConfirm = null; // Ȯ�� ��ư
    public RectTransform buttonCancel = null; // ��� ��ư
    [HideInInspector] public Button clickConfirmButton = null;  // Ȯ�� ��ư ������Ʈ

    private static UI_Build _instance = null; public static UI_Build instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    private void Awake()
    {
        _instance = this;   // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
        clickConfirmButton = buttonConfirm.gameObject.GetComponent<Button>();   // Ȯ�� ��ư ������Ʈ ��������
    }

    private void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ���
        buttonConfirm.gameObject.GetComponent<Button>().onClick.AddListener(Confirm);   // Ȯ�� ��ư Ŭ�� �� Confirm �޼��� ȣ��
        buttonCancel.gameObject.GetComponent<Button>().onClick.AddListener(Cancel);     // ��� ��ư Ŭ�� �� Cancel �޼��� ȣ��
        // ��ư ��ġ �ʱ�ȭ
        buttonConfirm.anchorMin = Vector3.zero;
        buttonConfirm.anchorMax = Vector3.zero;
        buttonCancel.anchorMin = Vector3.zero;
        buttonCancel.anchorMax = Vector3.zero;
    }

    private void Update()
    {
        // �ǹ� ��ġ ���� �� ��ư ��ġ ������Ʈ
        if (Building.buildInstance != null && CameraController.instance.isPlacingBuilding)
        {
            Vector3 end = UI_Main.instance._grid.GetEndPosition(Building.buildInstance); // �ǹ��� �� ��ġ

            // ȭ���� �׸��� ��ǥ ���
            Vector3 planBottomLeft = CameraController.instance.CameraScreenPositionToPlanePosition(Vector2.zero);
            Vector3 planTopRight = CameraController.instance.CameraScreenPositionToPlanePosition(new Vector2(Screen.width, Screen.height));

            float w = planTopRight.x - planBottomLeft.x; // �׸��� �ʺ�
            float h = planTopRight.z - planBottomLeft.z; // �׸��� ����

            float endW = end.x - planBottomLeft.x; // �� ��ġ�� ����� X ��ǥ
            float endH = end.z - planBottomLeft.z; // �� ��ġ�� ����� Z ��ǥ

            Vector2 screenPoint = new Vector2(endW / w * Screen.width, endH / h * Screen.height); // ȭ�� ������ ���� ��ǥ

            // Ȯ�� ��ư ��ġ ���
            Vector2 confirmPoint = screenPoint;
            confirmPoint.x -= (buttonConfirm.rect.width + 8f); // Ȯ�� ��ư ��ġ ����
            confirmPoint.y -= (buttonConfirm.rect.height + 100f);
            buttonConfirm.anchoredPosition = confirmPoint;  // Ȯ�� ��ư ��ġ ����

            // ��� ��ư ��ġ ���
            Vector2 cancelPoint = screenPoint;
            cancelPoint.x += (buttonCancel.rect.width + 8f); // ��� ��ư ��ġ ����
            cancelPoint.y -= (buttonConfirm.rect.height + 100f);
            buttonCancel.anchoredPosition = cancelPoint;    // ��� ��ư ��ġ ����
        }

        // ���õ� �ǹ��� ���ġ ���� �� ��ư ��ġ ������Ʈ
        if (Building.selectedInstance != null && CameraController.instance.isPlacingBuilding)
        {
            Vector3 end = UI_Main.instance._grid.GetEndPosition(Building.selectedInstance); // �ǹ��� �� ��ġ

            // ȭ���� �׸��� ��ǥ ���
            Vector3 planBottomLeft = CameraController.instance.CameraScreenPositionToPlanePosition(Vector2.zero);
            Vector3 planTopRight = CameraController.instance.CameraScreenPositionToPlanePosition(new Vector2(Screen.width, Screen.height));

            float w = planTopRight.x - planBottomLeft.x; // �׸��� �ʺ�
            float h = planTopRight.z - planBottomLeft.z; // �׸��� ����

            float endW = end.x - planBottomLeft.x; // �� ��ġ�� ����� X ��ǥ
            float endH = end.z - planBottomLeft.z; // �� ��ġ�� ����� Z ��ǥ

            Vector2 screenPoint = new Vector2(endW / w * Screen.width, endH / h * Screen.height); // ȭ�� ������ ���� ��ǥ ���

            // Ȯ�� ��ư ��ġ ���
            Vector2 confirmPoint = screenPoint;
            confirmPoint.x -= (buttonConfirm.rect.width + 8f); // Ȯ�� ��ư ��ġ ����
            confirmPoint.y -= (buttonConfirm.rect.height + 100f);
            buttonConfirm.anchoredPosition = confirmPoint;  // Ȯ�� ��ư ��ġ ����

            // ��� ��ư ��ġ ���
            Vector2 cancelPoint = screenPoint;
            cancelPoint.x += (buttonCancel.rect.width + 8f); // ��� ��ư ��ġ ����
            cancelPoint.y -= (buttonConfirm.rect.height + 100f);
            buttonCancel.anchoredPosition = cancelPoint;    // ��� ��ư ��ġ ����
        }
    }

    // UI ���� ���� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    // Ȯ�� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void Confirm()
    {
        if (Building.buildInstance != null)
        {
            // �׸��忡 �ǹ��� ��ġ�� �� �ִ��� Ȯ��
            if (UI_Main.instance._grid.CanPlaceBuilding(Building.buildInstance, Building.buildInstance.currentX, Building.buildInstance.currentY))
            {
                // �ǹ� ��ġ Ȯ��
                CameraController.instance.isPlacingBuilding = false;

                // �ǹ��� ��ġ�� ������Ű�� �߰� ������ ����
                Building.buildInstance.PlacedOnGrid(Building.buildInstance.currentX, Building.buildInstance.currentY);

                // �ٴ��� ���� ��Ÿ���� ������Ʈ ����
                Building.buildInstance.RemoveBaseArea();

                // ��� ���� ���� ���� ����
                Building.buildInstance._canIncreaseGold = true; // ��� ���� ����

                // UI ������Ʈ (��ġ �Ŀ��� UI_Build ��Ȱ��ȭ)
                SetStatus(false);

                // instance�� null�� �����Ͽ� �� �̻� �������� �ʵ��� ó��
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

                // instance�� null�� �����Ͽ� �� �̻� �������� �ʵ��� ó��
                UI_Building.instance = null;

                // �ǹ��� ��ġ�� ��� ��ġ �Ϸ� �޽��� ���
                Debug.Log("Building placed successfully.");
            }
            else
            {
                // �ǹ��� ��ġ�� �� ���� ��� ��� �޽��� ���
                Debug.Log("Cannot place building here!");
            }
        }
    }

    // ��� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void Cancel()
    {
        if (Building.buildInstance != null)
        {
            CameraController.instance.isPlacingBuilding = false; // �ǹ� ��ġ ��� ����
            Building.buildInstance.RemovedFromGrid(); // �׸��忡�� �ǹ� ����

            // instance�� null�� �����Ͽ� �� �̻� �������� �ʵ��� ó��
            Building.buildInstance = null;
            UI_Building.instance = null;

            // ���õ� �ǹ��� UI ���¸� �ʱ�ȭ
            if (Building.selectedInstance != null)
            {
                Building.selectedInstance.Deselected(); // ���� ����
            }

            // ��� �ǹ��� ���ġ ��ư Ȱ��ȭ
            UI_BuildingOptions.instance._replaceButton.interactable = true;
            UI_EnhancementBuildingOptions.instance._replaceButton.interactable = true;
            UI_RefineryBuildingOptions.instance._replaceButton.interactable = true;
            UI_BlacksmithBuildingOptions.instance._replaceButton.interactable = true;
            // UI_SkillBuildingOptions.instance._replaceButton.interactable = true;
        }
    }
}

