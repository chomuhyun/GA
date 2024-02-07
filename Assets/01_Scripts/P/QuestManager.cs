using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleJSON;
using Photon.Pun;

//###### Quest List, Quest Description UI 담당 ######

public class QuestManager : MonoBehaviour
{
    [Header("DonDestroy")]
    public DataMgrDontDestroy dataMgrDontDestroy;

    public TextAsset txtFile; //Jsonfile
    // 퀘스트 팝업은 싱글톤으로 다른 씬을 갔다가 온다. 그러므로 퀘스트 NPC에 상호작용을 할 때 
    // 현재 팝업에 있는 카운트를 퀘스트 매니저 스크립트에 있는 현재, 최대 카운트 값을 보내줘야한다
    // 그리고 상호작용할때 퀘스트가 완료되었을 때 bool값이 True인지 확인해서
    // 퀘스트완료 버튼을 SetActive = true 로 한다.
    // npc 커밋하고 할것. 
    [Header("Component")]
    public QuestPopUpManager questPopUpManager;

    [Header("퀘스트 선택창")]
    public GameObject questPanel;
    public GameObject quest_1;
    public GameObject quest_2;
    public GameObject quest_3;
    public GameObject quest_4;

    [Header("퀘스트 설명창")]
    public GameObject descriptionPanel;
    public Text questNameTxt;
    public Text goalTxt;

    [Header("NPC 대화")]
    public DialogueTrigger dialogueTrigger; //대본
    public GameObject nextBtn; //대본 진행
    public GameObject nPCConversation;

    public Text textName;
    public Text textSentence;

    Queue<string> naming = new Queue<string>();
    Queue<string> sentence = new Queue<string>();

    [Header("퀘스트 진행버튼")]
    public GameObject clearfowardQuestImg;
    public GameObject acceptBtn;
    public GameObject ingImg;
    public GameObject completedBtn;
    public GameObject endBtn;


    [Header("퀘스트 현재 진행도 창")]
    public GameObject questPopUpPanel;
    public Text questDescriptionGoalTxt; //이 텍스트에 questGoalTxt 의 문자가 들어감
    public string questGoalTxt;
    public int questCurCnt;
    public int questMaxCnt;


    [Header("퀘스트 보상")]
    public int expPotionReward;
    public int materialReward;
    public int goldReward;

    public Text rewardExp;
    public Text rewardMat;
    public Text rewardGold;

