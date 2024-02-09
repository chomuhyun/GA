using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using Photon.Pun;

public class EnforceMgr : MonoBehaviourPunCallbacks
{
    public DataMgrDontDestroy dataMgrDontDestroy;
    public TextAsset forcetxtFile; //Jsonfile

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

    [Header("강화 이후 정보")]
    public int playerWeaponLevel;
    public int playerGold;
    public int playerMaterial;
    public int playerAttackPower;

    [Header("NPC 대화")]
    public DialogueTrigger dialogueTrigger; //대본
    public GameObject nPCConversation;



    private void Awake()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        
        var jsonitemFile = Resources.Load<TextAsset>("Json/EnforceTable");
        forcetxtFile = jsonitemFile;
        lessTween = GameObject.Find("lessTween").GetComponent<Jun_TweenRuntime>();
        enforcePanel = GameObject.Find("EnforcePanel");
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
        playerWeaponLevel = dataMgrDontDestroy.WeaponLevel;
        playerMaterial = dataMgrDontDestroy.UserMaterial;
        playerGold = dataMgrDontDestroy.UserGold;
        playerAttackPower = dataMgrDontDestroy.AttackPower;
        enforceEffect.SetActive(false);
        successPanel.SetActive(false);
        failedPanel.SetActive(false);
        enforcePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("충돌일어남");
                dialogueTrigger.Trigger(); // 대본 가져옴
                nPCConversation.SetActive(true); // 대화창 켜짐
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void OnEnforcePanelBtn() // F 강화하기 버튼 눌러서 강화
    {
        OnEnforcePanel();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                StateManager stateManager = other.gameObject.GetComponent<StateManager>();
                stateManager.weaponLevel = playerWeaponLevel;
                stateManager.userMaterial = playerMaterial;
                stateManager.userGold = playerGold;
                stateManager.attackPower = playerAttackPower;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)   
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                // 강화창 껐으니까 플레이어의 정보에 반영
                enforcePanel.SetActive(false);
            }
        }
    }

    public void OnEnforcePanel() // 창이 열림, 플레이어 웨폰레벨을 받음
    {
        int replace = playerWeaponLevel - 1;

        string json = forcetxtFile.text;
        var jsonData = JSON.Parse(json);

        weaponNowTxt.text = $"현재 재련 수치{playerWeaponLevel} 단계";
        weaponAftTxt.text = $"현재 재련 수치{playerWeaponLevel + 1} 단계";
        wantEnforceTxt.text = $"강화 하시겠습니까? \n 강화 확률은 {(jsonData["Enforce"][replace]["Rate"])}% 입니다.";

        needMaterial.text = $"{playerMaterial}  /  {(jsonData["Enforce"][replace]["Material"])}";
        needGold.text = $"{playerGold}  /  { (jsonData["Enforce"][replace]["Gold"])}";
        //여기에 캐릭터 레벨에 맞는 강화 초기화!!!frg
        enforcePanel.SetActive(true);
    }



    public void EnforceBtn() 
    {
        string json = forcetxtFile.text;
        var jsonData = JSON.Parse(json);

        int needMat = (int)(jsonData["Enforce"][playerWeaponLevel - 1]["Material"]);
        int needGold = (int)(jsonData["Enforce"][playerWeaponLevel - 1]["Gold"]);
        
        if (playerMaterial >= needMat && playerGold >= needGold)
        {
            playerMaterial -= needMat;
            playerGold -= needGold;
            EnforcResult(playerWeaponLevel);
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
            beforeAtk.text = playerAttackPower.ToString();
            afterAtk.text = (playerAttackPower + (int)(jsonData["Enforce"][replace]["PlusAtk"])).ToString();
            successPanel.SetActive(true);
            playerAttackPower += 20;
            playerWeaponLevel += 1;
            successweaponNowTxt.text = $"{jsonData["Enforce"][playerWeaponLv]["ForceLv"]} 단계";
            //trophyMgr.TrophyIndexUp(3);
            InitAtk();
            SyncDataMgr();
        }
        else
        {
            //코루틴 넣어서 강화연출 혹은 플레이어한테 프리팹

            beforeAtkF.text = playerAttackPower.ToString();
            afterAtkF.text = playerAttackPower.ToString();
            Debug.Log("강화 실패!");
            failweaponNowTxt.text = $"{jsonData["Enforce"][playerWeaponLv]["ForceLv"]} 단계";
            failedPanel.SetActive(true);
            //failEnforceCount++;
            //trophyMgr.TrophyIndexUp(4);
            InitAtk();
            SyncDataMgr();
        }
        //trophyMgr.TrophyIndexUp(2);
        yield return null;
    }

    public void InitAtk()
    {
        string json = forcetxtFile.text;
        var jsonData = JSON.Parse(json);
        Debug.Log(playerWeaponLevel);
        //playerWeaponLevel을 이미 알고있음
        //playerWeaponLv = inventoryMgr.weaponLv;

        wantEnforceTxt.text = $"강화 하시겠습니까? \n 강화 확률은 {(jsonData["Enforce"][playerWeaponLevel - 1]["Rate"])}% 입니다.";
        weaponNowTxt.text = $"현재 재련 수치{playerWeaponLevel} 단계";
        weaponAftTxt.text = $"현재 재련 수치{playerWeaponLevel + 1} 단계";
        
        beforeAtkF.text = playerAttackPower.ToString();
        afterAtk.text = playerAttackPower.ToString();

        needMaterial.text = $"{playerMaterial}  /  {(jsonData["Enforce"][playerWeaponLevel - 1]["Material"])}";
        needGold.text = $"{playerGold}  /  { (jsonData["Enforce"][playerWeaponLevel - 1]["Gold"])}";
    }

    public void SyncDataMgr()
    {
        dataMgrDontDestroy.WeaponLevel = playerWeaponLevel;
        dataMgrDontDestroy.UserMaterial = playerMaterial;
        dataMgrDontDestroy.userGold = playerGold;
        dataMgrDontDestroy.AttackPower = playerAttackPower;
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
