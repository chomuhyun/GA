using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerInfo : MonoBehaviour
{
    public static int currentSlotNum;

    public Text[] slot1Text;
    public Text[] slot2Text;
    public Text[] slot3Text;

    public CharacterCreate characterCreate;

    private void Start()
    {
        characterCreate = GameObject.Find("CharacterCreate").GetComponent<CharacterCreate>();
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
}