using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPopUpManager : MonoBehaviour
{

    public int curQuestIndex; //현재 퀘스트 번호

    public Text questCountTxt;

    public string goal;
    public int curCount;
    public int maxCount;

    public bool isCompleted;

    private void Start()
    {
        questCountTxt = GameObject.Find("QCountTxt").GetComponent<Text>();
    }


    public enum QuestCondition
    {
        Talk = 1,
        NormalMonKill,
        BossKill,
        GotGold,
        Buy
    }

    private QuestCondition qcondition;

    public void QuestIndexUp(int n) //퀘스트 달성도 증가
    {
        qcondition = (QuestCondition)n; // 위의 이넘값을 1, 2 로 쓸거야
        if (curQuestIndex == n)
        { curCount++;}

        InitCurQuest();
    }

    public void InitCurQuest() //글씨 초기화
    {
        questCountTxt.text = $"({curCount} /  {maxCount} )";

        if(curCount >= maxCount)
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



