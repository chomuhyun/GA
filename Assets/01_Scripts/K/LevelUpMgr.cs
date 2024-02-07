using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using Photon.Pun;

public class LevelUpMgr : MonoBehaviourPunCallbacks
{
    public DataMgrDontDestroy dataMgrDontDestroy;
    public TextAsset leveltxtFile; //Jsonfile
    public GameObject lvupPanel;
    public Slider expSlider;
    public int classNum; //0:전사, 1:거너, 2:법사
    public int playerlv;
    public float playerHp;
    public int playerExp;
    public int playerExpPotion;
    public float playerCriDamage;
    public int playerCriChance;
    public int expRequire;

    [Header("레벨업 패널")]
    public Text playerLvTxt;
    public Image expSlideImg;
    public Text expRequireTxt;
    public Text curExpPotionTxt;

    [Header("증가값 패널")]
    public Text beforeHealth;
    public Text afterHealth;
    public Text beforeCriPer;
    public Text afterCriPer;
    public Text beforeCriDmg;
    public Text afterCriDmg;

    [Header("afterExp, ExpPotion(실반영안됨)")]
    public int afterExp;
    public int afterExpPotionCnt;

    [Header("NPC 대화")]
    public DialogueTrigger dialogueTrigger; //대본
    public GameObject nPCConversation;

    [Header("패널열기버튼")]
    public GameObject panelOnBtnLV;

