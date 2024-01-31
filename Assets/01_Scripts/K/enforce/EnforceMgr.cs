using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class EnforceMgr : MonoBehaviour
{
    public TextAsset forcetxtFile; //Jsonfile

    public InventoryManager inventoryMgr;
    public RewardMgr rewardMgr;
    public TrophyMgr trophyMgr;
    public StateManager stateMgr;
    public Button enBtn;

    [Header("강화하시겠습니까?")]
    public GameObject enforcePanel;
    public Text weaponNowTxt;
    public Text weaponAftTxt;
    public Text wantEnforceTxt;
    public Text needMaterial;
    public Text needGold;

    public GameObject enforceEffect;
    public Jun_TweenRuntime lessTween;

    [Header("재련성공")]
    public GameObject successPanel;
    public Text successweaponNowTxt;
    public Text beforeAtk;
    public Text afterAtk;

    [Header("재련실패")]
    public GameObject failedPanel;
    public Text failweaponNowTxt;
    public Text beforeAtkF;
    public Text afterAtkF;

    [Header("업적클리어용")]
    public int failEnforceCount;


    public int playerWeaponLv;




    private void Awake()
    {
        var jsonitemFile = Resources.Load<TextAsset>("Json/EnforceTable");
        forcetxtFile = jsonitemFile;
        lessTween = GameObject.Find("lessTween").GetComponent<Jun_TweenRuntime>();
        enforcePanel = GameObject.Find("EnforcePanel");
        rewardMgr = GameObject.Find("RewardMgr").GetComponent<RewardMgr>();
        trophyMgr = GameObject.Find("TrophyMgr").GetComponent<TrophyMgr>();
        wantEnforceTxt = GameObject.Find("ReallyTxt").GetComponent<Text>();
        weaponNowTxt = GameObject.Find("ReadyBefore").GetComponent<Text>();
        weaponAftTxt = GameObject.Find("ReadyAfter").GetComponent<Text>();
        needMaterial = GameObject.Find("needMaterial").GetComponent<Text>();
        needGold = GameObject.Find("needGold").GetComponent<Text>();
        enforceEffect = GameObject.Find("EnforceEffect");

        successPanel = GameObject.Find("SuccessPanel");
        successweaponNowTxt = GameObject.Find("ForceLvS").GetComponent<Text>();
        beforeAtk = GameObject.Find("beforeAtk").GetComponent<Text>();
        afterAtk = GameObject.Find("afterAtk").GetComponent<Text>();


        failedPanel = GameObject.Find("FailedPanel");
        failweaponNowTxt = GameObject.Find("ForceLvF").GetComponent<Text>();
        beforeAtkF = GameObject.Find("beforeAtkF").GetComponent<Text>();
        afterAtkF = GameObject.Find("afterAtkF").GetComponent<Text>();
    }
    void Start()
    {

        enforceEffect.SetActive(false);
        successPanel.SetActive(false);
        failedPanel.SetActive(false);
        enforcePanel.SetActive(false);
    }



    public void OnEnforcePanel(int playerWeaponLv) // 창이 열림, 플레이어 웨폰레벨을 받음
    {
        int replace = playerWeaponLv - 1;

        string json = forcetxtFile.text;
        var jsonData = JSON.Parse(json);

        weaponNowTxt.text = $"현재 재련 수치{playerWeaponLv} 단계";
        weaponAftTxt.text = $"현재 재련 수치{playerWeaponLv+ 1} 단계";
        wantEnforceTxt.text = $"강화 하시겠습니까? \n 강화 확률은 {(jsonData["Enforce"][replace]["Rate"])}% 입니다.";


        needMaterial.text = $"{inventoryMgr.materials}  /  {(jsonData["Enforce"][replace]["Material"])}";
        needGold.text = $"{inventoryMgr.gold}  /  { (jsonData["Enforce"][replace]["Gold"])}";
        //여기에 캐릭터 레벨에 맞는 강화 초기화!!!frg
        enforcePanel.SetActive(true);
    }



    public void EnforceBtn() 
    {
        string json = forcetxtFile.text;
        var jsonData = JSON.Parse(json);

        int needMat = (int)(jsonData["Enforce"][playerWeaponLv-1]["Material"]);
        int needGold = (int)(jsonData["Enforce"][playerWeaponLv-1]["Gold"]);
              
        if (inventoryMgr.materials >= needMat && inventoryMgr.gold >= needGold)
        {
                inventoryMgr.materials -= needMat;
                inventoryMgr.gold -= needGold;
                EnforcResult(playerWeaponLv);
        }
        else
        {
            lessTween.Play();
        }
    }

    public void EnforcResult(int playerWeaponLv) // 강화 버튼. GameObject Player?
    {
        StartCoroutine(EnforceEffect(playerWeaponLv));
    }
    IEnumerator EnforceEffect(int playerWeaponLv)
    {
        int replace = playerWeaponLv - 1;

        string json = forcetxtFile.text;
        var jsonData = JSON.Parse(json);

        int successRate = (int)(jsonData["Enforce"][replace]["Rate"]); //TODO: 이거랑 밑에 20이랑 테이블에서 가져오기!!
        int randomNumbuer = Random.Range(0, 101);

        Debug.Log(successRate);
        Debug.Log(randomNumbuer);

        //효과음
        enforceEffect.SetActive(true);
        yield return new WaitForSeconds(3f);
        enforceEffect.SetActive(false);

        if (randomNumbuer < successRate)
        {
            //코루틴 넣어서 강화연출 혹은 플레이어한테 프리팹
            Debug.Log("강화 성공!");
            beforeAtk.text = stateMgr.atk.ToString();
            afterAtk.text = (stateMgr.atk + (int)(jsonData["Enforce"][replace]["PlusAtk"])).ToString();
            successPanel.SetActive(true);
            stateMgr.atk += 20;
            inventoryMgr.weaponLv += 1;
            successweaponNowTxt.text = $"{jsonData["Enforce"][playerWeaponLv]["ForceLv"]} 단계";
            trophyMgr.TrophyIndexUp(3);
            InitAtk();
        }
        else
        {
            //코루틴 넣어서 강화연출 혹은 플레이어한테 프리팹

            beforeAtkF.text = stateMgr.atk.ToString();
            afterAtkF.text = stateMgr.atk.ToString();
            Debug.Log("강화 실패!");
            failweaponNowTxt.text = $"{jsonData["Enforce"][playerWeaponLv]["ForceLv"]} 단계";
            failedPanel.SetActive(true);
            failEnforceCount++;
            trophyMgr.TrophyIndexUp(4);
            InitAtk();
        }
        trophyMgr.TrophyIndexUp(2);
        yield return null;
    }




    public void InitAtk()
    {
        string json = forcetxtFile.text;
        var jsonData = JSON.Parse(json);

        playerWeaponLv = inventoryMgr.weaponLv;

        wantEnforceTxt.text = $"강화 하시겠습니까? \n 강화 확률은 {(jsonData["Enforce"][playerWeaponLv-1]["Rate"])}% 입니다.";
        weaponNowTxt.text = $"현재 재련 수치{playerWeaponLv} 단계";
        weaponAftTxt.text = $"현재 재련 수치{playerWeaponLv+1} 단계";
        inventoryMgr.atkInfo.text = stateMgr.atk.ToString();
        
        beforeAtkF.text = stateMgr.atk.ToString();
        afterAtk.text = stateMgr.atk.ToString();

        inventoryMgr.goldTxt.text = inventoryMgr.gold.ToString();
        inventoryMgr.materialTxt.text = inventoryMgr.materials.ToString();
        needMaterial.text = $"{inventoryMgr.materials}  /  {(jsonData["Enforce"][playerWeaponLv-1]["Material"])}";
        needGold.text = $"{inventoryMgr.gold}  /  { (jsonData["Enforce"][playerWeaponLv-1]["Gold"])}";
    }

    public void SuccesPanelOff()
    {
        successPanel.SetActive(false);
    }
    public void FailedPanelOff()
    {
        failedPanel.SetActive(false);
    }
}
