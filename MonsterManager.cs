using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI를 위한 네임스페이스

public class MonsterManager : MonoBehaviour
{
    public GameObject Player; //몬스터가 플레이어가 있음을 인지
    public float maxHealth = 10000f;  // 최대 체력
    private float currentHealth;      // 현재 체력
    public Text healthText;           // 체력을 표시할 UI 텍스트
    private Animator animator;  // 애니메이터 추가
    public int baseFontSize = 150; // 기본 글자 크기
    public Text currencyText;
    private float lastUpdateTime;
    private int invokeCount=0;
    private bool isSceneActive = true; // 씬이 활성화된 상태를 추적하는 변수
    public CurrencyManager currencyManager; // CurrencyManager 스크립트 참조
    // 사운드 관련 변수
    public AudioClip deathSound; // 몬스터 죽을 때 재생할 사운드 클립
    private AudioSource audioSource; // AudioSource 컴포넌트



    void Start()
    {
        animator = GetComponent<Animator>();  // 애니메이터 참조
        Player = GameObject.FindGameObjectWithTag("player"); //몬스터가 태그로 플레이어가 있음을 인지
        currentHealth = maxHealth;
        //lastUpdateTime = Time.time; // 씬 시작 시간
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 추가

        UpdateHealthUI();
        // Animator가 없을 경우 에러 메시지 출력
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the Beholder object.");
        }
    }

    private void Update()
    {
        if (!isSceneActive)
        {
            float timeElapsed = Time.time - lastUpdateTime;
            float damagePerSecond = 10f; // 초당 체력 감소량
            TakeDamage(damagePerSecond * timeElapsed); // 지난 시간만큼 체력 감소
            lastUpdateTime = Time.time; // 마지막 업데이트 시간 갱신
        }
        
    }
    

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // 플레이어의 현재 공격력을 가져옴
            float currentAttackPower = Player.GetComponent<PlayerManager>().attackPower;
            // 플레이어 공격력 증가 (현재 공격력의 0.5배 추가)
            Player.GetComponent<PlayerManager>().IncreaseAttackPower(currentAttackPower * 0.25f);
            Die(); // 즉시 Die 메서드 호출
        }
        else {
            // 플레이어가 공격하면 hurt 애니메이션 재생
            animator.SetBool("isHurt", true);
            // 일정 시간 후에 다시 idle 상태로 돌아가도록 코루틴 사용
            StartCoroutine(ResetHurtAnimation());
            //ResetHurtAnimation();
        }
        UpdateHealthUI();  // 체력 UI 업데이트

        if (currentHealth <= 0)
        {
            Die();
        }  
    }

     private IEnumerator ResetHurtAnimation()
    {
        yield return new WaitForSeconds(0.3f);  // 지연 시간 조정
        animator.SetBool("isHurt", false);
        yield return new WaitForSeconds(0.8f);  // hurt 애니메이션의 길이에 맞게 조정
        animator.SetBool("isHurt", false);
    }

    void UpdateHealthUI()
    {
        healthText.text = $"{currentHealth:0,0} / {maxHealth:0,0}";  // 체력 표시 갱신
        AdjustFontSize(); // 글자 크기 조정
    }

    void Die() // 몬스터 사망 처리
    {
        Player.GetComponent<PlayerManager>().Monster = null; //PlayerCS에 접근해서 몬스터 인지를 NULL로 바꿈
        animator.SetTrigger("Die");  // 죽을 때 Die 애니메이션 재생
        Debug.Log("Monster Died");

        // 재화 추가
        int currencyAmount = 100 + (invokeCount * 50);
        CurrencyManager.Instance.AddCurrency(currencyAmount); // CurrencyManager를 통한 재화 추가
        
        if(currencyText != null) {
            currencyText.text = CurrencyManager.Instance.GetCurrency().ToString(); // 현재 재화 업데이트
        }
        else {
            Debug.LogError("CurrencyManager is not assigned in MonsterManager.");
        }
        PlayDeathSound(); // 죽을 때 사운드 재생
        invokeCount++;  // 호출 횟수 증가
        Invoke("Regen", 0.5f); //0.5초 후 부활
    }
    void Resurrection() {
        //Player.GetComponent<PlayerManager>().Monster = null; //PlayerCS에 접근해서 몬스터 인지를 NULL로 바꿈
        animator.SetTrigger("Resurrection");  // 죽을 때 Die 애니메이션 재생
        Debug.Log("Monster Resurrection");

    }
    void PlayDeathSound()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound); // 사운드 재생
        }
        else
        {
            Debug.LogError("Death sound is not assigned in MonsterManager.");
        }
    }
    public void Regen() { //몬스터 부활
        invokeCount++;
        maxHealth *= 1.1f;  // 최대 체력을 1.1배로 증가
        currentHealth = maxHealth;  // 현재 체력을 최대 체력의 1.1배로 설정
        healthText.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
        Player.GetComponent<PlayerManager>().Monster = this.gameObject;
        UpdateHealthUI();  // UI 업데이트
        Resurrection();
    }
    void AdjustFontSize()
    {
        // 최대 체력의 크기에 따라 글자 크기 조정
        float sizeFactor = maxHealth > 10000 ? (Mathf.Log10(maxHealth) - 4) : 0;
        healthText.fontSize = baseFontSize - (int)sizeFactor; // 기본 크기에서 감소
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // 앱이 백그라운드로 갈 때 씬 비활성화
            isSceneActive = false;
        }
        else
        {
            // 앱이 활성화되면 씬 활성화
            isSceneActive = true;
        }
    }
    // 씬의 포커스 상태가 변경될 때 호출되는 함수
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            // 포커스가 다시 활성화되면 씬 활성화
            isSceneActive = true;
        }
        else
        {
            // 포커스가 없어지면 씬 비활성화
            isSceneActive = false;
        }
    }
    
}
