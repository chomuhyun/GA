using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    #region 싱글톤
    private static InventoryManager instance;
    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 인스턴스가 없으면 새로 생성
                GameObject singletonObject = new GameObject("InventoryManager");
                instance = singletonObject.AddComponent<InventoryManager>();
                DontDestroyOnLoad(singletonObject); // 씬 전환 시에도 유지되도록 설정
            }

            return instance;
        }
    }
    #endregion  

    [Space(1)]
    public StateManager stateMgr;
    public GameObject rewardCanvas;
    public GameObject inventoryCanvas;
    public bool isInven;

    [Header("PlayerState")]
    [Header("싱글톤 적용됌")]

    public int weaponLv = 1;
    public string playerName;
    public string playerNick;

    public int expPotion;
    public int materials;
    public int gold;

    [HideInInspector]
    public Image goldImage;
    public Image expImage;
    public Image materialImage;
    public Text goldTxt;
    public Text expTxt;
    public Text materialTxt;
    public Text atkInfo;
    public Text playerLv;   // 인벤토리창 레벨
    public Text playerNickTxt; // 인벤토리창 칭호
    public Text playerNameTxt; // 인벤토리창 이름

    private void Awake()
    {
        Transform tr = transform.GetChild(0).GetChild(0).GetChild(1);
        rewardCanvas = GameObject.Find("RewardContent").gameObject;//TODO:
        inventoryCanvas = GameObject.Find("InventoryCanvas").gameObject;
        goldImage = tr.Find("item_gold").GetComponent<Image>();
        expImage = tr.transform.Find("item_exp").GetComponent<Image>();
        materialImage = tr.transform.Find("item_material").GetComponent<Image>();
        goldTxt = tr.transform.Find("item_goldTxt").GetComponent<Text>();
        expTxt = tr.transform.Find("item_expTxt").GetComponent<Text>();
        materialTxt = tr.transform.Find("item_materialTxt").GetComponent<Text>();
        atkInfo = tr.transform.Find("atkInfo").GetComponent<Text>();
        playerLv = tr.transform.Find("lvInfo").GetComponent<Text>();

        playerNameTxt = tr.transform.Find("PlayerName").GetComponent<Text>();
        playerNickTxt = tr.transform.Find("PlayerNick").GetComponent<Text>();

        inventoryCanvas.SetActive(false);


    }

    private void Start()
    {
        // 싱글톤 인스턴스가 이미 존재하면 현재 인스턴스를 파괴
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // 처음 생성된 경우, 현재 인스턴스를 설정
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    } // 싱글톤 전용

    private void Update() // i 눌러서 인벤토리 열기
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isInven)
            {
                inventoryCanvas.SetActive(true);
                isInven = true;
            }
            else
            {
                inventoryCanvas.SetActive(false);
                isInven = false;
            }
        }

    }






    #region 아이템
    public void InitInventory()
    {
        if (gold >= 1)
        {
            goldImage.color = Color.white;
        }
        else
        {
            goldImage.color = Color.gray;
        }

        if (expPotion >= 1)
        {
            expImage.color = Color.white;
        }
        else
        {
            expImage.color = Color.gray;
        }

        if (materials >= 1)
        {
            materialImage.color = Color.white;
        }
        else
        {
            materialImage.color = Color.gray;
        }
        goldTxt.text = gold.ToString();
        expTxt.text = expPotion.ToString();
        materialTxt.text = materials.ToString();
        atkInfo.text = stateMgr.atk.ToString();
        playerLv.text = stateMgr.level.ToString();
        
        playerNickTxt.text = playerNick;
        playerNameTxt.text = playerName;

    } //아이템 1개 이상이면 불들어오게

    public void AddMaterial() // 태그 찾기 -> 인벤토리 개수 늘리기
    {
            for (int i = 0; i < rewardCanvas.transform.childCount; i++)
            {
                GameObject item = rewardCanvas.transform.GetChild(i).gameObject;
                if (rewardCanvas.transform.GetChild(i).CompareTag("Material"))
                {
                    materials += item.GetComponent<ItemJsonData>().count;
                    item.SetActive(false);
                }
                if (rewardCanvas.transform.GetChild(i).CompareTag("Exp"))
                {
                    expPotion += item.GetComponent<ItemJsonData>().count;
                    item.SetActive(false);
                }
                if (rewardCanvas.transform.GetChild(i).CompareTag("Gold"))
                {
                    gold += item.GetComponent<ItemJsonData>().count;
                    item.SetActive(false);
                }

       
            //TODO: Destroy(item);
        }
    }


    public void SendInventory() // Reward에 있는 아이템 수령받기 버튼
    {
        AddMaterial();
        InitInventory();
    }
#endregion


}
