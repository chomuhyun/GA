using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class DungeonLoadManager : MonoBehaviourPunCallbacks
{
    public string dungeonType;

    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("���� �����Ʈ��ũ ������ �ƴϱ⿡ �����մϴ�.");
            PhotonNetwork.ConnectUsingSettings();
        }
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("���� �κ� ���⿡ �κ� �����մϴ�.");
            PhotonNetwork.JoinLobby();
        }   
    }
    void Start()
    {
        dungeonType = RoomEnterManager.dungeonType;
        Debug.Log("DungeonLoad.dungeonType : " + dungeonType);
    }

    void Update()
    {

    }
    IEnumerator LoadLevelRaidDungeon()
    {
        yield return null;

        PhotonNetwork.LoadLevel("RaidDungeon");
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    IEnumerator EnterDungeonRoom()
    {
        if (PhotonNetwork.InLobby && dungeonType== "raidDungeon")
        {
            PhotonNetwork.JoinRoom("Room_Raid");
        }
        else
        {
            Debug.Log("EnterDungeonRoom�� if���� ����������");
        }

        yield return null;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("���� �����մϴ�");
        CreateRaidRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Room���� ����");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        SetRoomInfo();
    }

    public void CreateRaidRoom()
    {
        Debug.Log("CreateRaidRoom ����");

        // ���� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 3;     // �뿡 ������ �� �ִ� �ִ� ������ ��
        ro.IsOpen = true;       // ���� ���� ����
        ro.IsVisible = true;    // �κ񿡼� �� ��Ͽ� �����ų ����

        PhotonNetwork.CreateRoom("Room_Raid", ro);
    }

    // �� ���� ������ ���
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

    // �κ� �����Ϸ� ������ �ݹ��Լ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("�κ� ���� �Ϸ�.");
        StartCoroutine(EnterDungeonRoom());
    }

    // ������ ���ο� ��Ʈ��ũ ������ �����߶� ȣ��Ǵ� �ݹ��Լ�
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        msgList.text += msg;

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Debug.Log("CurPlayerCount==MaxPlayerCount �Դϴ�.");
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("������ Ŭ���̾�Ʈ �Դϴ�. �ڷ�ƾ ������.");
                StartCoroutine(LoadLevelRaidDungeon());
            }
            else
            {
                Debug.Log("������ Ŭ���̾�Ʈ�� �ƴմϴ�. �ڷ�ƾ�������.");
            }
        }
    }

    // �뿡�� ��Ʈ��ũ ������ �����߶� ȣ��Ǵ� �ݹ��Լ�
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        msgList.text += msg;
    }
}
