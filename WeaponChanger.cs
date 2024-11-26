using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    private WeaponManager weaponManager;  // WeaponManager 참조
    private int currentLevel = 0;
    public WeaponData[] weaponDataLevels; // 무기업그레이드 단계 데이터 배열

    private void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>(); //저장된 무기 레벨 불러오기
        currentLevel = PlayerPrefs.GetInt("WeaponLevel", 0);
        ChangeWeaponAppearance();
    }

    // 버튼을 눌러 무기 외형을 바꾸는 메서드
    /*public void ChangeWeaponAppearance()
    {
        currentLevel = (currentLevel + 1) % weaponDataLevels.Length; // 단계 변경
        weaponManager.UpdateWeapon(weaponDataLevels[currentLevel]);   // WeaponManager에 업데이트
        PlayerPrefs.SetInt("WeaponLevel", currentLevel); // 무기 레벨 저장
        PlayerPrefs.Save(); // 저장된 데이터를 즉시 저장하여 다른 씬에서도 유지
    }*/

        public void ChangeWeaponAppearance()
    {
        currentLevel = (currentLevel + 1) % weaponDataLevels.Length;
        Debug.Log("Current Weapon Level: " + currentLevel); // 현재 무기 레벨 출력
        // 애니메이션 상태 확인
        Debug.Log("Upgrading Weapon to: " + weaponDataLevels[currentLevel].weaponSprite.name);
        weaponManager.UpdateWeapon(weaponDataLevels[currentLevel]);
        PlayerPrefs.SetInt("WeaponLevel", currentLevel); // 무기 레벨 저장
        PlayerPrefs.Save(); // 저장된 데이터를 즉시 저장하여 다른 씬에서도 유지

    }
}
