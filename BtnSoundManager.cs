using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSoundManager : MonoBehaviour
{
    public AudioClip buttonSound; // ��ư Ŭ�� ����
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(buttonSound); // ��ư ���� ���
    }
}
