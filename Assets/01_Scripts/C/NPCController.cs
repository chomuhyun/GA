using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    //public GameObject questPanel;
    //public GameObject achievementPanel;
    //public GameObject weaponPanel;
    //public GameObject lvPanel;
    //public GameObject clothingPanel;

    //public InventoryManager inventoryMgr;
    //public EnforceMgr enforceMgr;
    //public LevelUpMgr levelupMgr;
    //public QuestManager questMgr;

    //[TextArea(1, 3)]
    //public string introduce;

    //public int npcNum;
    
    //void Awake()
    //{
    //    inventoryMgr = GameObject.Find("InventoryMgr").GetComponent<InventoryManager>();
    //    enforceMgr = GameObject.Find("EnforceMgr").GetComponent<EnforceMgr>();
    //    levelupMgr = GameObject.Find("LevelupMgr").GetComponent<LevelUpMgr>();
    //    questMgr = GameObject.Find("QuestMgr").GetComponent<QuestManager>();
    //    questPanel = GameObject.Find("QuestPanel");
    //    achievementPanel = GameObject.Find("AchievementPanel");
    //    weaponPanel = GameObject.Find("EnforcePanel");
    //    lvPanel = GameObject.Find("LevelUpPanel");
    //    clothingPanel = GameObject.Find("ClothingPanel");
    //}
    
    //private void Start()
    //{
    //    questPanel.SetActive(false);
    //    achievementPanel.SetActive(false);
    //    weaponPanel.SetActive(false);
    //    lvPanel.SetActive(false);
    //    clothingPanel.SetActive(false);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player")&& npcNum== 1) //Äù½ºÆ®
    //    {
            
    //        questPanel.SetActive(true);
            
    //    }
    //    if (other.CompareTag("Player") && npcNum == 2) //¾÷Àû
    //    {
    //        achievementPanel.SetActive(true);

    //    }
    //    if (other.CompareTag("Player") && npcNum == 3) //´ëÀåÀåÀÌ
    //    {
    //        //enforceMgr.stateMgr = GameObject.FindWithTag("Player").GetComponent<StateManager>();
    //        //enforceMgr.inventoryMgr = GameObject.Find("InventoryMgr").GetComponent<InventoryManager>();
    //        enforceMgr.InitAtk();
    //        weaponPanel.SetActive(true);

    //    }
    //    if (other.CompareTag("Player") && npcNum == 4) //·¾¾÷
    //    {
    //        //levelupMgr.stateMgr = GameObject.FindWithTag("Player").GetComponent<StateManager>();
    //        lvPanel.SetActive(true);

    //    }
    //    if (other.CompareTag("Player") && npcNum == 5) //¿Ê
    //    {
    //        clothingPanel.SetActive(true);

    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        questPanel.SetActive(false);
    //        achievementPanel.SetActive(false);
    //        weaponPanel.SetActive(false);
    //        lvPanel.SetActive(false);
    //        clothingPanel.SetActive(false);
    //    }
    //}

   
}
