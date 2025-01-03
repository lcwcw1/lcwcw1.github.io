using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Refinery : MonoBehaviour
{
    [SerializeField] public GameObject _elements = null;    // UI 요소를 포함하는 게임 오브젝트
    [SerializeField] private Button _closeButton = null;    // 닫기 버튼
    [SerializeField] private Button _refineButton = null;    // 정제 버튼
    [SerializeField] public TextMeshProUGUI _refineIngredient = null;

    private static UI_Refinery _instance = null; public static UI_Refinery instance { get { return _instance; } } // 싱글턴 인스턴스

    public bool _status = false; // 건물 정보 UI 활성화 여부 

    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
        _elements.SetActive(false); // UI 비활성화
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);    // 건물 정보 닫기 버튼 클릭 시 Close 메서드 호출
        _refineButton.onClick.AddListener(refine);    // 정제 버튼 클릭 시 refine 메서드 호출
    }

    // UI 활성화 상태를 설정하는 메서드
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    private void refine()
    {
        if (UI_Main.instance._essence < Building.selectedInstance.CurrentLevel.Amount1)
        {
            UI_Caution.instance.SetRStatus(true);
            return;
        }
        UI_Main.instance._essence -= Building.selectedInstance.CurrentLevel.Amount1;
        UI_Main.instance._refinedEssence += 1;
        _refineIngredient.text = UI_Main.instance._essence + "/" + Building.selectedInstance.CurrentLevel.Amount1;
        UI_Main.instance._essenceText.text = UI_Main.instance._essence.ToString();
        UI_Main.instance._refinedEssenceText.text = UI_Main.instance._refinedEssence.ToString();
    }

    // 건물 정보 UI 닫기 메서드
    private void Close()
    {
        // 메인 UI에 있는 모든 버튼 활성화
        UI_Main.instance._shopButton.interactable = true;
        UI_Main.instance._mapButton.interactable = true;
        UI_Main.instance._playerInfoButton.interactable = true;
        UI_Main.instance._menuButton.interactable = true;

        _status = false;    // 건물 정보 UI 상태 비활성화
        UI_Main.instance.isActive = true;   // 메인 UI 활성화
        SetStatus(false); // 건물 정보 UI 비활성화
        Building.selectedInstance = null;
    }

}
