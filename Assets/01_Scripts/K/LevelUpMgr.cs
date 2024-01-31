using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class LevelUpMgr : MonoBehaviour
{

    public TextAsset leveltxtFile; //Jsonfile


    public StateManager stateMgr;
    public InventoryManager inventoryMgr;
    public GameObject lvupPanel;
    public int playerlv;
    public int classNum;
    public int playerExp;
    public int upMaxExp;

    [Header("레벨업 패널")]
    public Text playerLvTxt;
    public Image expSlideImg;
    public Text expChartTxt;
    public Text playerHaveExpTxt;

    [Header("증가값 패널")]
    public Text beforeHealth;
    public Text afterHealth;
    public Text beforeCriPer;
    public Text afterCriPer;
    public Text beforeCriDmg;
    public Text afterCriDmg;

    private void Awake()
    {
        var jsonitemFile = Resources.Load<TextAsset>("Json/LvupTable");
        leveltxtFile = jsonitemFile;

        lvupPanel = GameObject.Find("LevelUpPanel");
        playerLvTxt = GameObject.Find("PlayerLevelInfo").GetComponent<Text>();
    }


    public void PlayerCheck() // 현재 플레이어 가진거 불러옴, npc 누를때 호출
    {
        playerlv = stateMgr.level;
        playerHaveExpTxt.text = inventoryMgr.expPotion.ToString();
        
    }

    public void LevelUpCheck() // 레벨업 처음에 세팅됌
    {
        InitCheckLevel(playerlv,classNum);
        lvupPanel.SetActive(true);
    }

    public void OnLevelUpBtn()
    {
        string json = leveltxtFile.text;
        var jsonData = JSON.Parse(json);


        //여기 if문으로 class보고 따로 받음

        //여기에 json의 추가량만큼 더해줌

        InitCheckLevel(playerlv, classNum); // 초기화해줌
    }

    public void InitCheckLevel(int lv, int classnum)
    {
        string json = leveltxtFile.text;
        var jsonData = JSON.Parse(json);

        playerLvTxt.text = stateMgr.level.ToString();
        playerHaveExpTxt.text = inventoryMgr.expPotion.ToString();
        beforeHealth.text = stateMgr.hp.ToString();

        //여기서 클래스에 따른 json파일 가져옴

        afterHealth.text = (jsonData["LvWarrior"][lv]["PlayerHp"]);
        beforeCriPer.text = stateMgr.criChance.ToString();
        //afeterCriPer.text = json 다음꺼
        beforeCriPer.text = stateMgr.criDamage.ToString();
        //afterCriDmg.Text = json 다음꺼



        inventoryMgr.InitInventory(); // 인벤토리를 초기화해줌
    }
}
