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

    public bool isFirst;

    [Header("퀘스트 진행버튼")]
    public GameObject acceptBtn;
    public GameObject ingImg;
    public GameObject completedBtn;
    public GameObject endBtn;

    [Header("퀘스트 현재 진행도 창")]
    public GameObject questPopUpPanel;
    //public bool questPopUpPanelVisible;
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

    
    public void QuestClearReward(int n)
    {
        string json = txtFile.text;
        var jsonData = JSON.Parse(json);

        int item = n - 1; //매개변수

        expPotionReward=(jsonData["Quest"][item]["RewardExp"]);
        materialReward=(jsonData["Quest"][item]["RewardMat"]);
        goldReward=(jsonData["Quest"][item]["RewardGold"]);

        //이때 퀘스트 보상 수령.
        dataMgrDontDestroy.UserExpPotion += expPotionReward;
        dataMgrDontDestroy.UserMaterial += materialReward;
        dataMgrDontDestroy.UserGold += goldReward;
    }

    private void Awake()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        questPopUpManager = GameObject.Find("QuestPopUp").GetComponent<QuestPopUpManager>();
        descriptionPanel.SetActive(false);
    }
    private void Start()
    {
        //ingBtn.SetActive(false);
        completedBtn.SetActive(false);
        ingImg.SetActive(false);
        endBtn.SetActive(false);

        questPanel.SetActive(false);
        questPopUpPanel.SetActive(false);
        questPopUpManager.questIdx = dataMgrDontDestroy.QuestIdx;
        questGoalTxt = dataMgrDontDestroy.GoalTxt;
        questCurCnt = dataMgrDontDestroy.QuestCurCnt;
        questMaxCnt = dataMgrDontDestroy.QuestMaxCnt;
        nPCConversation.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("정상동작");
            nextBtn.GetComponent<TalkMgr>();
            Debug.Log("### TalkMgr 정상동작 ###");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("충돌일어남");
                questPanel.SetActive(true);
                QuestCompletedCheck();
                nPCConversation.SetActive(true);
                if (isFirst)
                {
                    dialogueTrigger.Trigger();
                }
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

        dataMgrDontDestroy.questIdx = n;
        questNameTxt.text = (jsonData["Quest"][item]["QuestName"]);
        goalTxt.text = (jsonData["Quest"][item]["Goal"]);
        rewardExp.text = (jsonData["Quest"][item]["RewardExp"]);
        rewardMat.text = (jsonData["Quest"][item]["RewardMat"]);
        rewardGold.text = (jsonData["Quest"][item]["RewardGold"]);
        Debug.Log("Json 데이터 불러옴");
        
        #region
        //character.transform.name = (jsonData["시트1"][n]["QuestName"]);
        //character.GetComponent<QuestData>().charname = (jsonData["시트1"][n]["QuestName"]);
        //character.GetComponent<QuestData>().atk = (int)(jsonData["시트1"][n]["Count"]);
        ////character.GetComponent<QuestData>().count++; //QuestData의 카운트 증가

        //character.tag = "Player"; //prefab에 태그를 달거야.

        //character.transform.SetParent(questCanvas.transform); //나는 questCanvas를 부모로 두고 응애하고 Prefab이 태어남.
        #endregion
    }

    public void AcceptBtn()
    {
        ReceiveQuest(dataMgrDontDestroy.questIdx);
        isFirst = true;
        //uIMgr.UpdateQuestPopUpInfo(questPopUpManager.questGoalTxt.text, questPopUpManager.questCountTxt.text);
    }

    public void ReceiveQuest(int n)
    {

        string json = txtFile.text;
        var jsonData = JSON.Parse(json);
        int item = n - 1;

        if (jsonData["Quest"][item]["Goal"] != null)
        {
            dataMgrDontDestroy.GoalTxt = jsonData["Quest"][item]["Goal"];
        }
        else
        {
            // 에러 처리 또는 기본값 할당
            dataMgrDontDestroy.GoalTxt = "Default Goal Text";
        }
        dataMgrDontDestroy.questMaxCnt = (int)(jsonData["Quest"][item]["Count"]);
        dataMgrDontDestroy.questIdx = (int)(jsonData["Quest"][item]["QuestNum"]);

        questPopUpManager.UpdateQuestStatus(dataMgrDontDestroy.goalTxt, dataMgrDontDestroy.questCurCnt, dataMgrDontDestroy.questMaxCnt);

        //questPopUpPanelVisible
        acceptBtn.SetActive(false);
        questPopUpPanel.SetActive(true);
        ingImg.SetActive(true);
        
    }

    public void CompletedBtn()
    {
        questPopUpManager.InitCurQuest();

        if (questPopUpManager.isCompleted)
        {
            // 보상 지급 및 처리
            // (보상 관련 코드 추가 필요)
            

            // 퀘스트 버튼 비활성화
            acceptBtn.SetActive(false);
            ingImg.SetActive(false);
            completedBtn.SetActive(false);
            endBtn.SetActive(true);
            // 다음 퀘스트를 선택할 때 수락 버튼이 나오도록 설정
            //acceptBtn.SetActive(true);
            //ingImg.SetActive(false);
            //completedBtn.SetActive(false); 

            //퀘스트 보상 수령하는 함수
            QuestClearReward(dataMgrDontDestroy.questIdx);

            // 퀘스트 인덱스 증가 및 데이터 매니저 갱신
            dataMgrDontDestroy.questIdx++;
            dataMgrDontDestroy.QuestIdx = questPopUpManager.questIdx;

            // 퀘스트 팝업 패널 비활성화
            questPopUpPanel.SetActive(false);

            
            //퀘스트 완료시 동작
            //if (questCurCnt >= questMaxCnt)
            //{

            //}
            //transform.Find("IngImg").gameObject.SetActive(false);
            //transform.Find("CompletedBtn").gameObject.SetActive(true);
        }
       

    }

    public void QuestCompletedCheck()
    {
        if(dataMgrDontDestroy.questCurCnt >= dataMgrDontDestroy.questMaxCnt && isFirst)
        {
            completedBtn.SetActive(true);
        }
    }

    

    //public void CompleteButton()
    //{
            
    //}
}