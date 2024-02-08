using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotalTeleport : MonoBehaviour
{
    Transform TeleportPos;
    public GameObject loadPanels;
    public Image loadPanel;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            loadPanels.SetActive(true);
            other.transform.position = TeleportPos.position;
            loadPanel.fillAmount = -Time.deltaTime;
            if(loadPanel.fillAmount == 0)
            {
                Destroy(loadPanel);
            }

        }
    }
}
