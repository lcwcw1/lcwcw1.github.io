using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeManager : MonoBehaviour
{
    public PlayerManager playerManager; // PlayerManager 스크립트 참조
    public GameObject weaponUpgradePanel; // 무기 업그레이드 패널
    public Text currencyText; // 재화 표시 텍스트
    public Button OpenButton;
    public Button SwordButton;
    public Button ShytheButton; // 낫 버튼
    public Button hammerButton; // 해머 버튼
    public Button closeButton; // 닫기 버튼

    // 무기 오브젝트 참조
    public GameObject Sword;
    public GameObject Shythe;
    public GameObject Hammer;

    private int currency = 1000; // 예시 재화 수치
    private string currentWeapon = "Basic"; // 현재 무기

    void Start()
    {
        UpdateCurrencyText();
        
        OpenButton.onClick.AddListener(OpenUpgradeButton);
        SwordButton.onClick.AddListener(UpgradeToSword);
        ShytheButton.onClick.AddListener(UpgradeToShythe);
        hammerButton.onClick.AddListener(UpgradeToHammer);
        closeButton.onClick.AddListener(CloseUpgradeButton);
    }

    public void OpenUpgradeButton()
    {
        bool isActive = weaponUpgradePanel.activeSelf;
        Debug.Log("Current Panel State: " + isActive); // 패널 상태 확인용 디버그 메시지
        weaponUpgradePanel.SetActive(!isActive); // 패널 활성화 상태 토글
        Debug.Log("New Panel State: " + !isActive); // 변경된 상태 확인
    }
    public void CloseUpgradeButton()
    {
        bool isActive = weaponUpgradePanel.activeSelf;
        Debug.Log("Current Panel State: " + isActive); // 패널 상태 확인용 디버그 메시지
        weaponUpgradePanel.SetActive(!isActive); // 패널 활성화 상태 토글
        Debug.Log("New Panel State: " + !isActive); // 변경된 상태 확인
    }

    public void UpgradeToSword()
    {
        if (currency >= 0) // 칼로 변경할 비용
        {
            currency -= 0; // 재화 소모
            currentWeapon = "Sword"; // 무기 변경
            playerManager.ChangeWeapon(currentWeapon); // 캐릭터 무기 변경
            ActivateWeapon(Sword); // Sword 무기 활성화
            UpdateCurrencyText();
            Debug.Log("무기가 칼로 변경되었습니다.");
        }
        else
        {
            Debug.Log("재화가 부족합니다.");
        }
    }
    public void UpgradeToShythe()
    {
        if (currency >= 1000) // 낫으로 변경할 비용
        {
            currency -= 1000; // 재화 소모
            currentWeapon = "Shythe"; // 무기 변경
            playerManager.ChangeWeapon(currentWeapon); // 캐릭터 무기 변경
            ActivateWeapon(Shythe); // Shythe 무기 활성화
            UpdateCurrencyText();
            Debug.Log("무기가 낫으로 변경되었습니다.");
        }
        else
        {
            Debug.Log("재화가 부족합니다.");
        }
    }

    public void UpgradeToHammer()
    {
        if (currency >= 3000) // 해머로 변경할 비용
        {
            currency -= 3000; // 재화 소모
            currentWeapon = "Hammer"; // 무기 변경
            playerManager.ChangeWeapon(currentWeapon); // 캐릭터 무기 변경
            ActivateWeapon(Hammer); // Hammer 무기 활성화
            UpdateCurrencyText();
            Debug.Log("무기가 해머로 변경되었습니다.");
        }
        else
        {
            Debug.Log("재화가 부족합니다.");
        }
    }

    private void ActivateWeapon(GameObject selectedWeapon)
    {
        // 모든 무기 비활성화
        Sword.SetActive(false);
        Shythe.SetActive(false);
        Hammer.SetActive(false);

        // 선택한 무기만 활성화
        selectedWeapon.SetActive(true);
    }
    
    private void UpdateCurrencyText()
    {
        currencyText.text = currency.ToString(); // 재화 업데이트
    }
}
