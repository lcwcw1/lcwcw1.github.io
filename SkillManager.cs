using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject subPlayerPrefab; // 서브플레이어 프리팹
    public Button skillButton; // 스킬 버튼
    public float skillDuration = 3f; // 서브플레이어가 공격하는 시간
    public float cooldownTime = 10f; // 스킬 쿨타임

    private bool isCooldown = false;

    void Start()
    {
        skillButton.onClick.AddListener(UseSkill);
    }

    void UseSkill()
    {
        if (isCooldown) return;

        StartCoroutine(SummonSubPlayer());
        StartCoroutine(StartCooldown());
    }

    IEnumerator SummonSubPlayer()
    {
        // 서브플레이어 생성 위치를 설정합니다. 
        // 예를 들어 플레이어의 위치에서 약간 앞쪽으로 이동
        Vector3 spawnPosition = transform.position + new Vector3(5, 0.75f, 0);

        GameObject subPlayer = Instantiate(subPlayerPrefab, spawnPosition, Quaternion.identity);
        subPlayer.SetActive(true);
        
        // 생성된 위치를 디버깅 로그로 출력
        Debug.Log("SubPlayer Spawn Position: " + spawnPosition);

        yield return new WaitForSeconds(skillDuration);

        Destroy(subPlayer);
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        skillButton.interactable = false;

        yield return new WaitForSeconds(cooldownTime);

        skillButton.interactable = true;
        isCooldown = false;
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject subPlayerPrefab; // 서브플레이어 프리팹
    public Button skillButton; // 스킬 버튼
    public float skillDuration = 3f; // 서브플레이어가 공격하는 시간
    public float cooldownTime = 10f; // 스킬 쿨타임

    private bool isCooldown = false;

    void Start()
    {
        skillButton.onClick.AddListener(UseSkill);
    }

    void UseSkill()
    {
        if (isCooldown) return;

        StartCoroutine(SummonSubPlayer());
        StartCoroutine(StartCooldown());
    }

    IEnumerator SummonSubPlayer()
    {
        GameObject subPlayer = Instantiate(subPlayerPrefab, transform.position, Quaternion.identity);
        subPlayer.SetActive(true);

        yield return new WaitForSeconds(skillDuration);

        Destroy(subPlayer);
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        skillButton.interactable = false;

        yield return new WaitForSeconds(cooldownTime);

        skillButton.interactable = true;
        isCooldown = false;
    }
}*/








