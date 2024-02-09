using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginSystem : MonoBehaviour
{
    public static string userEmail;
    public string password;
    public InputField userIDInput;
    //public InputField pwInput;

    public Text outputText;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }
    public void OnClickCreateBtn()
    {
        Debug.Log("회원가입버튼 누름");
        if (string.IsNullOrEmpty(userIDInput.text))
        {
            Debug.Log("userID를 입력해주세요!");
        }
        else
        {
            // PlayerPrefs에 userID 저장
            PlayerPrefs.SetString("UserID", userIDInput.text);
            PlayerPrefs.Save();
        }
    }

    public void OnClickLoginBtn()
    {
        Debug.Log("로그인버튼 누름");
        if (string.IsNullOrEmpty(userIDInput.text))
        {
            Debug.Log("userID를 입력해주세요!");
        }
        else
        {
            if (PlayerPrefs.HasKey("UserID"))
            {
                SceneManager.LoadScene("ChSelect");
            }
            else
            {
                Debug.Log("userID가 없습니다. 회원가입을 해주세요");
            }
        }
    }
}
