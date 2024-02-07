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

                if(dataMgrDontDestroy.QuestCurCnt < dataMgrDontDestroy.QuestMaxCnt)
                {
                    dataMgrDontDestroy.QuestCurCnt++;
                    if(dataMgrDontDestroy.QuestCurCnt == dataMgrDontDestroy.QuestMaxCnt)
                    {
                        dataMgrDontDestroy.IsCompleted = true;
                    }
                }
                else
                {
                    return;
                }

                questPopUpManager.UpdateQuestStatus();
                Debug.Log("·¯µå");
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
