using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public float speed = 10.0f;

    //Ŭ���� ����� �޴� ���� ����
    private Vector3 currPos;
    private Quaternion currRot;

    private Transform tr;
    public PhotonView pv;
    public Text nickNameTxt;

    //public ChatManager chatManager;

    private void Awake()
    {
        //chatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
    }

    void Start()
    {
        //chatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        
        //chatManager.inputChat.enabled = false;
        //chatManager.chatLog.text = "";
        
        //StartCoroutine(CheckEnterKey());
    }

    void Update()
    {   
        if (pv.IsMine)
        {
            nickNameTxt.text = PhotonNetwork.NickName + " (��)";
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            tr.Translate(movement * Time.deltaTime * speed);
            // ���� ä��â�� Ȱ��ȭ�Ǿ� ������ �÷��̾ �������� ����
            //if (!chatManager.inputChat.isFocused)
            //{
            //    float moveHorizontal = Input.GetAxis("Horizontal");
            //    float moveVertical = Input.GetAxis("Vertical");
            //    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            //    tr.Translate(movement * Time.deltaTime * speed);
            //    //ChatterUpdate();
            //}
        }
        else
        {
            nickNameTxt.text = pv.Owner.NickName;
            nickNameTxt.color = Color.red;
            //������ �ð��� �ʹ� �� ���(�ڷ���Ʈ)
            if ((tr.position - currPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            //������ �ð��� ª�� ���(�ڿ������� ���� - ���巹Ŀ��)
            else
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }
    }

    //#region ä�� �Լ�
    //public void OnClickSendBtn()
    //{
    //    if (chatManager.inputChat.text.Trim().Equals(""))
    //    {
    //        Debug.Log("ä��â Empty, ä��â�� ��Ȱ��ȭ �մϴ�");
    //        // ä��â ��������� ��Ȱ��ȭ
    //        chatManager.inputChat.Select();
    //        chatManager.inputChat.enabled = false;
    //        return;
    //    }
    //    else
    //    {
    //        string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, chatManager.inputChat.text);
    //        Debug.Log(msg);
    //        photonView.RPC("ReceiveMsg", RpcTarget.AllBuffered, msg);
    //        chatManager.inputChat.ActivateInputField(); // �޼����� ������ Ȱ��ȭ
    //        chatManager.inputChat.text = "";
    //    }
    //}

    ////void ChatterUpdate()
    ////{
    ////    if (pv.IsMine)
    ////    {
    ////        string[] playerList = PhotonNetwork.PlayerList.Select(p => p.NickName).ToArray();
    ////        string concatenatedPlayerList = string.Join("\n", playerList); // ���� ������� ���
    ////        photonView.RPC("SyncChatterList", RpcTarget.AllBuffered, concatenatedPlayerList);
    ////    }
    ////}

    //[PunRPC]
    //public void SyncChatterList(string playerList)
    //{
    //    if (chatManager != null)
    //    {
    //        chatManager.chattingList.text = playerList;
    //    }
    //}

    //[PunRPC]
    //public void ReceiveMsg(string msg)
    //{
    //    chatManager.chatLog.text += "\n" + msg;
    //    chatManager.scroll_rect.verticalNormalizedPosition = 0.0f;
    //}

    //IEnumerator CheckEnterKey()
    //{
    //    while (true)
    //    {
    //        if (pv.IsMine && Input.GetKeyDown(KeyCode.Return))
    //        {
    //            if (chatManager != null && chatManager.inputChat != null)
    //            {
    //                if (chatManager.inputChat.enabled == false)
    //                {
    //                    Debug.Log("enterŰ ����. ä��âȰ��ȭ �մϴ�..");
    //                    chatManager.inputChat.enabled = true;
    //                    chatManager.inputChat.ActivateInputField();

    //                    yield return null;
    //                }
    //                else
    //                {
    //                    if (!chatManager.inputChat.isFocused)
    //                    {
    //                        Debug.Log("enterŰ ����. Focused ����������=�����̰�����=�޼����Է°���");
    //                        OnClickSendBtn();
    //                    }
    //                    else
    //                    {
    //                        Debug.Log("enterŰ ����. Focused ��������������=������������=�޼����ԷºҰ�");
    //                        chatManager.inputChat.ActivateInputField();
    //                    }
    //                }
    //            }
    //        }
    //        yield return null;
    //    }
    //}
    //#endregion

    // ��� �ۼ���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //����� ������ 
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        //Ŭ���� ����� �޴� 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}