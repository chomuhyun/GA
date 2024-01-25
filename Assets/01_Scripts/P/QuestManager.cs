using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleJSON;
public class QuestManager : MonoBehaviour
{
    public TextAsset txtFile; //Jsonfile
    public GameObject jsonObject; //�Ƚᵵ ��

    public GameObject questCanvas;
    public Text questNameTxt;
    public Text goalNameTxt;
    public Text countTxt;
    public Image questRewards;
    public GameObject descriptionPanel;

    public int acceptIdx;

    [Header("����Ʈ�˾�")]
    public GameObject questPopUpPanel;
    public Text questGoalTxt;
    public Text questCountTxt;
    public int questCurCount;
    //public int questMaxCount;

    //[Header("����Ʈ ������ ǥ��")]
    //public Image 


    //Player enterPlayer;

    public void Enter(Player player)
    {
        //enterPlayer = player;
        //uiGroup.anchoredPosition = Vector3.zero;
    }
    private void Awake()
    {
        questNameTxt = GameObject.Find("questNameTxt").GetComponent<Text>();
        goalNameTxt = GameObject.Find("goalNameTxt").GetComponent<Text>();
        countTxt = GameObject.Find("countTxt").GetComponent<Text>();
        questRewards = GameObject.Find("QuestRewards").GetComponent<Image>();
        questPopUpPanel = GameObject.Find("QuestPanel");
        questGoalTxt = GameObject.Find("GoalTxt").GetComponent<Text>();
        questCountTxt = GameObject.Find("CountTxt").GetComponent<Text>();
        descriptionPanel.SetActive(false);
    }
    void Start()
    {


    }


    public void InstQuest(int n)
    {
        string json = txtFile.text;
        var jsonData = JSON.Parse(json); //var�� �ǹ�: Unity���� ������ �ٰ����´�.

        int item = n-1; //�Ű�����

        //GameObject character = Instantiate(jsonObject);


        questNameTxt.text = (jsonData["Quest"][item]["QuestName"]);
        goalNameTxt.text = (jsonData["Quest"][item]["Goal"]);
        countTxt.text = (jsonData["Quest"][item]["Count"]);
        acceptIdx = n;

        #region
        //character.transform.name = (jsonData["��Ʈ1"][n]["QuestName"]);
        //character.GetComponent<QuestData>().charname = (jsonData["��Ʈ1"][n]["QuestName"]);
        //character.GetComponent<QuestData>().atk = (int)(jsonData["��Ʈ1"][n]["Count"]);
        ////character.GetComponent<QuestData>().count++; //QuestData�� ī��Ʈ ����

        //character.tag = "Player"; //prefab�� �±׸� �ްž�.

        //character.transform.SetParent(questCanvas.transform); //���� questCanvas�� �θ�� �ΰ� �����ϰ� Prefab�� �¾.
        #endregion
    }

    public void AcceptQuestBtn()
    {
        ReceiveQuest(acceptIdx);
    }
    public void ReceiveQuest(int n)
    {
        Debug.Log("Tlqkf");
        string json = txtFile.text;
        var jsonData = JSON.Parse(json); //var�� �ǹ�: Unity���� ������ �ٰ����´�.
        int item = n-1;

        questGoalTxt.text = (jsonData["Quest"][item]["Goal"]);
        questCountTxt.text = $"({questCurCount} / {(jsonData["Quest"][item]["Count"])})";
    }
}
