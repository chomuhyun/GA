using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    public SpawnScipt spawnMgr;
    public int slotNum;

    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;
    public Button exitBtn;
    public GameObject chatBox;

    void Awake()
    {
        // 접속 정보 추출 및 표시
        //SetRoomInfo();
        spawnMgr = GameObject.Find("SpawnMgr").GetComponent<SpawnScipt>();
    }

    void Start()
    {
        spawnMgr.StartCoroutine(spawnMgr.SpwanPlayer());
    }

    // 룸 접속 정보를 출력
    //void SetRoomInfo()
    //{
     //   Room room = PhotonNetwork.CurrentRoom;
    //    roomName.text = room.Name;
    //    connectInfo.text = $"({room.PlayerCount}/{room.MaxPlayers})";
    //}

    // Exit 버튼의 OnClick에 연결할 함수
    public void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LoginIntro");
    }

    //public void OnClickStartBtn()
    //{
    //    SetUserId();
   //     selectCharPanel.SetActive(false);
    //    //chatBox.SetActive(true);
   //     spawnMgr.StartCoroutine(spawnMgr.SpwanPlayer());
    //}

    //public void OnClickGoLoginSceneBtn()
    //{
    //    PhotonNetwork.LeaveRoom();
    //    SceneManager.LoadScene("Login");
    //}

    //유저명을 설정하는 로직
    //public void SetUserId()
    //{
     //   slotNum = SelectSlot.slotNum;
     //   Debug.Log("SlotNum : " + slotNum);
     //   switch (slotNum)
     //   {
     //       case 0:
      //          PhotonNetwork.NickName = loadPlayerInfo.slot1Text[0].text;
      //          break;
     //       case 1:
     //           PhotonNetwork.NickName = loadPlayerInfo.slot2Text[0].text;
     //          break;
      //      case 2:
      //          PhotonNetwork.NickName = loadPlayerInfo.slot3Text[0].text;
      //          break;
      //      default:
      //          break;
     //   }
    //}

    #region 포톤 콜백함수
    // 포톤 룸에서 퇴장했을 때 호출되는 콜백함수
    public override void OnLeftRoom()
    {
        Debug.Log("방 나가기 완료.");
        //PhotonNetwork.JoinLobby();
        //Debug.Log("JoinLobby 실행");
    }

    // 룸으로 새로운 네트워크 유저가 입장했때 호출되는 콜백함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)  
    {
        //SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        msgList.text += msg;
    }

    // 룸에서 네트워크 유저가 퇴장했때 호출되는 콜백함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        msgList.text += msg;
    }
    #endregion
}
