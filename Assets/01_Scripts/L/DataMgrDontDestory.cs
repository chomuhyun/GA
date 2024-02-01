using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgrDontDestroy : MonoBehaviour
{
    public static DataMgrDontDestroy _instance;
    public static DataMgrDontDestroy Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DataMgrDontDestroy>();
            }
            return _instance;
        }
    }

    #region 플레이어의 정보를 저장하는 변수
    [Header("플레이어의 정보")]
    public string nickName;
    public int level;
    public int exp;
    public float maxhp;
    public float hp; // 현재체력이 필요한가?
    public int weaponLevel;
    public int attackPower;
    public int criChance;
    public float criDamage;
    public int userGold;
    public int userMateiral;
    public int userExpPotion;
    #endregion

    #region 퀘스트의 정보를 저장하는 변수
    [Header("퀘스트의 정보")]
    public int questIdx;
    public string goalTxt;
    public int questCurCnt;
    public int questMaxCnt;
    #endregion

    // 싱글톤
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스 파괴
        }
    }

    // 초기화 필요하면 사용
    void Start()
    {

    }
    #region 캐릭터의 정보관련
    // 닉네임
    public string NickName
    {
        get { return nickName; }
        set { nickName = value; }
    }

    // 공격력
    public int AttackPower
    {
        get { return attackPower; }
        set { attackPower = value; }
    }

    // 레벨
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    // 경험치통
    public int Exp
    {
        get { return exp; }
        set { exp = value; }
    }

    // 최대HP
    public float MaxHp
    {
        get { return maxhp; }
        set { maxhp = value; }
    }

    // 현재HP?
    public float Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    // 무기의 레벨
    public int WeaponLevel
    {
        get { return weaponLevel; }
        set { weaponLevel = value; }
    }

    // 크리확률
    public int CriChance
    {
        get { return criChance; }
        set { criChance = value; }
    }

    // 크리데미지
    public float CriDamage
    {
        get { return criDamage; }
        set { criDamage = value; }
    }

    // 보유골드
    public int UserGold
    {
        get { return userGold; }
        set { userGold = value; }
    }

    // 보유재화
    public int UserMaterial
    {
        get { return userMateiral; }
        set { userMateiral = value; }
    }

    // 보유 경험치포션
    public int UserExpPotion
    {
        get { return userExpPotion; }
        set { userExpPotion = value; }
    }
    #endregion

    #region 퀘스트관련
    public int QusetIdx
    {
        get { return questIdx; }
        set { questIdx = value; }
    }

    public string GoalTxt
    {
        get { return goalTxt; }
        set { goalTxt = value; }
    }

    public int QuestCurCnt
    {
        get { return questCurCnt; }
        set { questCurCnt = value; }
    }

    public int QuestMaxCnt
    {
        get { return questMaxCnt; }
        set { questMaxCnt = value; }
    }
    #endregion

    public void LoadData() // 접속했을때, 저장 <- 이건 이미 loadplayer에서 start누를때 datamgr에 저장을 함.
    {

    }

    public void SaveDate() // 접속종료할때 저장
    {

    }
}