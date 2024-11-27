using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSoundManager : MonoBehaviour
{
    public AudioClip buttonSound; // 버튼 클릭 사운드
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(buttonSound); // 버튼 사운드 재생
    }
}
