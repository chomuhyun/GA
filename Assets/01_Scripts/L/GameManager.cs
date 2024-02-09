using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    public DataMgrDontDestroy dataMgrDontDestroy;

    Button exitBtn;

    private void Start()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 마을의 Exit 버튼을 누르면 지금까지의 정보를 저장하고 캐릭터 선택씬으로 이동.
    public void OnExitClick()
    {
        dataMgrDontDestroy.SaveDate();
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("CHSelect");
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //if(Input.GetKeyDown(KeyCode.Tab))
            //{
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
            //}
        }
    }
}
