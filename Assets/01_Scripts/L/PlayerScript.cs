using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    //클론이 통신을 받는 변수 설정
    public Vector3 currPos;
    public Quaternion currRot;

    public float speed = 10.0f;

    private Transform tr;
    public PhotonView pv;
    public Text nickNameTxt;

    public ChatManager chatManager;

    public bool allowMove = true;

    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        chatManager = GetComponent<ChatManager>();
        Canvas nickCanvas = GetComponentInChildren<Canvas>();
        nickNameTxt = nickCanvas.GetComponentInChildren<Text>();

        chatManager.StartCoroutine(chatManager.CheckEnterKey());
    }

    void Update()
    {
        if(pv.IsMine)
        {
            nickNameTxt.text = PhotonNetwork.NickName + " (나)";
            //만약 채팅창이 활성화되어 있으면 플레이어를 움직이지 않음
            if (allowMove)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");
                Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
                tr.Translate(movement * Time.deltaTime * speed);
            }
        }
        else
        {
            nickNameTxt.text = pv.Owner.NickName;
            nickNameTxt.color = Color.red;
            //끊어진 시간이 너무 길 경우(텔레포트)
            if ((tr.position - currPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            //끊어진 시간이 짧을 경우(자연스럽게 연결 - 데드레커닝)
            else
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }
    }


    // 통신 송수신
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        //클론이 통신을 받는 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}