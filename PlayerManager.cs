using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public GameObject Monster; //캐릭터가 몬스터가 있음을 인지
    public CurrencyManager currencyManager; // CurrencyManager를 참조
    public MonsterManager monster;            // 공격할 몬스터 스크립트
    public float attackSpeed = 1f;  // 공격 간격
    public float attackPower = 50f;    // 공격력
    private float attackTimer = 0f;
    public string equippedWeapon; 
    private bool isDoubleSpeed = false; // 공격 속도가 두 배인지 여부
    private Animator animator;          // Animator 컴포넌트

    // AudioSource 컴포넌트와 사운드 클립
    private AudioSource audioSource; 
    public AudioClip attackSound;  // 공격 사운드 클립

    //public bool isUpgrading = false;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        ResetAttackPower();
        animator = GetComponent<Animator>();  // Animator 컴포넌트 참조
        Monster = GameObject.FindGameObjectWithTag("monster"); //플레이어가 태그로 몬스터가 있음을 감지
        audioSource = GetComponent<AudioSource>();  // AudioSource 컴포넌트 참조
        // 공격력 초기화: PlayerPrefs에서 공격력을 가져오고 없을 경우 기본값 사용
        attackPower = PlayerPrefs.GetFloat("PlayerAttackPower", 50f);

        if (currencyManager == null)
        {
            currencyManager = FindObjectOfType<CurrencyManager>();
        }

        if (audioSource == null)
        {
            Debug.LogError("Player 객체에 AudioSource 컴포넌트가 없습니다. 추가해주세요.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackSpeed)
        {
            Attack();
            attackTimer = 0f;
        }

        // 화면 터치로 공격 속도 증가
        /*if (Input.GetMouseButtonDown(0) && !isDoubleSpeed)
        {
            SetDoubleAttackSpeed(true);
        }

        if (isDoubleSpeed)
        {
            SetDoubleAttackSpeed(false);
        }*/
        if (Input.GetMouseButtonDown(0))
        {
        SetDoubleAttackSpeed(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
        SetDoubleAttackSpeed(false);
        }

        // 애니메이션 전환
        animator.SetBool("isAttacking", attackTimer < attackSpeed / 2);
        }
        
        /*// 화면 터치 감지하여 공격 속도 2배로 증가
        if (Input.GetMouseButtonDown(0))  // 모바일에서 터치도 마우스 클릭으로 인식 가능
        {
            //if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
                isDoubleSpeed = true;
                attackSpeed /= 2;  // 공격 속도 2배
                animator.speed = 2f;
            //}
        }

        if (Input.GetMouseButtonUp(0))  // 터치 해제 시 원래 속도로 돌아옴
        {
            isDoubleSpeed = false;
            attackSpeed *= 2;  // 공격 속도 원상 복구
            animator.speed = 1f;
        }

        // 공격 중 애니메이션 전환
        if (attackTimer < attackSpeed / 2)
        {
            animator.SetBool("isAttacking", true);  // 공격 애니메이션
        }
        else
        {
            animator.SetBool("isAttacking", false); // Idle 애니메이션
        }*/

    void Attack()
    {
        if (monster == null)
        {
            Debug.LogWarning("공격할 몬스터가 없습니다.");
            return; // 몬스터가 없을 경우 함수 종료
        }
        monster.TakeDamage(attackPower);
        PlayAttackSound();
}
    private void PlayAttackSound()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound); // 공격 사운드 재생
        }
    }
    public void IncreaseAttackPower(float amount)
    {
        attackPower += amount; // 공격력 증가
        currencyManager.AddCurrency(100); // 공격력 증가 시 50 재화 추가 (예시)
        PlayerPrefs.SetFloat("PlayerAttackPower", attackPower); // 공격력 영구 저장
        PlayerPrefs.Save(); // 즉시 저장
    }
    public void ResetAttackPower()
    {
        attackPower = 50f; // 기본값으로 초기화 (원하는 기본값으로 변경 가능)
        PlayerPrefs.SetFloat("PlayerAttackPower", attackPower); // PlayerPrefs에 저장
        PlayerPrefs.Save(); // 즉시 저장
    }

    private void SetDoubleAttackSpeed(bool isDouble)
    {
        if (isDoubleSpeed == isDouble) return;
        else attackSpeed = 1f;

        isDoubleSpeed = isDouble;
        attackSpeed = isDouble ? attackSpeed / 2 : attackSpeed * 2; // 공격 속도 조정
        animator.speed = isDouble ? 2f : 1f; // 애니메이터 속도 조정
    }
    public void ChangeWeapon(string newWeapon)
    {
        equippedWeapon = newWeapon;
        // 추가적인 무기 능력치 변경 로직...
        Debug.Log("현재 장착된 무기: " + equippedWeapon);
    }
    /*public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else if (clip == null)
        {
            Debug.LogWarning("AudioClip is null. Cannot play sound.");
        }
    }*/

    /*public void UpgradeWeapon()
    {
        isUpgrading = true; // 업그레이드 상태 설정
        // 업그레이드 로직 추가
        // 업그레이드가 끝난 후에는 isUpgrading을 false로 변경
        // 예: UpgradeWeapon에서 호출될 때는 Coroutine이나 다른 로직으로 일정 시간 후에 다시 false로 설정
    }*/

    /* 공격력을 기본값으로 초기화할 필요가 있을 때
    public void ResetAttackPower()
    {
        attackPower = 10f; // 기본값으로 초기화
        PlayerPrefs.SetFloat("PlayerAttackPower", attackPower); // PlayerPrefs에 저장
        PlayerPrefs.Save(); // 즉시 저장
    }*/
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Monster = GameObject.FindGameObjectWithTag("monster"); // 몬스터 다시 찾기
        if (Monster != null)
        {
            monster = Monster.GetComponent<MonsterManager>();
        }
}

}