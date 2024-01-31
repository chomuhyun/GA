using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownTeleportMgr : MonoBehaviour
{
    public GameObject chHouseMain;
    public GameObject bossPrefab;

    void Start()
    {
        chHouseMain = GameObject.Find("ChHouseMain");
        bossPrefab = GameObject.FindWithTag("Boss");
    }

    public void VSBoss()
    {
        chHouseMain.transform.position = new Vector3(-41.5f, 50f, -60);
        bossPrefab.SetActive(true);
    }

    public void HappyGround()
    {
        chHouseMain.transform.position = new Vector3(3.8685f, 4.2894f, 32.388f);
        bossPrefab.SetActive(false);
    }
}
