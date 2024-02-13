using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardMgr : MonoBehaviour
{
    public DataMgrDontDestroy dataMgrDontDestroy;

    public GameObject rewardPanel;
    public GameObject movePanel;
    public GameObject defeatPanel;
    public GameObject rewardContent;
    public GameObject itemPrefab;
    public Sprite[] imageList; //0번은 material, 1번은 expPotion, 2번은 gold
    public int DgSortIdx;
    public int expPotionReward; 
    public int materialReward;

    public int goldReward;
    public int clearCut = 1;
    //보상=(재화 종류) * 던전의 번호? 1-2, 1-3의 2랑 3을 재화종류에 곱해주는 이런식(일단 이렇게 설정만 해놓음 필요시 수정)
    //던전의 종류에 따라 리워드를 설정하고, 난이도에따라(더 높은단계의 던전이면) 그만큼 리워드에 곱해준다.

    private void Start()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        DgSortIdx = dataMgrDontDestroy.DungeonSortIdx;
        rewardPanel.SetActive(false);
        movePanel.SetActive(false);
    }

    private void Update()
    {
        playerDieful();
    }
    public void ShowReward()
    {
        rewardPanel.SetActive(true);
        movePanel.SetActive(true);
        switch (DgSortIdx)
        {
            case 1:
                SoloClearReward();
                break;
            case 2:
                ChaosClearReward();
                break;
            case 3:
                RaidClearReward();
                break;
            default:
                break;
        }
    }

    public void playerDieful()
    {
        if(dataMgrDontDestroy.playerDie)
        {
            movePanel.SetActive(true);  
            defeatPanel.SetActive(true);
        }
    }
        
    public void OnClickReceiveBtn()
    {
        clearCut = 0;
        dataMgrDontDestroy.UserExpPotion += expPotionReward;
        dataMgrDontDestroy.UserMaterial += materialReward;
        dataMgrDontDestroy.UserGold += goldReward;
        rewardPanel.SetActive(false);
    }

    #region 클리어보상 설정
    public void SoloClearReward()
    {
        expPotionReward = 3;
        materialReward = 100;

        expPotionReward *= dataMgrDontDestroy.dungeonNumIdx;
        materialReward *= dataMgrDontDestroy.dungeonNumIdx;
        InstExp(expPotionReward);
        InstMaterial(materialReward);
    }

    public void ChaosClearReward()
    {
        expPotionReward = 500;
        materialReward = 300;

        expPotionReward *= dataMgrDontDestroy.dungeonNumIdx;
        materialReward *= dataMgrDontDestroy.dungeonNumIdx;
        InstExp(expPotionReward);
        InstMaterial(materialReward);
    }

    public void RaidClearReward()
    {
        expPotionReward = 1000;
        goldReward = 15000;

        expPotionReward *= dataMgrDontDestroy.dungeonNumIdx;
        goldReward *= dataMgrDontDestroy.dungeonNumIdx;
        InstExp(expPotionReward);
        InstGold(goldReward);
    }
    #endregion

    #region 스크롤뷰 Content에 보상 보이게하기
    public void InstMaterial(int itemcount)
    {
        GameObject character = Instantiate(itemPrefab);

        character.transform.GetChild(0).GetComponent<Image>().sprite = imageList[0];
        character.GetComponentInChildren<Text>().text = itemcount.ToString();
        character.transform.SetParent(rewardContent.transform);
    }

    public void InstExp(int itemcount)
    {
        GameObject character = Instantiate(itemPrefab);

        character.transform.GetChild(0).GetComponent<Image>().sprite = imageList[1];
        character.GetComponentInChildren<Text>().text = itemcount.ToString();
        character.transform.SetParent(rewardContent.transform);
    }

    public void InstGold(int itemcount)
    {
        GameObject character = Instantiate(itemPrefab);

        character.transform.GetChild(0).GetComponent<Image>().sprite = imageList[2];
        character.GetComponentInChildren<Text>().text = itemcount.ToString();
        character.transform.SetParent(rewardContent.transform);
    }
    #endregion
}