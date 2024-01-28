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

    [Header("��ȭ�Ͻðڽ��ϱ�?")]
    public GameObject enforcePanel;
    public Text weaponNowTxt;
    public Text weaponAftTxt;
    public Text wantEnforceTxt;
    public Text needMaterial;
    public Text needGold;

    public GameObject enforceEffect;
    public Jun_TweenRuntime lessTween;

    [Header("��ü���")]
    public GameObject successPanel;
    public Text successweaponNowTxt;
    public Text beforeAtk;
    public Text afterAtk;

    [Header("��ý���")]
    public GameObject failedPanel;
    public Text failweaponNowTxt;
    public Text beforeAtkF;
    public Text afterAtkF;

    [Header("����Ŭ�����")]
    public int failEnforceCount;


    public int playerWeaponLv;




    private void Awake()
    {
        var jsonitemFile = Resources.Load<TextAsset>("Json/EnforceTable");
        forcetxtFile = jsonitemFile;
        lessTween = GameObject.Find("lessTween").GetComponent<Jun_TweenRuntime>();
        enforcePanel = GameObject.Find("EnforcePanel");
        stateMgr = GameObject.FindWithTag("Player").GetComponent<StateManager>();
        inventoryMgr = GameObject.Find("InventoryMgr").GetComponent<InventoryManager>();
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
        InitAtk();
        enforcePanel.SetActive(false);
    }



    public void OnEnforcePanel(int playerWeaponLv) // â�� ����, �÷��̾� ���������� ����
    {
        int replace = playerWeaponLv - 1;

        string json = forcetxtFile.text;
        var jsonData = JSON.Parse(json);

        weaponNowTxt.text = $"���� ��� ��ġ{playerWeaponLv} �ܰ�";
        weaponAftTxt.text = $"���� ��� ��ġ{playerWeaponLv+ 1} �ܰ�";
        wantEnforceTxt.text = $"��ȭ �Ͻðڽ��ϱ�? \n ��ȭ Ȯ���� {(jsonData["Enforce"][replace]["Rate"])}% �Դϴ�.";


        needMaterial.text = $"{inventoryMgr.materials}  /  {(jsonData["Enforce"][replace]["Material"])}";
        needGold.text = $"{inventoryMgr.gold}  /  { (jsonData["Enforce"][replace]["Gold"])}";
        //���⿡ ĳ���� ������ �´� ��ȭ �ʱ�ȭ!!!frg
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

    public void EnforcResult(int playerWeaponLv) // ��ȭ ��ư. GameObject Player?
    {
        StartCoroutine(EnforceEffect(playerWeaponLv));
    }
    IEnumerator EnforceEffect(int playerWeaponLv)
    {
        int replace = playerWeaponLv - 1;

        string json = forcetxtFile.text;
        var jsonData = JSON.Parse(json);

        int successRate = (int)(jsonData["Enforce"][replace]["Rate"]); //TODO: �̰Ŷ� �ؿ� 20�̶� ���̺��� ��������!!
        int randomNumbuer = Random.Range(0, 101);

        Debug.Log(successRate);
        Debug.Log(randomNumbuer);

        //ȿ����
        enforceEffect.SetActive(true);
        yield return new WaitForSeconds(3f);
        enforceEffect.SetActive(false);

        if (randomNumbuer < successRate)
        {
            //�ڷ�ƾ �־ ��ȭ���� Ȥ�� �÷��̾����� ������
            Debug.Log("��ȭ ����!");
            beforeAtk.text = stateMgr.atk.ToString();
            afterAtk.text = (stateMgr.atk + (int)(jsonData["Enforce"][replace]["PlusAtk"])).ToString();
            successPanel.SetActive(true);
            stateMgr.atk += 20;
            inventoryMgr.weaponLv += 1;
            successweaponNowTxt.text = $"{jsonData["Enforce"][playerWeaponLv]["ForceLv"]} �ܰ�";
            trophyMgr.TrophyIndexUp(3);
            InitAtk();
        }
        else
        {
            //�ڷ�ƾ �־ ��ȭ���� Ȥ�� �÷��̾����� ������

            beforeAtkF.text = stateMgr.atk.ToString();
            afterAtkF.text = stateMgr.atk.ToString();
            Debug.Log("��ȭ ����!");
            failweaponNowTxt.text = $"{jsonData["Enforce"][playerWeaponLv]["ForceLv"]} �ܰ�";
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

        wantEnforceTxt.text = $"��ȭ �Ͻðڽ��ϱ�? \n ��ȭ Ȯ���� {(jsonData["Enforce"][playerWeaponLv-1]["Rate"])}% �Դϴ�.";
        weaponNowTxt.text = $"���� ��� ��ġ{playerWeaponLv} �ܰ�";
        weaponAftTxt.text = $"���� ��� ��ġ{playerWeaponLv+1} �ܰ�";
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
