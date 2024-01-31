using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CellsInfo_Chaos : MonoBehaviour
{
    public DungeonMgr dungeonManager;

    public int dungeonIdxNum;
    public string dungeonName;
    public string difficult;
    public string discription;

    public Image bossFace;
    public Text dungeonNameTxt;
    public Text difficultTxt;
    public Text discriptionTxt;

    void Start()
    {
        discriptionTxt = GameObject.Find("DungeonDescription").GetComponent<Text>();
        dungeonManager = GameObject.Find("DungeonMgr").GetComponent<DungeonMgr>();
        dungeonNameTxt = gameObject.transform.GetChild(2).GetComponent<Text>();
        difficultTxt = gameObject.transform.GetChild(3).GetComponent<Text>();
        dungeonNameTxt.text = dungeonName;
        difficultTxt.text = difficult;
    }

    public void OnChaosBtn()
    {
        dungeonManager.discription.text = discription;
        dungeonManager.dungeonNum = dungeonIdxNum;
    }

}
