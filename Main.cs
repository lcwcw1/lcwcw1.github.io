using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] public Button _startButton = null; // 게임 시작 버튼
    [SerializeField] public Button _optionButton = null; // 설정 버튼

    private void Start()
    {
        _startButton.onClick.AddListener(gameStart); // 게임 시작 버튼 클릭 시 gameStart 메서드 호출
        _optionButton.onClick.AddListener(option); // 설정 버튼 클릭 시 option 메서드 호출
    }

    private void gameStart()
    {
        SceneManager.LoadScene("Village");
    }

    private void option()
    {

    }

    private void OnApplicationQuit()
    {
        GameManager.instance.SaveGameData(); // 게임 종료 시 데이터 저장
    }
}
