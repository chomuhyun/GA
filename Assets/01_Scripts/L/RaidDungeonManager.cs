using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RaidDungeonManager : MonoBehaviourPunCallbacks
{
    public SpawnScipt spawnMgr;

    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;

    public Button exitBtn;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        // 접속 정보 추출 및 표시
        SetRoomInfo();
        // Exit 버튼 이벤트 연결
        exitBtn.onClick.AddListener(() => OnClickExitBtn());

        Debug.Log("클라이언트 상태: " + PhotonNetwork.NetworkClientState + ", InRoom : "+PhotonNetwork.InRoom+ ", InRoom : "+PhotonNetwork.InLobby);
        spawnMgr = GameObject.Find("SpawnMgr").GetComponent<SpawnScipt>();
    }

    private void Start()
    {
        spawnMgr.CreatePlayer();
    }

    // Exit 버튼의 OnClick에 연결할 함수
    private void OnClickExitBtn()
    {
        RoomEnterManager.dungeonType = "None";
        PhotonNetwork.LeaveRoom();
    }

    #region 포톤 콜백함수
    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    // 포톤 룸에서 퇴장했을 때 호출되는 콜백함수
    public override void OnLeftRoom()
    {
        Debug.Log("방 나나기 완료.");
        PhotonNetwork.JoinLobby();
        //SceneManager.LoadScene("Lobby");
    }

    // 로비 참가완료 했을때 콜백함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비 입장 완료.");
        PhotonNetwork.JoinRoom("Room_Home");
        Debug.Log("Room_Home에 Join 실행");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Room_Home에 Join 실패");

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;     // 룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true;       // 룸의 오픈 여부
        ro.IsVisible = true;    // 로비에서 룸 목록에 노출시킬 여부

        PhotonNetwork.CreateRoom("Room_Home", ro);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Room_Home에 Join 성공");

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Town");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("접속끊김. 재접속 실행..");
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion

    #region RoomInfo
    // 룸 접속 정보를 출력
    void SetRoomInfo()
    {
        Room room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        connectInfo.text = $"({room.PlayerCount}/{room.MaxPlayers})";
    }

    // 룸으로 새로운 네트워크 유저가 입장했때 호출되는 콜백함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        msgList.text += msg;
    }

    // 룸에서 네트워크 유저가 퇴장했때 호출되는 콜백함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        msgList.text += msg;
    }
    #endregion
}
