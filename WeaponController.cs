using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponData[] weaponDataLevels; // 무기 데이터 배열
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private int upgradeLevel = 0;
    //private Image spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer = GetComponent<Image>();

        // 초기 상태 설정
        UpdateWeaponAppearance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeWeapon()
    {
        upgradeLevel++;
        //if (upgradeLevel > 2) upgradeLevel = 0;  // 최대 업그레이드 후 초기화 (순환적으로 변경)

        UpdateWeaponAppearance();
    }
    
    public void UpdateWeaponAppearance()
    {
        if (upgradeLevel < weaponDataLevels.Length)
        {
            // 무기 이미지 및 애니메이션 변경
            spriteRenderer.sprite = weaponDataLevels[upgradeLevel].weaponSprite;

            // 애니메이션 트리거 설정
            switch (upgradeLevel)
            {
                case 0:
                    animator.SetTrigger("ToSword"); // 기본 애니메이션 설정
                    break;
                case 1:
                    animator.SetTrigger("ToHammer"); // 해머 애니메이션 설정
                    break;
                case 2:
                    animator.SetTrigger("ToShythe"); // 시타 애니메이션 설정
                    break;
            }
        }
    }
}
