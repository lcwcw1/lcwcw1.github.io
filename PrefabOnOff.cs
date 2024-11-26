using System.Collections;
using UnityEngine;

public class TogglePrefabScript : MonoBehaviour
{
    private bool isToggling = false;  // 활성화/비활성화 여부
    public float toggleInterval = 2f; // 온/오프 간격 (초 단위)

    // 버튼을 누를 때 실행되는 함수
    public void StartToggle()
    {
        if (!isToggling)
        {
            isToggling = true;
            StartCoroutine(ToggleOnOff());
        }
    }

    // 일정 시간 간격으로 온오프 전환
    private IEnumerator ToggleOnOff()
    {
        while (isToggling)
        {
            gameObject.SetActive(!gameObject.activeSelf); // 활성화 상태 반전
            yield return new WaitForSeconds(toggleInterval);
        }
    }

    // 토글 멈추기
    public void StopToggle()
    {
        isToggling = false;
        StopCoroutine(ToggleOnOff());
        gameObject.SetActive(true); // 원래 상태로 설정 (필요 시 수정 가능)
    }

    // 스킬 버튼 클릭 시 호출되는 함수
    public void OnSkillButtonClicked()
    {
        StopToggle(); // 현재 활성화 상태를 멈춤
        StartCoroutine(StartToggleAfterDelay(3f)); // 3초 후 StartToggle 호출
    }

    // 지연 후 StartToggle 호출
    private IEnumerator StartToggleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartToggle();
    }
}
