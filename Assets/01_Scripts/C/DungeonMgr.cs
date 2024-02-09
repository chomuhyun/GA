using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System;
using Photon.Pun;

public class DungeonMgr : MonoBehaviourPunCallbacks
{
    public DataMgrDontDestroy dataMgrDontDestroy;
    public Text discription;

    public int dungeonSort; // 현재 던전 형식(싱글1, 카오스2, 레이드3)
    public int dungeonNum;

    public string single = "Single";
    public string chaos = "Chaos";
    public string raid = "Raid";

    public GameObject singlePanel;
    public GameObject chaosPanel;
    public GameObject raidPanel;

    public GameObject singleContent;
    public GameObject chaosContent;
    public GameObject raidContent;

    TextAsset txtFile; //Jsonfile
    public GameObject singleCell; // Prefab
    public GameObject chaosCell;
    public GameObject raidCell;

    private void Awake()
    {
        var jsonitemFIle = Resources.Load<TextAsset>("Json/DungeonList");
        txtFile = jsonitemFIle;
    }

    private void Start()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        BackDungeon();
        var DunGeonListFile = Resources.Load<TextAsset>("Json/DungeonList");
        txtFile = DunGeonListFile;

        string json = txtFile.text;
        var jsonData = JSON.Parse(json);

        for (int i = 1; i < jsonData["Single"].Count + 1; i++)
        {
            InstSingleDunGeon(i);
        }
        for (int i = 1; i < jsonData["Chaos"].Count + 1; i++)
        {
            InstChaosDunGeon(i); 
        }
        for (int i = 1; i < jsonData["Raid"].Count + 1; i++)
        {
            InstRaidDunGeon(i);
        }
    }

    public void InstSingleDunGeon(int n)
    {
        string json = txtFile.text;
        var jsonData = JSON.Parse(json);

        int idx = n - 1; // 매개변수

        GameObject character = Instantiate(singleCell); // 만들거야

        character.transform.name = jsonData["Single"][idx]["dungeonName"]; // 오브젝트명 정의

        character.GetComponent<CellsInfo_Single>().dungeonIdxNum = (int)(jsonData["Single"][idx]["num"]);
        character.GetComponent<CellsInfo_Single>().dungeonName = (jsonData["Single"][idx]["dungeonName"]);
        character.GetComponent<CellsInfo_Single>().discription = (jsonData["Single"][idx]["Discription"]);
        character.GetComponent<CellsInfo_Single>().difficult = (jsonData["Single"][idx]["difficulty"]);

        character.transform.SetParent(singleContent.transform);
    }

    public void InstChaosDunGeon(int n)
    {
        string json = txtFile.text;
        var jsonData = JSON.Parse(json);

        int idx = n - 1; // 매개변수

        GameObject character = Instantiate(chaosCell);

        character.transform.name = jsonData["Chaos"][idx]["dungeonName"];

        character.GetComponent<CellsInfo_Chaos>().dungeonIdxNum = (int)(jsonData["Chaos"][idx]["num"]);
        character.GetComponent<CellsInfo_Chaos>().dungeonName = (jsonData["Chaos"][idx]["dungeonName"]);
        character.GetComponent<CellsInfo_Chaos>().discription = jsonData["Chaos"][idx]["Discription"];
        character.GetComponent<CellsInfo_Chaos>().difficult = (jsonData["Chaos"][idx]["difficulty"]);

        character.transform.SetParent(chaosContent.transform);
    }
    public void InstRaidDunGeon(int n)
    {
        string json = txtFile.text;
        var jsonData = JSON.Parse(json);

        int idx = n - 1; // 매개변수

        GameObject character = Instantiate(raidCell);

        character.transform.name = jsonData["Raid"][idx]["dungeonName"];

        character.GetComponent<CellsInfo_Raid>().dungeonIdxNum = (int)(jsonData["Raid"][idx]["num"]);
        character.GetComponent<CellsInfo_Raid>().dungeonName = (jsonData["Raid"][idx]["dungeonName"]);
        character.GetComponent<CellsInfo_Raid>().discription = jsonData["Raid"][idx]["Discription"];
        character.GetComponent<CellsInfo_Raid>().difficult = (jsonData["Raid"][idx]["difficulty"]);

        character.transform.SetParent(raidContent.transform);
    }

    public void OnSinglePanel()
    {
        singlePanel.SetActive(true);
        chaosPanel.SetActive(false);
        raidPanel.SetActive(false);
        dungeonSort = 1;
        string single = "혼자와써? 응 싱글이야.";
        discription.text = single;
    }

    public void OnChaosPanel()
    {
        singlePanel.SetActive(false);
        chaosPanel.SetActive(true);
        raidPanel.SetActive(false);
        dungeonSort = 2;
        string chaos = "카오스 던전을 선택하세요.";
        discription.text = chaos;
    }

    public void OnRaidPanel()
    {
        singlePanel.SetActive(false);
        chaosPanel.SetActive(false);
        raidPanel.SetActive(true);
        dungeonSort = 3;
        string raid = "레이드 던전을 선택하세요.";
        discription.text = raid;
    }

    public void BackDungeon()
    {
        singlePanel.SetActive(false);
        chaosPanel.SetActive(false);
        raidPanel.SetActive(false);
        string main = "던전을 선택해.";
        discription.text = main;
    }

    public void MoveDungeon()
    {
        switch (dungeonSort)
        {
            case 1:
                Debug.Log($"현재 선택한 던전은 : single");
                dataMgrDontDestroy.DungeonSortIdx = dungeonSort;
                dataMgrDontDestroy.DungeonNumIdx = dungeonNum;
                Debug.Log($"선택한 던전의 번호는 {dataMgrDontDestroy.DungeonSortIdx} / {dataMgrDontDestroy.DungeonNumIdx}");
                PhotonNetwork.Disconnect();
                //SceneManager.LoadScene(single);
                break;
            case 2:
                Debug.Log($"현재 선택한  : chaos");
                dataMgrDontDestroy.DungeonSortIdx = dungeonSort;
                dataMgrDontDestroy.DungeonNumIdx = dungeonNum;
                Debug.Log($"선택한 던전의 번호는 {dataMgrDontDestroy.DungeonSortIdx} / {dataMgrDontDestroy.DungeonNumIdx}");
                PhotonNetwork.Disconnect();
                //SceneManager.LoadScene(chaos);
                break;
            case 3:
                Debug.Log($"현재 선택한  : raid");
                dataMgrDontDestroy.DungeonSortIdx = dungeonSort;
                dataMgrDontDestroy.DungeonNumIdx = dungeonNum;
                Debug.Log($"선택한 던전의 번호는 {dataMgrDontDestroy.DungeonSortIdx} / {dataMgrDontDestroy.DungeonNumIdx}");
                //SceneManager.LoadScene(raid);
                break;
            default:
                break;
        }
    }
}