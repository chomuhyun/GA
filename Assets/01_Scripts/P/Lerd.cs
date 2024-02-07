using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerd : MonoBehaviour
{
    public QuestPopUpManager questPopUpManager;
    public DataMgrDontDestroy dataMgrDontDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                dataMgrDontDestroy.QuestCurCnt++;
                questPopUpManager.UpdateQuestStatus();
                Debug.Log("����");
            }
        }
    }
    void Start()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
    }

    void Update()
    {
        
    }
}
