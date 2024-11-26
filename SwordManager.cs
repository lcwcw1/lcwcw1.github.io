using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    private Animator swordAnimator;
    private float originalSpeed;
    // Start is called before the first frame update
    void Start()
    {
        // Animator 컴포넌트를 가져오고 원래 속도를 저장
        swordAnimator = GetComponent<Animator>();
        originalSpeed = swordAnimator.speed;
    }

    // Update is called once per frame
    void Update()
    {
        // 화면을 클릭할 때 애니메이션 속도를 두 배로 설정
        if (Input.GetMouseButtonDown(0))
        {
            swordAnimator.speed = originalSpeed * 2;
        }
        // 클릭이 끝나면 원래 속도로 돌아옴
        if (Input.GetMouseButtonUp(0))
        {
            swordAnimator.speed = originalSpeed;
        }
    }
}
