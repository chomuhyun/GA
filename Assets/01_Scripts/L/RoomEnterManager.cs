using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class RoomEnterManager : MonoBehaviourPunCallbacks
{
    public GameObject dungeonPanel;

    public static string dungeonType;

    //솔로던전은 roomleave하고 씬매니저로 돌릴것
    public void OnSoloDungeon1ButtonClick()
    {
        dungeonType = "singleDungeon";
        Debug.Log("던전의 타입 : " + dungeonType);
        LeaveVillige();
    }

    public void OnSoloDungeon2ButtonClick()
    {
        dungeonType = "chaosDungeon";
        Debug.Log("던전의 타입 : " + dungeonType);
        LeaveVillige();
    }

    public void OnRaidDungeonButtonClick()
    {
        dungeonType = "raidDungeon";
        Debug.Log("던전의 타입 : " + dungeonType);
        LeaveVillige();
    }

    public void LeaveVillige()
    {
        PhotonNetwork.LeaveRoom(); // 마을 룸을 떠납니다.
        StartCoroutine(LoadLoadingScene());
    }

    IEnumerator LoadLoadingScene()
    {
        yield return new WaitForSeconds(1.0f); // 로딩 딜레이

        SceneManager.LoadScene("DungeonLoadingScene"); // 로딩 씬으로 전환
    }

    // 여기에 콜백함수 없어도될듯
    #region 포톤 콜백 함수
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("방떠나기 완료");
        Debug.Log("Lobby에 입장 시도합니다");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Lobby에 입장 완료");
        SceneManager.LoadScene("DungeonLoadingScene");
    }
    //public override void OnJoinedRoom()
    //{
    //    // 현재 방의 던전 타입을 가져옴
    //    if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("DungeonType", out object dungeonTypeObj))
    //    {
    //        string dungeonType = dungeonTypeObj.ToString();
    //        Debug.Log("던전의 이름은 : " + dungeonType);
    //        // 방장인 경우
    //        if (PhotonNetwork.IsMasterClient)
    //        {
    //            // 해당 던전 타입에 맞는 씬을 로드
    //            PhotonNetwork.LoadLevel(dungeonType);
    //        }
    //    }
    //}

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }
    #endregion

    // 던전입장 UI panel키는 버튼누르는 함수
    public void OnClickEnterDungeonBtn()
    {
        dungeonPanel.SetActive(true);
    }
    //던전입장 UI panel끄는 버튼누르는 함수
    public void OnClickCancelBtn()
    {
        dungeonPanel.SetActive(false);
    }
}
