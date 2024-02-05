using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidDungeonMgr : MonoBehaviour
{
    public int Count = 0;
    public Transform[] spawnPoint;
    public GameObject bossPrefab;
    public GameObject bossMakeEffect;
    public GameObject bossPhaseEffect;

    public GameObject ground_1f;


    public void MakeBoss()
    {
        Count++; //몬스터 스크립에서 죽을때마다 카운트 증가 필요.

        Debug.Log("잔몹 " + Count + "마리 잡음.");
        Debug.Log("보스까지 " + (10 - Count) + "마리 남음.");

        if (Count >= 10)
        {
            Instantiate(bossPrefab, spawnPoint[0]);
            Instantiate(bossMakeEffect, spawnPoint[0]);
        }
    }

    public void BossPhase()
    {
        StartCoroutine(AngryBoss());
    }

    IEnumerator AngryBoss()
    {
        Destroy(ground_1f);
        yield return new WaitForSeconds(1.5f);
        Instantiate(bossPhaseEffect, spawnPoint[1]);
    }

    public void ClearEndBoss()
    {
        //clearPanel.SetActive(true);
    }
}
