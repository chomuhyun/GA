using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class ChatManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public List<string> chatList = new List<string>();

    public Text chatLog; // viewport에 채팅로그
    //public Text chatterList; // 채터리스트
    public InputField inputChat; // 인풋필드
    public ScrollRect scrollRect;
    //public string chatters;

    public Text nickNameTxt;

    public PhotonView pv;
    public Player player;
    // 여기 위에가 추가한것. 현창

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        player = GetComponent<Player>();
        // 여기 위에가 추가한것. 현창

        GameObject canvasObj = GameObject.Find("WorldCanvas");
        GameObject backChSelect = canvasObj.transform.Find("BackChSelect_Chat").gameObject;
        GameObject chattingBox = backChSelect.transform.Find("ChattingBox").gameObject;
        scrollRect = chattingBox.GetComponentInChildren<ScrollRect>();

        //chatterList = chattingBox.GetComponentInChildren<Text>();
        inputChat = chattingBox.GetComponentInChildren<InputField>();
        chatLog = scrollRect.content.GetComponentInChildren<Text>();

        Canvas nickCanvas = GetComponentInChildren<Canvas>();
        nickNameTxt = nickCanvas.GetComponentInChildren<Text>();

        inputChat.enabled = false;
        chatLog.text = "";

        StartCoroutine(CheckEnterKey());
    }

    public void Update()
    {
        //ChatterUpdate();

        if (pv.IsMine)
        {
            if (!inputChat.isFocused)
            {
                player.allowMove = true; // 깜빡이지 않으니 움직일수있다.
            }
            else
            {
                player.allowMove = false; // 깜빡이니 못움직임
            }
        }
    }


    #region 채팅 함수
    public void OnClickSendBtn()
    {
        if (inputChat.text.Trim().Equals(""))
        {
            Debug.Log("채팅창 Empty, 채팅창을 비활성화 합니다");
            // 채팅창 비어있으니 비활성화
            inputChat.Select();
            inputChat.enabled = false;
            return;
        }
        else
        {
            string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, inputChat.text);
            Debug.Log(msg);
            photonView.RPC("ReceiveMsg", RpcTarget.AllBuffered, msg);
            inputChat.ActivateInputField(); // 메세지를 보내도 활성화
            inputChat.text = "";
        }
    }

    //void ChatterUpdate()
    //{
    //    if (pv.IsMine)
    //    {
    //        string[] playerList = PhotonNetwork.PlayerList.Select(p => p.NickName).ToArray();
    //        string concatenatedPlayerList = string.Join("\n", playerList); // 들어온 순서대로 출력
    //        photonView.RPC("SyncChatterList", RpcTarget.AllBuffered, concatenatedPlayerList);
    //    }
    //}

    //[PunRPC]
    //public void SyncChatterList(string playerList)
    //{
    //    chatterList.text = playerList;
    //}

    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        chatLog.text += "\n" + msg;
        scrollRect.verticalNormalizedPosition = 0.0f;
    }

    public IEnumerator CheckEnterKey()
    {
        while (true)
        {
            if (pv.IsMine && Input.GetKeyDown(KeyCode.Return))
            {
                if (inputChat != null)
                {
                    if (inputChat.enabled == false)
                    {
                        Debug.Log("enter키 누름. 채팅창활성화 합니다..");
                        inputChat.enabled = true;
                        inputChat.ActivateInputField();

                        yield return null;
                    }
                    else
                    {
                        if (!inputChat.isFocused)
                        {
                            Debug.Log("enter키 누름. Focused 가지고있음=깜빡이고있음=메세지입력가능");
                            OnClickSendBtn();
                        }
                        else
                        {
                            Debug.Log("enter키 누름. Focused 가지고있지않음=깜빡이지않음=메세지입력불가");
                            inputChat.ActivateInputField();
                        }
                    }
                }
            }
            yield return null;
        }
    }
    #endregion
    // IPunObservable 구현
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 여기서는 통신 메시지를 보낼 필요가 없으므로 아무 작업도 하지 않습니다.
        }
        else
        {
            // 여기서는 통신 메시지를 받을 필요가 없으므로 아무 작업도 하지 않습니다.
        }
    }
}