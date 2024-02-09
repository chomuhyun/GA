using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgrDontDestroy : MonoBehaviour
{
    public static DataMgrDontDestroy _instance;
    public bool playerDie; 
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
    public int classNum; //0:전사, 1:거너, 2:법사
    public int level;
    public int exp;
    public float maxhp;
    public float hp;
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
    public bool isCompleted;
    public bool isDoing;
    public bool isFirst;
    int completed;
    int doing;
    int tutoIsFirst;
    #endregion

    #region 던전의 정보를 저장하는 변수
    [Header("던전의 정보")]
    public int dungeonSortIdx = 0; //싱글, 카오스, 레이드
    public int dungeonNumIdx = 0; // n-1, n-2, n-3의 1, 2, 3번
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

    #region 캐릭터의 정보관련
    // 닉네임
    public string NickName
    {
        get { return nickName; }
        set { nickName = value; }
    }

    // 클래스번호 0:전사, 1:거너, 2:법사
    public int ClassNum
    {
        get { return classNum; }
        set { classNum = value; }
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
    public int QuestIdx
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

    public bool IsCompleted
    {
        get { return isCompleted; }
        set { isCompleted = value; }
    }

    public bool IsDoing
    {
        get { return isDoing; }
        set { isDoing = value; }
    }

    public bool IsFirst
    {
        get { return isFirst; }
        set { isFirst = value; }
    }
    #endregion

    #region 던전 관련
    public int DungeonSortIdx
    {
        get { return dungeonSortIdx; }
        set { dungeonSortIdx = value; }
    }

    public int DungeonNumIdx
    {
        get { return dungeonNumIdx; }
        set { dungeonNumIdx = value; }
    }
    #endregion

    public void SaveData() // 접속종료할때 저장
    {
        int slotNum = SelectSlot.slotNum;
        Debug.Log("현재 슬롯넘버는 : "+slotNum);
        //PlayerPrefs.SetString($"SlotNum_{slotNum}", slotNum.ToString());
        // 주석부분은 필요없을거같은데 게임내에서 변하는 정보가 아님
        //PlayerPrefs.SetString($"{slotNum}_Class", className);
        //PlayerPrefs.SetInt($"{slotNum}_ClassNum", classNum);
        #region 캐릭터 정보 저장
        PlayerPrefs.SetString($"{slotNum}_NickName", NickName);
        PlayerPrefs.SetInt($"{slotNum}_Level", Level);
        PlayerPrefs.SetInt($"{slotNum}_Exp", Exp);
        PlayerPrefs.SetFloat($"{slotNum}_MaxHp", MaxHp);
        PlayerPrefs.SetFloat($"{slotNum}_Hp", Hp);
        PlayerPrefs.SetInt($"{slotNum}_WeaponLevel", WeaponLevel);
        PlayerPrefs.SetInt($"{slotNum}_AttackPower", AttackPower);
        PlayerPrefs.SetInt($"{slotNum}_CriChance", CriChance);
        PlayerPrefs.SetFloat($"{slotNum}_CriDamage", CriDamage);
        PlayerPrefs.SetInt($"{slotNum}_UserGold", UserGold);
        PlayerPrefs.SetInt($"{slotNum}_Material", UserMaterial);
        PlayerPrefs.SetInt($"{slotNum}_ExpPotion", userExpPotion);
        #endregion

        #region 퀘스트 정보 저장
        PlayerPrefs.SetInt($"{slotNum}_QuestIdx", questIdx);
        PlayerPrefs.SetInt($"{slotNum}_QuestCurCnt", questCurCnt);
        PlayerPrefs.SetInt($"{slotNum}_QuestMaxCnt", questMaxCnt);
        PlayerPrefs.SetString($"{slotNum}_GoalTxt", goalTxt);
        if (isCompleted == true)
        {
            completed = 1;
            PlayerPrefs.SetInt($"{slotNum}_IsCompleted", 1);
        }
        else
        {
            completed = 0;
            PlayerPrefs.SetInt($"{slotNum}_IsCompleted", 0);
        }
        if (isDoing == true)
        {
            doing = 1;
            PlayerPrefs.SetInt($"{slotNum}_IsDoing", 1);
        }
        else
        {
            doing = 0;
            PlayerPrefs.SetInt($"{slotNum}_IsDoing", 0);
        }
        if (isFirst == true)
        {
            tutoIsFirst = 1;
            PlayerPrefs.SetInt($"{slotNum}_IsFirst", 1);
        }
        else
        {
            tutoIsFirst = 0;
            PlayerPrefs.SetInt($"{slotNum}_IsFirst", 0);
        }
        #endregion

        PlayerPrefs.Save();
    }
}