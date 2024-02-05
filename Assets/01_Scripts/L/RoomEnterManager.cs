using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class RoomEnterManager : MonoBehaviourPunCallbacks
{
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

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }
    #endregion
}
