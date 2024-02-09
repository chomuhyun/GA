using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosNPCTalker : MonoBehaviour
{

    public DialogueTrigger dia;
    public GameObject npcConverPanel;

    private void Start()
    {
        npcConverPanel.SetActive(false);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            dia.Trigger();
            npcConverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        npcConverPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