    private void Awake()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        questPopUpManager = GameObject.Find("QuestPopUp").GetComponent<QuestPopUpManager>();
        descriptionPanel.SetActive(false);
    }
    private void Start()
    {
        completedBtn.SetActive(false);
        ingImg.SetActive(false);

        questPanel.SetActive(false);
        
        questPopUpManager.questIdx = dataMgrDontDestroy.QuestIdx;
        questGoalTxt = dataMgrDontDestroy.GoalTxt;
        questCurCnt = dataMgrDontDestroy.QuestCurCnt;
        questMaxCnt = dataMgrDontDestroy.QuestMaxCnt;
        //nPCConversation.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("충돌일어남");
                questPanel.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                nPCConversation.SetActive(false);
                descriptionPanel.SetActive(false);
                questPanel.SetActive(false);
            }
        }
    }

    public void InstQuest(int n)
    {
        string json = txtFile.text;
        var jsonData = JSON.Parse(json); //var의 의미: Unity외의 파일을 다가져온다.

        int item = n - 1; //매개변수

        questNameTxt.text = (jsonData["Quest"][item]["QuestName"]);
        goalTxt.text = (jsonData["Quest"][item]["Goal"]);
        rewardExp.text = (jsonData["Quest"][item]["RewardExp"]);
        rewardMat.text = (jsonData["Quest"][item]["RewardMat"]);
        rewardGold.text = (jsonData["Quest"][item]["RewardGold"]);
        Debug.Log("Json 데이터 불러옴");
    }

    public void CompleteFirst(int n) //수락이냐 아니냐, "먼저 진행하고 오세요" 기능
    {
        if (n == dataMgrDontDestroy.QuestIdx)
        {
            if (dataMgrDontDestroy.IsDoing == true)
            {
                if (dataMgrDontDestroy.IsCompleted == true)
                {
                    acceptBtn.SetActive(false);
                    clearfowardQuestImg.SetActive(false);
                    ingImg.SetActive(false);
                    completedBtn.SetActive(true);
                    endBtn.SetActive(false);
                }
                else
                {
                    acceptBtn.SetActive(false);
                    clearfowardQuestImg.SetActive(false);
                    ingImg.SetActive(true);
                    completedBtn.SetActive(false);
                    endBtn.SetActive(false);
                }
            }
            else
            {
                if (dataMgrDontDestroy.IsCompleted == true)
                {
                    acceptBtn.SetActive(false);
                    clearfowardQuestImg.SetActive(false);
                    ingImg.SetActive(false);
                    completedBtn.SetActive(true);
                    endBtn.SetActive(false);
                }
                else
                {
                    acceptBtn.SetActive(false);
                    clearfowardQuestImg.SetActive(false);
                    ingImg.SetActive(false);
                    completedBtn.SetActive(false);
                    endBtn.SetActive(true);
                }
            }
        }
        else if (n > dataMgrDontDestroy.QuestIdx)
        {
            if (n - dataMgrDontDestroy.QuestIdx == 1)
            {
                if(n==1 && dataMgrDontDestroy.QuestIdx == 0)
                {
                    acceptBtn.SetActive(true);
                    clearfowardQuestImg.SetActive(false);
                    ingImg.SetActive(false);
                    completedBtn.SetActive(false);
                    endBtn.SetActive(false);
                }
                else
                {
                    if (dataMgrDontDestroy.IsDoing == false)
                    {
                        acceptBtn.SetActive(true);
                        clearfowardQuestImg.SetActive(false);
                        ingImg.SetActive(false);
                        completedBtn.SetActive(false);
                        endBtn.SetActive(false);
                    }
                    else
                    {
                        acceptBtn.SetActive(false);
                        clearfowardQuestImg.SetActive(true);
                        ingImg.SetActive(false);
                        completedBtn.SetActive(false);
                        endBtn.SetActive(false);
                    }
                }
            }
            else
            {
                acceptBtn.SetActive(false);
                clearfowardQuestImg.SetActive(true);
                ingImg.SetActive(false);
                completedBtn.SetActive(false);
                endBtn.SetActive(false);
            }
        }
        else
        {
            acceptBtn.SetActive(false);
            clearfowardQuestImg.SetActive(false);
            ingImg.SetActive(false);
            completedBtn.SetActive(false);
            endBtn.SetActive(true);
        }
    }
    public void AcceptBtn()
    {
        dataMgrDontDestroy.QuestIdx++;
        dataMgrDontDestroy.QuestCurCnt = 0;
        dataMgrDontDestroy.IsDoing = true; // 나중에 싱글톤으로 보내야

        string json = txtFile.text;
        var jsonData = JSON.Parse(json);
        int item = dataMgrDontDestroy.QuestIdx - 1;

        if (jsonData["Quest"][item]["Goal"] != null)
        {
            dataMgrDontDestroy.GoalTxt = jsonData["Quest"][item]["Goal"];
        }
        else
        {
            // 에러 처리 또는 기본값 할당
            dataMgrDontDestroy.GoalTxt = "Default Goal Text";
        }
        dataMgrDontDestroy.QuestMaxCnt = (int)(jsonData["Quest"][item]["Count"]);

        questPopUpManager.UpdateQuestStatus();

        acceptBtn.SetActive(false);
        ingImg.SetActive(true);
    }

    public void CompletedBtn()
    {
        QuestClearReward(dataMgrDontDestroy.QuestIdx);
        dataMgrDontDestroy.IsCompleted = false;
        dataMgrDontDestroy.IsDoing = false;
        questPopUpPanel.SetActive(false);
        endBtn.SetActive(true);
    }

    public void QuestClearReward(int n)
    {
        string json = txtFile.text;
        var jsonData = JSON.Parse(json);

        int item = n - 1; //매개변수

        expPotionReward = (jsonData["Quest"][item]["RewardExp"]);
        materialReward = (jsonData["Quest"][item]["RewardMat"]);
        goldReward = (jsonData["Quest"][item]["RewardGold"]);

        //이때 퀘스트 보상 수령
        dataMgrDontDestroy.UserExpPotion += expPotionReward;
        dataMgrDontDestroy.UserMaterial += materialReward;
        dataMgrDontDestroy.UserGold += goldReward;
    }
}