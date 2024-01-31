using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayWeekData : MonoBehaviour
{
    public ImageList imagelist;
    public InventoryManager inventoryMgr;

    //파일이 생성 후 오브젝트가 담을 정보들

    [Header("퀘스트 이름, 조건")]
    public string trophyName; //메인창에 얻었다고 팝업
    public string goalName; //업적 조건
    public int curGoal = 0;
    public int goal;
    public GameObject getEndBtn;


    [Header("퀘스트 조건만족시")] //얻는거
    public int rewardItem; //얻는 아이템, 아이템 얻으면 숫자를 태그로 받아서 아이템 획득
    public int rewardCount; //얻는 아이템 개수

    [Header("얻는 칭호")]
    public string styleName;

    

    private void Start()
    {
        inventoryMgr = GameObject.Find("InventoryMgr").GetComponent<InventoryManager>();
        imagelist = GameObject.Find("ImageList").GetComponent<ImageList>();

        gameObject.transform.GetChild(2).GetComponent<Text>().text = trophyName;
        gameObject.transform.GetChild(3).GetComponent<Text>().text = goalName;
        gameObject.transform.GetChild(4).GetComponent<Image>().sprite = imagelist.meterialsImage[rewardItem];
        gameObject.transform.GetChild(5).GetComponent<Text>().text = rewardCount.ToString();
        gameObject.transform.GetChild(6).GetComponent<Text>().text = $"{curGoal} / {goal}";
        getEndBtn = gameObject.transform.GetChild(8).gameObject;
        getEndBtn.SetActive(false);
    }

    public void InitAcheive()
    {
        if(curGoal >= goal)
        {
            gameObject.transform.GetChild(7).gameObject.SetActive(true);
        }
        gameObject.transform.GetChild(6).GetComponent<Text>().text = $"{curGoal} / {goal}";
        
    }


    public void GetRewardBtn() // 보상 수령 버튼
    {
        //얻었다 창 팝업
        if(rewardItem == 2) //2번은 재료
        {
            inventoryMgr.materials += rewardCount;
        }
        if (rewardItem == 3) //3번은 아이템
        {
            inventoryMgr.gold += rewardCount;
        }
        inventoryMgr.InitInventory();
        getEndBtn.SetActive(true);

    }
}

