using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicClips; // ������� Ŭ�� �迭
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomMusic();
    }

    void PlayRandomMusic()
    {
        if (musicClips.Length == 0) return;

        // �������� ���� ����
        AudioClip clip = musicClips[Random.Range(0, musicClips.Length)];
        audioSource.clip = clip;
        audioSource.Play();

        // ������ ������ ���� ���� ���
        StartCoroutine(WaitForMusicEnd(clip.length));
    }

    IEnumerator WaitForMusicEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayRandomMusic(); // ���� ���� ���
    }
}
