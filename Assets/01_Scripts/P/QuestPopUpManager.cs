using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPopUpManager : MonoBehaviour
{
    public DataMgrDontDestroy dataMgr;
    public Text questCountTxt;
    public Text goalTxt;

    public string goalInfo;
    public int questIdx;
    public int curCnt;
    public int maxCnt;

    public bool isCompleted;

    public void UpdateQuestStatus()
    {
        goalInfo = dataMgr.GoalTxt;
        questIdx = dataMgr.QuestIdx;
        curCnt = dataMgr.QuestCurCnt;
        maxCnt = dataMgr.QuestMaxCnt;

        goalTxt.text = goalInfo;
        questCountTxt.text = $"({curCnt} / {maxCnt})";

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
    private void Start()
    {
        dataMgr = DataMgrDontDestroy.Instance;

        questCountTxt = GameObject.Find("QCountTxt").GetComponent<Text>();
        goalInfo = dataMgr.GoalTxt;
        questIdx = dataMgr.QuestIdx;
        curCnt = dataMgr.QuestCurCnt;
        maxCnt = dataMgr.QuestMaxCnt;
    }
}