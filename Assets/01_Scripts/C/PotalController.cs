using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class PotalController : MonoBehaviour
{
    public QuestPopUpManager qPopupMgr;
    //나중애 삭제

    public bool isPortal = false;
    public GameObject portalPanel;

    public GameObject skyPortal;

    private void Awake()
    {
        qPopupMgr = GameObject.Find("QuestPopUp").GetComponent<QuestPopUpManager>();//퀘스트 1 달성과제 관련 항목.
        portalPanel = GameObject.Find("PortalPanel");
        skyPortal = GameObject.Find("SkyPortal");
        skyPortal.SetActive(false);
    }
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Portal();
            qPopupMgr.QuestIndexUp(1);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Portal();
        }
    }
    public void Portal()
    {
        if (isPortal == false)
        {
            Jun_TweenRuntime[] gameObjects = portalPanel.GetComponents<Jun_TweenRuntime>();
            gameObjects[0].Play();
            isPortal = true;
            skyPortal.SetActive(true);
            skyPortal.GetComponent<Jun_TweenRuntime>().Play();
        }
        else
        {
            Jun_TweenRuntime[] gameObjects = portalPanel.GetComponents<Jun_TweenRuntime>();
            gameObjects[1].Play();
            isPortal = false;

            skyPortal.SetActive(false);
        }
    }
}
