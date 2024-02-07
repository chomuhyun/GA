using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class DungeonLoadManager : MonoBehaviourPunCallbacks
{
    public DataMgrDontDestroy dataMgrDontDestroy;

    public int roomCnt = 0;

    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;

    private void Awake()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("현재 포톤네트워크 접속이 아니기에 접속합니다.");
            PhotonNetwork.ConnectUsingSettings();
        }
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("현재 로비에 없기에 로비에 입장합니다.");
            PhotonNetwork.JoinLobby();
        }
    }

    // 마을로가고싶으면  던전에서 나갈때 dataMgrDontDestroy.dungeonSortIdx를 0으로 하고나서 로딩씬에 오면된다.
    IEnumerator EnterDungeonRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            switch (dataMgrDontDestroy.dungeonSortIdx)
            {
                case 1:
                    PhotonNetwork.JoinRoom($"Room_Single_{roomCnt}");
                    break;
                case 2:
                    PhotonNetwork.JoinRoom($"Room_Chaos_{roomCnt}");
                    break;
                case 3:
                    PhotonNetwork.JoinRoom($"Room_Raid_{roomCnt}");
                    break;
                default:
                    PhotonNetwork.JoinRoom("Room_Home");
                    break;
            }
        }
        else
        {
            Debug.Log("EnterDungeonRoom의 if문에 들어가지못했음");
        }

        yield return null;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("현재 inLobby : " + PhotonNetwork.InLobby);
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("던전 생성합니다");
        switch (dataMgrDontDestroy.dungeonSortIdx)
         {
            case 1:
                CreateSingleRoom(roomCnt);
                break;
            case 2:
                CreateChaosRoom(roomCnt);
                break;
            case 3:
                CreateRaidRoom(roomCnt);
                break;
            default:
                CreateHome();
                break;
            }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Room입장 성공");
        SetRoomInfo();

        if (PhotonNetwork.InRoom)
        {
            switch (dataMgrDontDestroy.dungeonSortIdx)
            {
                case 1:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.LoadLevel("SingleDungeon");
                    }
                    break;
                case 2:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.LoadLevel("ChaosD");
                    }
                    break;
                case 3:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.LoadLevel("Raid");
                        PhotonNetwork.AutomaticallySyncScene = true;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("OnJoinedRoom 콜백함수의 if문에 들어가지못했음");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        roomCnt++;
        StartCoroutine(EnterDungeonRoom());
    }

    public void CreateSingleRoom(int Cnt)
    {
        Debug.Log("CreateRaidRoom 실행");

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 1;     // 룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true;       // 룸의 오픈 여부
        ro.IsVisible = false;    // 로비에서 룸 목록에 노출시킬 여부

        PhotonNetwork.CreateRoom($"Room_Single_{Cnt}", ro);
    }

    public void CreateChaosRoom(int Cnt)
    {
        Debug.Log("CreateRaidRoom 실행");

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 1;     // 룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true;       // 룸의 오픈 여부
        ro.IsVisible = false;    // 로비에서 룸 목록에 노출시킬 여부

        PhotonNetwork.CreateRoom($"Room_Chaos_{Cnt}", ro);
    }

    public void CreateRaidRoom(int Cnt)
    {
        Debug.Log("CreateRaidRoom 실행");

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 3;     // 룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true;       // 룸의 오픈 여부
        ro.IsVisible = true;    // 로비에서 룸 목록에 노출시킬 여부

        PhotonNetwork.CreateRoom($"Room_Raid_{Cnt}", ro);
    }

    public void CreateHome()
    {
        Debug.Log("CreateHome 실행");

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;     // 룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true;       // 룸의 오픈 여부
        ro.IsVisible = true;    // 로비에서 룸 목록에 노출시킬 여부

        PhotonNetwork.CreateRoom("Room_Home");
    }

    // 룸 접속 정보를 출력
    void SetRoomInfo()
    {
        Room room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        connectInfo.text = $"({room.PlayerCount}/{room.MaxPlayers})";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    // 로비 참가완료 했을때 콜백함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비 입장 완료.");
        Debug.Log("현재 inLobby : "+PhotonNetwork.InLobby);
        StartCoroutine(EnterDungeonRoom());
    }
}
