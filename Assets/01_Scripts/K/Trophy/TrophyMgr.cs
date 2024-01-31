using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON; //########################댕겨와야행

public class TrophyMgr : MonoBehaviour
{
    #region 싱글톤
    private static TrophyMgr instance;
    public static TrophyMgr Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<TrophyMgr>();
            }
            return instance;
        }
    }
    #endregion

    public TextAsset txtFile; //Jsonfile
    public GameObject jsonObjectD;
    public GameObject jsonObjectW;
    public GameObject jsonObject; //Prefab (Json char 달린)

    public GameObject dailyContent;
    public GameObject weekContent;
    public GameObject achievementContent; // 이거 리소스폴더에서 안받아져서 그냥 껴넣음 //TODO:


    public GameObject trophyPanel; //TODO: 지금 p로 열지만 npc생기면 지움!!

    public GameObject dailyPanel;
    public GameObject weekPanel;
    public GameObject achievementPanel;
    public bool isTrophy; //TODO: npc생기면 지움!!

    private void Awake()
    {
        trophyPanel = GameObject.Find("TrophyCanvas"); //TODO: npc생기면 지움!!



        var jsonitemFile = Resources.Load<TextAsset>("Json/TrophyTable");
        txtFile = jsonitemFile;

        dailyContent = GameObject.Find("DailyContent");
        weekContent = GameObject.Find("WeekContent");
        achievementContent = GameObject.Find("AchievementContent");

        dailyPanel = GameObject.Find("DailyPanel");
        weekPanel = GameObject.Find("WeekPanel");
        achievementPanel = GameObject.Find("AchievementPanel");
        


        string json = txtFile.text;
        var jsonData = JSON.Parse(json);
        for (int i = 1; i < jsonData["DailyA"].Count + 1; i++)
        {
            InstDailyContent(i);
        }
        for (int i = 1; i < jsonData["WeekA"].Count + 1; i++)
        {
            InstWeekContent(i);
        }
        for (int i = 1; i < jsonData["Achievement"].Count + 1; i++)
        {
            InstachievementContent(i);
        }
    }
    private void Start()
    {


    }

    public void InstDailyContent(int n)
    {

        string json = txtFile.text;
        var jsonData = JSON.Parse(json);


        int item = n - 1; // 매개변수


        GameObject character = Instantiate(jsonObjectD); // 만들거야

        character.transform.name = jsonData["DailyA"][item]["GoalName"]; // 오브젝트명 정의

        character.GetComponent<DayQData>().trophyName = (jsonData["DailyA"][item]["TrophyName"]);
        character.GetComponent<DayQData>().goalName = jsonData["DailyA"][item]["GoalName"];
        character.GetComponent<DayQData>().goal = (int)(jsonData["DailyA"][item]["Goal"]);

        character.GetComponent<DayQData>().rewardItem = (int)(jsonData["DailyA"][item]["RewardItem"]);
        character.GetComponent<DayQData>().rewardCount = (int)(jsonData["DailyA"][item]["RewardCount"]);


        character.transform.SetParent(dailyContent.transform);

        character.transform.GetChild(7).gameObject.SetActive(false);
        } //일일퀘스트 만들기
    public void InstWeekContent(int n)
    {

        string json = txtFile.text;
        var jsonData = JSON.Parse(json);


        int item = n - 1; // 매개변수


        GameObject character = Instantiate(jsonObjectW); // 만들거야

        character.transform.name = jsonData["WeekA"][item]["GoalName"]; // 오브젝트명 정의

        character.GetComponent<WeekQData>().trophyName = (jsonData["WeekA"][item]["TrophyName"]);
        character.GetComponent<WeekQData>().goalName = jsonData["WeekA"][item]["GoalName"];
        character.GetComponent<WeekQData>().goal = (int)(jsonData["WeekA"][item]["Goal"]);

        character.GetComponent<WeekQData>().rewardItem = (int)(jsonData["WeekA"][item]["RewardItem"]);
        character.GetComponent<WeekQData>().rewardCount = (int)(jsonData["WeekA"][item]["RewardCount"]);


        character.transform.SetParent(weekContent.transform);

        character.transform.GetChild(7).gameObject.SetActive(false);

    } //주간퀘스트 만들기
    public void InstachievementContent(int n)
    {

        string json = txtFile.text;
        var jsonData = JSON.Parse(json);


        int item = n-1; // 매개변수


        GameObject character = Instantiate(jsonObject); // 만들거야

        character.transform.name = jsonData["Achievement"][item]["GoalName"]; // 오브젝트명 정의
        
        character.GetComponent<TrophyJsonData>().trophyName = (jsonData["Achievement"][item]["TrophyName"]);
        character.GetComponent<TrophyJsonData>().goalName = jsonData["Achievement"][item]["GoalName"];
        character.GetComponent<TrophyJsonData>().goal = (int)(jsonData["Achievement"][item]["Goal"]);
      
        character.GetComponent<TrophyJsonData>().rewardItem = (int)(jsonData["Achievement"][item]["RewardItem"]);
        character.GetComponent<TrophyJsonData>().rewardCount = (int)(jsonData["Achievement"][item]["RewardCount"]);

        character.GetComponent<TrophyJsonData>().styleName = (jsonData["Achievement"][item]["StyleName"]);


        character.transform.SetParent(achievementContent.transform);

        character.transform.GetChild(7).gameObject.SetActive(false);

    } //업적 만들기





    public enum ConditionCode
    {
        Lvup = 1, // 레벨달성
        Enforced, //2 강화시도
        EnforceS, //3 강화성공
        EnforceF, //4 강화실패
        GoldSpend, //5 골드소모
        SingleMonDead, //6 일반몬스터 처치
        RaidBossDead, //7 레이드몹 사망
        HiddenMonDead, //8 히든몬스터 처치
        NoDeadinRaid, //9 레이드에서 안죽음
        End
    }
    private ConditionCode conditionCode;


            //싱글톤 사용은 TrophyMgr.Instance.TrophyIndexUp(int n);으로 쓴다!!!
    public void TrophyIndexUp(int n) // 싱글톤으로 아무대서나 땡겨서 매개변수로 업적 해금 조건대입
    {
        conditionCode= (ConditionCode)n; 

        switch (conditionCode)
        {
            case ConditionCode.Lvup:
                achievementContent.transform.Find("플레이어 Lv.10 달성하기").GetComponent<TrophyJsonData>().curGoal++;
                achievementContent.transform.Find("플레이어 Lv.10 달성하기").GetComponent<TrophyJsonData>().InitAcheive();
                achievementContent.transform.Find("플레이어 Lv.40 달성하기").GetComponent<TrophyJsonData>().curGoal++;
                achievementContent.transform.Find("플레이어 Lv.40 달성하기").GetComponent<TrophyJsonData>().InitAcheive();
                break;
            case ConditionCode.Enforced:// 강화 누를때마다 int 2 넣어서 호출하기!!
                achievementContent.transform.Find("강화 100번하기").GetComponent<TrophyJsonData>().curGoal++;
                achievementContent.transform.Find("강화 100번하기").GetComponent<TrophyJsonData>().InitAcheive();
                break;
            case ConditionCode.EnforceS:
                break;
            case ConditionCode.EnforceF:
                break;
            case ConditionCode.GoldSpend:
                break;
            case ConditionCode.SingleMonDead:
                break;
            case ConditionCode.RaidBossDead:
                break;
            case ConditionCode.HiddenMonDead:
                break;
            case ConditionCode.NoDeadinRaid:
                break;
            case ConditionCode.End:
                break;
            default:
                break;
        }

    }


    private void Update() //TODO: 나중에 npc로 바꿈!!!!!! p 눌러서 업적 열기
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isTrophy)
            {
                trophyPanel.SetActive(true);
                isTrophy = true;
            }
            else
            {
                trophyPanel.SetActive(false);
                isTrophy = false;
            }
        }

    }




    public void DailyQBtn()
    {
        dailyPanel.SetActive(true);
        weekPanel.SetActive(false) ;
        achievementPanel.SetActive(false);
    }
    public void WeekQBtn()
    {
        dailyPanel.SetActive(false);
        weekPanel.SetActive(true);
        achievementPanel.SetActive(false);
    }
    public void AchieveBtn()
    {
        dailyPanel.SetActive(false);
        weekPanel.SetActive(false);
        achievementPanel.SetActive(true);
    }
    public void ChangeTrophy(int n) //1일퀘, 2주퀘, 3업적 안쓸지도
    {
        string json = txtFile.text;
        var jsonData = JSON.Parse(json);


        int item = n - 1; // 매개변수
    } 
}
