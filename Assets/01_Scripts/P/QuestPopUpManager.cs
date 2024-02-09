using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPopUpManager : MonoBehaviour
{
    public DataMgrDontDestroy dataMgr;
    public int questIndex; //현재 퀘스트 번호
    public Text questCountTxt;
    public Text goalTxt;

    public string goalInfo;
    public int curCnt;
    public int maxCnt;

    public int questMaxCnt { get; set; }
    public int questCurCnt { get; set; }
    public int questIdx { get; set; }

    public bool isCompleted;

    public void UpdateQuestStatus(string goals, int curCnts, int maxCnts)
    {
        goalInfo = goals;
        curCnt = curCnts;
        maxCnt = maxCnts;

        goalTxt.text = goalInfo;
        questCurCnt = curCnt;
        questMaxCnt = maxCnt;
        questCountTxt.text = $"({curCnt} / {maxCnt})";
    }
    private void Start()
    {
        dataMgr = DataMgrDontDestroy.Instance;

        questCountTxt = GameObject.Find("QCountTxt").GetComponent<Text>();
        questIndex = dataMgr.questIdx;
        goalInfo = dataMgr.GoalTxt;
        curCnt = dataMgr.QuestCurCnt;
        maxCnt = dataMgr.QuestMaxCnt;
    }

    //private QuestCondition qcondition;

    public void QuestIndexUp(int n) //퀘스트 달성도 증가
    {
        //qcondition = (QuestCondition)n; // 위의 이넘값을 1, 2 로 쓸거야
        if (dataMgr.questIdx == n)
        {
            curCnt++;
            dataMgr.QuestCurCnt = curCnt; // 데이터 매니저에 현재 퀘스트 달성도 업데이트
        }
        InitCurQuest();
    }

    public void InitCurQuest() //글씨 초기화
    {
        questCountTxt.text = $"({curCnt} /  {maxCnt} )";

        if (curCnt >= maxCnt)
        {
            questCountTxt.color = Color.yellow;
            isCompleted = true;
        }
        else
        {
            questCountTxt.color = Color.white;
        }
    }


}