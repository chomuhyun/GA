using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 게임의 버전
    private readonly string version = "1.0";

    public LoadPlayerInfo loadPlayerInfo;
    public int slotNum;
    public Canvas chCanvas;
    public GameObject fakeloading;

    void Awake()
    {
        // 마스터 클라이언트의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true;

        // 게임 버전 설정
        PhotonNetwork.GameVersion = version;

        // 포톤 서버와의 데이터의 초당 전송 횟수
        //Debug.Log(PhotonNetwork.SendRate);

        // 포톤 서버 접속
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        loadPlayerInfo = GameObject.Find("LoadPlayerInfo").GetComponent<LoadPlayerInfo>();
    }


    #region 포톤 콜백 함수
    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 참가실패");
        Debug.Log($"JoinRoom Failed {returnCode}:{message}");
        MakeHome();
    }

    // 룸 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Create Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }
    // 룸에 입장한 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("MasterClient is LoadLevel 실행");
            PhotonNetwork.LoadLevel("Town 1");
        }
    }
    #endregion

    #region UI_BUTTON_EVENT
    //유저명을 설정하는 로직
    public void SetUserId()
    {
        slotNum = SelectSlot.slotNum;
        Debug.Log("SlotNum : " + slotNum);
        switch (slotNum)
        {
            case 0:
                PhotonNetwork.NickName = loadPlayerInfo.slot1Text[0].text;
                break;
            case 1:
                PhotonNetwork.NickName = loadPlayerInfo.slot2Text[0].text;
                break;
            case 2:
                PhotonNetwork.NickName = loadPlayerInfo.slot3Text[0].text;
                break;
            default:
                break;
        }
    }

    public void OnClickStartBtn()
    {
        SetUserId();
        StartCoroutine(FakePanelOn());
    }

    IEnumerator FakePanelOn()
    {
        Instantiate(fakeloading, chCanvas.transform);
        yield return new WaitForSeconds(1f);
        JoinHome();
    }

    public void JoinHome()
    {
        Debug.Log("JoinHome 실행");
        PhotonNetwork.JoinRoom("Room_Home");
    }

    public void MakeHome()
    {
        Debug.Log("MakeHome 실행");

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;     // 룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true;       // 룸의 오픈 여부
        ro.IsVisible = true;    // 로비에서 룸 목록에 노출시킬 여부

        PhotonNetwork.CreateRoom("Room_Home", ro);
    }

    public void OnClickGoIntroBtn() // 연결끊고 인트로씬으로가기
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Intro");
    }
    #endregion
}