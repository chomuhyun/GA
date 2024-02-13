using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinNPCcollider : MonoBehaviour
{
    [Header("NPC 대화")]
    public DialogueTrigger dialogueTrigger; //대본
    public GameObject nPCConversation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                dialogueTrigger.Trigger();
                nPCConversation.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        nPCConversation.SetActive(false);
    }
}
