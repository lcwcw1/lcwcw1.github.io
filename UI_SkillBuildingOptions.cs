using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillBuildingOptions : MonoBehaviour // 스킬 건물의 옵션 UI에 연결 (현재 사용하지 않을 예정)
{
    [SerializeField] public GameObject _elements = null;    // UI 요소를 포함하는 게임 오브젝트
    [SerializeField] private Button _skillButton = null;    // 스킬 구매 버튼
    [SerializeField] private Button _upgradeButton = null;  // 건물 업그레이드 버튼
    [SerializeField] private Button _infoButton = null;     // 건물 정보 버튼
    [SerializeField] public Button _replaceButton = null;   // 건물 재배치 버튼
    private static UI_SkillBuildingOptions _instance = null; public static UI_SkillBuildingOptions instance { get { return _instance; } } // 싱글턴 인스턴스

    public bool _isReplacing = false;   // 재배치 여부

    // 게임 오브젝트가 처음 생성될 때 호출되는 메서드
    private void Awake()
    {
        _instance = this; // 싱글톤 인스턴스 초기화
        _elements.SetActive(false); // UI 비활성화
    }

    private void Start()
    {
        _skillButton.onClick.AddListener(skill);    // 스킬 버튼 클릭 시 skill 메서드 호출
        _upgradeButton.onClick.AddListener(upgrade);    // 건물 업그레이드 버튼 클릭 시 upgrade 메서드 호출
        _infoButton.onClick.AddListener(info);  // 건물 정보 버튼 클릭 시 info 메서드 호출
        _replaceButton.onClick.AddListener(replace);    // 건물 재배치 버튼 클릭 시 replace 메서드 호출
    }

    // UI 활성화 상태를 설정하는 메서드
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    private void skill()
    {
        // UI에 skill를 만들어 스킬 구매 UI가 나오게 한다.
    }

    // 건물 업그레이드 버튼 클릭 시 호출되는 메서드
    private void upgrade()
    {
        // UI에 upgrade를 만들어 해당 건물의 강화 UI가 나오게 한다.
    }

    // 건물 정보 버튼 클릭 시 호출되는 메서드
    private void info()
    {
        // UI에 info를 만들어 해당 건물의 정보가 나오게 한다.
    }

    // 건물 재배치 버튼 클릭 시 호출되는 메서드
    private void replace()
    {
        _isReplacing = !_isReplacing;   // 재배치 여부 토글
        if (!_isReplacing)
        {
            Building.selectedInstance._baseArea.gameObject.SetActive(false);    // 재배치 종료 시 재배치 가능 여부 판단 바닥 영역 비활성화
        }
        else
        {
            Building.selectedInstance._baseArea.gameObject.SetActive(true);     // 재배치 시작 시 재배치 가능 여부 판단 바닥 영역 활성화
        }

        // 추후 개선사항 : 재배치 버튼을 누를 시 UI가 꺼지면서 처음 건물을 건설했을 때처럼 확인 버튼이 나오게 하기

    }
}
