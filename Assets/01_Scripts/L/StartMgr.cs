using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StartMgr : MonoBehaviourPunCallbacks
{
    public GameObject roomName;
    public GameObject connectInfo;
    public GameObject msgList;
    public GameObject exitBtn;

    public GameObject chatBox;

    private void Start()
    {
        roomName.SetActive(true);
        connectInfo.SetActive(true);
        msgList.SetActive(true);
        exitBtn.SetActive(true);
        chatBox.SetActive(true);
    }
}
