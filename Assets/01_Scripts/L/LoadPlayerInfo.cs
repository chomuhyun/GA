using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerInfo : MonoBehaviour
{

    public int currentSlotNum;

    private DataMgrDontDestroy dataMgrDontDestroy;
    public string nickName;
    public int level;
    public int exp;
    public float maxhp;
    public float hp;
    public int attackPower;
    public int weaponLevel;
    public int criChance;
    public float criDamage;
    public int userGold;
    public int userMaterial;
    public int userExpPotion;

    public Text[] slot1Text;
    public Text[] slot2Text;
    public Text[] slot3Text;

    public CharacterCreate characterCreate;

    private void Start()
    {
        characterCreate = GameObject.Find("CharacterCreate").GetComponent<CharacterCreate>();
        // DataMgr의 인스턴스 가져오기
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        //nickName = dataMgrDontDestroy.NickName;
        //level = dataMgrDontDestroy.Level;
        //exp = dataMgrDontDestroy.Exp;
        //maxhp = dataMgrDontDestroy.MaxHp;
        //hp = dataMgrDontDestroy.Hp;
        //attackPower = dataMgrDontDestroy.AttackPower;
        //criChance = dataMgrDontDestroy.CriChance;
        //criDamage = dataMgrDontDestroy.CriDamage;
        //weaponLevel = dataMgrDontDestroy.WeaponLevel;
        //userGold = dataMgrDontDestroy.UserGold;
        //userMaterial = dataMgrDontDestroy.UserMaterial;
        //userExpPotion = dataMgrDontDestroy.UserExpPotion;

        LoadEverySlotData();
    }

    public void LoadEverySlotData()
    {
        for (int slotNum = 0; slotNum < 3; slotNum++)
        {
            Debug.Log($"LoadEverySlotData의 for문 {slotNum}번째");

            if (PlayerPrefs.HasKey($"SlotNum_{slotNum}"))
            {
                string nickName = PlayerPrefs.GetString($"{slotNum}_NickName");
                string className = PlayerPrefs.GetString($"{slotNum}_Class");
                
                int level = PlayerPrefs.GetInt($"{slotNum}_Level");
                PlayerPrefs.GetInt($"{slotNum}_MaxHp");
                PlayerPrefs.GetInt($"{slotNum}_WeaponLevel");
                PlayerPrefs.GetInt($"{slotNum}_ATK");
                PlayerPrefs.GetInt($"{slotNum}_CriticalPer");
                PlayerPrefs.GetInt($"{slotNum}_UserGold");
                PlayerPrefs.GetInt($"{slotNum}_Material");
                PlayerPrefs.GetInt($"{slotNum}_ExpPotion");

                switch (slotNum)
                {
                    case 0:
                        Debug.Log("닉네임 : " + nickName);
                        slot1Text[0].text = nickName;
                        slot1Text[1].text = className;
                        slot1Text[2].text = level.ToString();
                        break;
                    case 1:
                        slot2Text[0].text = nickName;
                        slot2Text[1].text = className;
                        slot2Text[2].text = level.ToString();
                        break;
                    case 2:
                        slot3Text[0].text = nickName;
                        slot3Text[1].text = className;
                        slot3Text[2].text = level.ToString();
                        break;
                    default:
                        break;
                }
            }
        }
    }


    public void LoadCurrnetPlayerData()
    {
        currentSlotNum = SelectSlot.slotNum;
        Debug.Log("데이터를 불러올 현재 슬롯의 번호는 " + currentSlotNum);
        // Start버튼을 눌렀을때, currentSlotNum에 있는 정보를 가져와서 dataMgr에 넣어준다.
        #region 캐릭터정보 변수에 담기
        nickName = PlayerPrefs.GetString($"{currentSlotNum}_NickName"); //필요하면 쓰자
        level = PlayerPrefs.GetInt($"{currentSlotNum}_Level");
        exp = PlayerPrefs.GetInt($"{currentSlotNum}_Exp");
        maxhp = PlayerPrefs.GetFloat($"{currentSlotNum}_MaxHp");
        hp = PlayerPrefs.GetFloat($"{currentSlotNum}_Hp");
        attackPower = PlayerPrefs.GetInt($"{currentSlotNum}_AttackPower");
        weaponLevel = PlayerPrefs.GetInt($"{currentSlotNum}_WeaponLevel");
        criChance = PlayerPrefs.GetInt($"{currentSlotNum}_CriChance");
        criDamage = PlayerPrefs.GetFloat($"{currentSlotNum}_CriDamage");
        userGold = PlayerPrefs.GetInt($"{currentSlotNum}_UserGold");
        userMaterial = PlayerPrefs.GetInt($"{currentSlotNum}_Material");
        userExpPotion = PlayerPrefs.GetInt($"{currentSlotNum}_ExpPotion");
        #endregion

        #region 캐릭터정보. 변수의 값을 싱글톤에 보내주기
        dataMgrDontDestroy.NickName = nickName;
        dataMgrDontDestroy.Level = level;
        dataMgrDontDestroy.Exp = exp;
        dataMgrDontDestroy.MaxHp = maxhp;
        dataMgrDontDestroy.Hp = hp;
        dataMgrDontDestroy.AttackPower = attackPower;
        dataMgrDontDestroy.WeaponLevel = weaponLevel;
        dataMgrDontDestroy.CriChance = criChance;
        dataMgrDontDestroy.CriDamage = criDamage;
        dataMgrDontDestroy.UserGold = userGold;
        dataMgrDontDestroy.UserMaterial = userMaterial;
        dataMgrDontDestroy.UserExpPotion = userExpPotion;
        #endregion
    }

}