    private void Awake()
    {
        var jsonitemFile = Resources.Load<TextAsset>("Json/LvupTable");
        leveltxtFile = jsonitemFile;

        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        lvupPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                panelOnBtnLV.SetActive(true);
                panelOnBtnLV.GetComponent<Jun_TweenRuntime>().Play();
                dialogueTrigger.Trigger();
                nPCConversation.SetActive(true);
                Debug.Log("충돌일어남");
                PlayerDataCheck();
                Debug.Log("업데이트할 ui의 클래스넘버 : " + classNum);
                UpdateUiData(classNum);
            }
        }
    }

    public void OnLevelUpPanel()
    {
        lvupPanel.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.F))
            {
                OnLevelUpPanel();
            }

            if (other.GetComponent<PhotonView>().IsMine)
            {
                SyncDataMgr();
                StateManager stateManager = other.gameObject.GetComponent<StateManager>();
                stateManager.level = playerlv;
                stateManager.maxhp = playerHp;
                stateManager.hp = playerHp;
                stateManager.criChance = playerCriChance;
                stateManager.criDamage = playerCriDamage;
                stateManager.exp = playerExp;
                stateManager.userExpPotion = playerExpPotion;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                panelOnBtnLV.SetActive(false);
                nPCConversation.SetActive(false);
                SyncDataMgr();
                lvupPanel.SetActive(false);
            }
        }
    }

    public void OnClickOneBtn()
    {
        if (afterExpPotionCnt == 0) // 포션이 0개라면
        {
            Debug.Log("포션이 부족합니다.");
        }
        else // 포션이 0개가 아니라면
        {
            if (afterExp == expRequire) // 경험치가 최대로 모여있었다면
            {
                Debug.Log("if문 경험치 최대로 모였음");
                Debug.Log("레벨업 가능");
            }
            else // 경험치가 최대로 모여있지않으니 else문 실행
            {
                afterExpPotionCnt -= 1;
                afterExp += 100;
                UpdateUiData(classNum);
            }
        }
    }
    public void OnClickMaxBtn()
    {
        if (afterExpPotionCnt == 0) // 포션이 0개라면
        {
            Debug.Log("포션이 부족합니다.");
        }
        else // 포션이 0개가 아니라면
        {
            if (afterExp == expRequire) // 경험치가 최대로 모여있었다면
            {
                Debug.Log("if문 경험치 최대로 모였음");
                Debug.Log("레벨업 가능");
            }
            else // 경험치가 최대로 모여있지않으니 else문 실행
            {
                int n = (expRequire - afterExp) / 100;
                Debug.Log("한번에 사용할 개수 : " + n);
                if (n > afterExpPotionCnt)
                {
                    afterExp += afterExpPotionCnt * 100;
                    afterExpPotionCnt = 0;
                }
                else
                {
                    afterExpPotionCnt -= n;
                    afterExp += n * 100;
                }
                UpdateUiData(classNum);
            }
        }
    }

    public void OnClickLevelUpBtn()
    {
        string json = leveltxtFile.text;
        var jsonData = JSON.Parse(json);

        if (afterExp==expRequire)
        {
            playerExpPotion = afterExpPotionCnt;
            playerExp = 0;
            afterExp = 0;
            playerlv++;

            playerHp = (jsonData[classNum][playerlv]["PlayerHp"]);
            playerCriDamage = (jsonData[classNum][playerlv]["CriDMG"]);
            playerCriChance = (jsonData[classNum][playerlv]["CriPer"]);
            expRequire = jsonData["ExpRequireTable"][playerlv]["needExp"];
            UpdateUiData(classNum);
        }
        else
        {
            playerExpPotion = afterExpPotionCnt;
            playerExp = afterExp;

            playerHp = (jsonData[classNum][playerlv]["PlayerHp"]);
            playerCriDamage = (jsonData[classNum][playerlv]["CriDMG"]);
            playerCriChance = (jsonData[classNum][playerlv]["CriPer"]);
            expRequire = jsonData["ExpRequireTable"][playerlv]["needExp"];
            UpdateUiData(classNum);
        }

        // DataMgrDontDestroy에도 정보를 보내준다.
        dataMgrDontDestroy.Level = playerlv;
        dataMgrDontDestroy.Exp = playerExp;
        dataMgrDontDestroy.UserExpPotion = playerExpPotion;
        dataMgrDontDestroy.MaxHp = playerHp;
        dataMgrDontDestroy.CriDamage = playerCriDamage;
        dataMgrDontDestroy.CriChance = playerCriChance;
    }

    public void PlayerDataCheck()
    {
        playerlv = dataMgrDontDestroy.Level;
        playerExp = dataMgrDontDestroy.Exp;
        playerExpPotion = dataMgrDontDestroy.UserExpPotion;
        playerHp = dataMgrDontDestroy.MaxHp;
        playerCriDamage = dataMgrDontDestroy.CriDamage;
        playerCriChance = dataMgrDontDestroy.CriChance;
        classNum = dataMgrDontDestroy.ClassNum;

        afterExpPotionCnt = playerExpPotion;
        afterExp = playerExp;

        //playerExpPotion = afterExpPotionCnt;
        //playerExp = afterExp;

        string json = leveltxtFile.text;
        var jsonData = JSON.Parse(json);
        expRequire = jsonData["ExpRequireTable"][playerlv]["needExp"];
    }
    public void UpdateUiData(int classNumber)
    {
        Debug.Log("현재 플레이어의 경험치는 : " + playerExp);
        Debug.Log("레벨업하기위해 필요한 경험치 량 : " + expRequire);
        string json = leveltxtFile.text;
        var jsonData = JSON.Parse(json);

        playerLvTxt.text = playerlv.ToString();
        beforeHealth.text = playerHp.ToString();
        afterHealth.text = (jsonData[classNumber][playerlv+1]["PlayerHp"]);
        beforeCriDmg.text = playerCriDamage.ToString();
        afterCriDmg.text = (jsonData[classNumber][playerlv + 1]["CriDMG"]);
        beforeCriPer.text = playerCriChance.ToString();
        afterCriPer.text = (jsonData[classNumber][playerlv + 1]["CriPer"]);
        curExpPotionTxt.text = afterExpPotionCnt.ToString();
        expRequireTxt.text = $"{afterExp} / {expRequire}";
        expSlider.value = ((float)afterExp / expRequire);
    }

    public void SyncDataMgr()
    {
        dataMgrDontDestroy.Level = playerlv;
        dataMgrDontDestroy.Exp = playerExp;
        dataMgrDontDestroy.UserExpPotion = playerExpPotion;
        dataMgrDontDestroy.MaxHp = playerHp;
        dataMgrDontDestroy.CriDamage = playerCriDamage;
        dataMgrDontDestroy.CriChance = playerCriChance;
        dataMgrDontDestroy.ClassNum = classNum;
    }
}
