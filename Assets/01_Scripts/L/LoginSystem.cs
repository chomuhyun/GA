using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoginSystem : MonoBehaviourPunCallbacks
{
    public static string userEmail;
    public string password;
    public InputField userIDInput;
    //public InputField pwInput;
    public GameObject em;
    public Text outputText;

    public bool isExist = false;

    PhotonManager photonManager;

    void Start()
    {
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }

    public void OnClickCreateBtn()
    {
        Debug.Log("ȸ�����Թ�ư ����");
        if (string.IsNullOrEmpty(userIDInput.text))
        {
            Debug.Log("userID�� �Է����ּ���!");
        }
        else
        {
            // PlayerPrefs�� userID ����
            PlayerPrefs.SetString("UserID", userIDInput.text);
            PlayerPrefs.Save();
        }
    }

    public void OnClickLoginBtn()
    {
        Debug.Log("�α��ι�ư ����");
        if (string.IsNullOrEmpty(userIDInput.text))
        {
            Debug.Log("userID�� �Է����ּ���!");
        }
        else
        {
            if (PlayerPrefs.HasKey("UserID"))
            {
                //em.SetActive(true);
                photonManager.JoinHome();
            }
            else
            {
                Debug.Log("userID�� �����ϴ�. ȸ�������� ���ּ���");
            }
        }
    }
}
