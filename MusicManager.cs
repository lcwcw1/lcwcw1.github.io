using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicClips; // 배경음악 클립 배열
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomMusic();
    }

    void PlayRandomMusic()
    {
        if (musicClips.Length == 0) return;

        // 무작위로 음악 선택
        AudioClip clip = musicClips[Random.Range(0, musicClips.Length)];
        audioSource.clip = clip;
        audioSource.Play();

        // 음악이 끝나면 다음 음악 재생
        StartCoroutine(WaitForMusicEnd(clip.length));
    }

    IEnumerator WaitForMusicEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayRandomMusic(); // 다음 음악 재생
    }
}
