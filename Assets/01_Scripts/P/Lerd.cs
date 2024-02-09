using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerd : MonoBehaviour
{
    public QuestPopUpManager questPopUpManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                questPopUpManager.QuestIndexUp(1);                
                Debug.Log("·¯µå");
            }
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
