using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardMgr : MonoBehaviour
{
    public DataMgrDontDestroy dataMgrDontDestroy;

    public GameObject rewardContent;
    public GameObject itemPrefab;
    public Sprite[] imageList; //0번은 material, 1번은 expPotion, 2번은 gold

    public int expPotionReward;
    public int materialReward;
    public int goldReward;

    //보상=(재화 종류) * 던전의 번호? 1-2, 1-3의 2랑 3을 재화종류에 곱해주는 이런식(일단 이렇게 설정만 해놓음 필요시 수정)
    //던전의 종류에 따라 리워드를 설정하고, 난이도에따라(더 높은단계의 던전이면) 그만큼 리워드에 곱해준다.

    public void SendInventory()
    {

    }
    public void SoloClearReward()
    {
        expPotionReward = 3;
        materialReward = 100;

        expPotionReward *= dataMgrDontDestroy.dungeonNumIdx;
        materialReward *= dataMgrDontDestroy.dungeonNumIdx;
        //InstExp(expPotionReward); 클리어 패널이 열렸을때 호출
        //InstMaterial(materialReward); ""
    }
    public void ChaosClearReward()
    {
        expPotionReward = 5;
        materialReward = 200;

        expPotionReward *= dataMgrDontDestroy.dungeonNumIdx;
        materialReward *= dataMgrDontDestroy.dungeonNumIdx;
        InstExp(expPotionReward);
        InstMaterial(materialReward);
    }
    public void RaidClearReward()
    {
        expPotionReward = 10;
        goldReward = 1000;

        expPotionReward *= dataMgrDontDestroy.dungeonNumIdx;
        goldReward *= dataMgrDontDestroy.dungeonNumIdx;
        InstExp(expPotionReward);
        InstGold(goldReward);
    }
    
    //경험치 물약 소환 함수
    public void InstExp(int itemcount)
    {
        GameObject character = Instantiate(itemPrefab);

        character.GetComponent<Image>().sprite = imageList[1];
        character.GetComponentInChildren<Text>().text = expPotionReward.ToString();
        //character.tag = "Exp";
        character.transform.SetParent(rewardContent.transform);
        //dataMgrDontDestroy.UserExpPotion += expPotionReward;
    }

    // 재료소환 함수
    public void InstMaterial(int itemcount)
    {
        GameObject character = Instantiate(itemPrefab);

        character.GetComponent<Image>().sprite = imageList[0];
        character.GetComponentInChildren<Text>().text = materialReward.ToString();
        //character.tag = "Material";
        character.transform.SetParent(rewardContent.transform);
        //dataMgrDontDestroy.UserMaterial += materialReward;
    }

    // 골드 소환 함수
    public void InstGold(int itemcount)
    {
        GameObject character = Instantiate(itemPrefab);

        character.GetComponent<Image>().sprite = imageList[2];
        character.GetComponentInChildren<Text>().text = goldReward.ToString();
        //character.tag = "Gold";
        character.transform.SetParent(rewardContent.transform);
        //dataMgrDontDestroy.UserGold += goldReward;
    }
}