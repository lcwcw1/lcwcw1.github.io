using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] public Button _startButton = null; // ���� ���� ��ư
    [SerializeField] public Button _optionButton = null; // ���� ��ư

    private void Start()
    {
        _startButton.onClick.AddListener(gameStart); // ���� ���� ��ư Ŭ�� �� gameStart �޼��� ȣ��
        _optionButton.onClick.AddListener(option); // ���� ��ư Ŭ�� �� option �޼��� ȣ��
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
        GameManager.instance.SaveGameData(); // ���� ���� �� ������ ����
    }
}
