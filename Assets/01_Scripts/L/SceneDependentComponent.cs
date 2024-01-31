using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDependentComponent : MonoBehaviour
{
    public ChatManager componentA;
    //public PlayerScript componentB;

    void Awake()
    {
        componentA = GetComponent<ChatManager>();
        // 현재 씬 이름 가져오기
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("현재 씬 이름 : "+currentSceneName);

        // 현재 씬에 따라 컴포넌트 A와 B를 활성화 또는 비활성화
        if (currentSceneName == "Town")
        {
            // 씬 A에서는 컴포넌트 A 활성화, 컴포넌트 B 비활성화
            componentA.enabled = true;
            //componentB.enabled = false;
        }
        else // 홈이 아닐때, 솔로던전이나 레이드던전일때
        {
            // 씬 B에서는 컴포넌트 A 비활성화, 컴포넌트 B 활성화
            componentA.enabled = false;
            //componentB.enabled = true;
        }
    }
}
