using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Image weaponImage;        // 무기 외형을 표시할 UI 이미지
    public WeaponData[] weaponDataLevels;
    public WeaponData currentWeapon; // 현재 무기 데이터
    
    
    void Start()
    {
        int savedLevel = PlayerPrefs.GetInt("WeaponLevel", 0); // 저장된 무기 레벨 불러오기
        // 배열의 인덱스 범위를 벗어나지 않도록 방어 코드 추가
        if (savedLevel >= 0 && savedLevel < weaponDataLevels.Length)
        {
            UpdateWeapon(weaponDataLevels[savedLevel]);
        }
        else
        {
            Debug.LogWarning("Saved weapon level is out of bounds, setting to default level 0.");
            UpdateWeapon(weaponDataLevels[0]); // 기본 레벨로 설정
        }
    }

    // 무기 데이터를 변경하여 외형과 공격력을 업데이트하는 메서드
    public void UpdateWeapon(WeaponData newWeaponData)
    {
        if (newWeaponData != null)
        {
            currentWeapon = newWeaponData; // 현재 무기를 업데이트
            weaponImage.sprite = currentWeapon.weaponSprite; // 무기 이미지 업데이트

            // 애니메이션 트리거 추가
            WeaponController WeaponController = GetComponent<WeaponController>();
            if (WeaponController != null)
            {
                WeaponController.UpdateWeaponAppearance(); // 무기 애니메이션 업데이트
            }

            Debug.Log("Weapon Updated: " + currentWeapon.weaponSprite.name); // 업데이트된 무기 이미지 확인
        }
        else
        {
            Debug.LogWarning("New weapon data is null.");
        }
    }
}
