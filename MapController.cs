using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public GameObject mapPanel; // 맵 패널 오브젝트


    // 맵 열기/닫기
    public void ToggleMap()
    {
        mapPanel.SetActive(!mapPanel.activeSelf); // 현재 상태를 반전시킴
    }

    public void CloseMap()
    {
        mapPanel.SetActive(false); // 패널을 비활성화
    }

    // 씬 전환
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // 씬 이름을 통해 씬 로드
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public void LoadSceneAdditive(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
