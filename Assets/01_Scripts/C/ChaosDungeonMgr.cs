using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MageMiddleBoss;


public class ChaosDungeonMgr : MonoBehaviour
{
    public DataMgrDontDestroy dataMgr;


    public int bossKilled;


    public GameObject door11;
    public GameObject door12;
    public GameObject door21;
    public GameObject door22;

    public int cDungeonStep; 

    public bool isBattle;           //전투 시 문 닫힘.

    public GameObject reset;        //떨어지면 리셋되는 플레이어
    public GameObject[] bossPrefab; //bossPrefab[0]은 빈칸
    public GameObject[] mobPrefab;
    public Transform[] spawnPoint;
    public Transform[] mobSpawnPoint;

    public GameObject midBossEffect;
    public GameObject endBossEffect;


    private void Start()
    {
        if(dataMgr != null)
        {
            cDungeonStep = DataMgrDontDestroy.Instance.DungeonNumIdx;
        }
    }

    #region 1보스 소환
    public void InstBoss1()
    {
        StartCoroutine(MakeBoss1());
        StartCoroutine(MakeMob1());

        //Door();
    }

    IEnumerator MakeBoss1()
    {
        Instantiate(midBossEffect, spawnPoint[1]); // 이펙트 생성
        GameObject bossnem1 = Instantiate(bossPrefab[1], spawnPoint[1]); // 보스 생성
        yield return new WaitForSeconds(0.5f);
        bossnem1.GetComponent<StateManager>().maxhp *= cDungeonStep;
        bossnem1.GetComponent<StateManager>().hp *= cDungeonStep;
        bossnem1.GetComponent<StateManager>().attackPower += (cDungeonStep * 30);
    }
    IEnumerator MakeMob1()
    {
        GameObject mob1 = Instantiate(mobPrefab[1], mobSpawnPoint[1]);
        yield return new WaitForSeconds(0.5f);
        mob1.GetComponentInChildren<StateManager>().maxhp *= cDungeonStep;
        mob1.GetComponentInChildren<StateManager>().hp *= cDungeonStep;
        mob1.GetComponentInChildren<StateManager>().attackPower += (cDungeonStep * 30);
    }

    #endregion

    #region 2보스 소환
    public void InstBoss2()
    {
        StartCoroutine(MakeBoss2());
        StartCoroutine(MakeMob2());
    }
    IEnumerator MakeBoss2()
    {
        Instantiate(midBossEffect, spawnPoint[2]); // 이펙트 생성
        GameObject bossnem1 = Instantiate(bossPrefab[2], spawnPoint[2]); // 보스 생성
        yield return new WaitForSeconds(0.5f);
        bossnem1.GetComponent<StateManager>().maxhp *= cDungeonStep;
        bossnem1.GetComponent<StateManager>().hp *= cDungeonStep;
        bossnem1.GetComponent<StateManager>().attackPower += (cDungeonStep * 30);
    }
    IEnumerator MakeMob2()
    {
        GameObject mob1 = Instantiate(mobPrefab[2], mobSpawnPoint[2]);
        yield return new WaitForSeconds(0.5f);
        mob1.GetComponentInChildren<StateManager>().maxhp *= cDungeonStep;
        mob1.GetComponentInChildren<StateManager>().hp *= cDungeonStep;
        mob1.GetComponentInChildren<StateManager>().attackPower += (cDungeonStep * 30);
    }
    #endregion

    #region 3보스 소환
    public void InstBoss3()
    {
        StartCoroutine(MakeBoss3());
        StartCoroutine(MakeMob3());
    }

    IEnumerator MakeBoss3()
    {
        Instantiate(midBossEffect, spawnPoint[3]); // 이펙트 생성
        GameObject bossnem1 = Instantiate(bossPrefab[3], spawnPoint[3]); // 보스 생성
        yield return new WaitForSeconds(0.5f);
        bossnem1.GetComponent<StateManager>().maxhp *= cDungeonStep *3;
        bossnem1.GetComponent<StateManager>().hp *= cDungeonStep *3;
        bossnem1.GetComponent<StateManager>().attackPower += (cDungeonStep * 30);
    }
    IEnumerator MakeMob3()
    {
        GameObject mob1 = Instantiate(mobPrefab[3], mobSpawnPoint[3]);
        yield return new WaitForSeconds(0.5f);
        mob1.GetComponentInChildren<StateManager>().maxhp *= cDungeonStep;
        mob1.GetComponentInChildren<StateManager>().hp *= cDungeonStep;
        mob1.GetComponentInChildren<StateManager>().attackPower += (cDungeonStep * 30);
    }
    #endregion



    public void ClearMidBoss() //중간 보스잡을때마다 호출해야함.
    {
        isBattle = false;
        Door();
    }


    public void Door() //문마다 달려있음. 싸움 시작하거나 끝날 시 호출 필요.
    {
        
        Jun_TweenRuntime[] gameObject1 = door11.GetComponents<Jun_TweenRuntime>();
        Jun_TweenRuntime[] gameObject2 = door12.GetComponents<Jun_TweenRuntime>();

        if (isBattle == true)
        {
            gameObject1[0].Play(); // 열리기
            gameObject2[0].Play(); // 열리기
            isBattle = false;
        }
        else
        {
            gameObject1[1].Play(); // 닫히기
            gameObject2[1].Play(); // 열리기
            isBattle = true;
        }
    }

    

    public void ClearEndBoss()
    {
        //clearPanel.SetActive(true);
    }

    public void Update()
    {
        //#region
        //if (reset.transform.position.y < -7f)
        //    ResetPlayer();
        //#endregion
        //if (boss1.GetComponent<MageMiddleBoss>().die)
        //{
        //    bossKilled = 1;
        //}


    }

    public void ResetPlayer()
    {
        reset.transform.position = spawnPoint[0].position;
    }

}
