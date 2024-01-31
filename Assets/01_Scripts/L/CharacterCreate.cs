using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CharacterCreate : MonoBehaviour
{
    public GameObject selectPanel;
    public GameObject nickNamePanel;

    public InputField nickNameIF;
    public string characterNickName;

    public static string currentCharacterClass;

    public static int currentClassNum;
    public static int currentSlotNum;
    public Sprite[] sprites;
    public Button[] slots;

    public LoadPlayerInfo loadPlayerInfo;
    private int classNum;

    void Start()
    {
        selectPanel.SetActive(false);
        nickNamePanel.SetActive(false);

        loadPlayerInfo = GameObject.Find("LoadPlayerInfo").GetComponent<LoadPlayerInfo>();
    }

    // 만들 슬롯 클릭 후, 캐릭터생성버튼 클릭 => 패널띄우기
    public void OnClickPanelActiveBtn()
    {
        currentSlotNum = SelectSlot.slotNum;
        // 할당된 슬롯번호 확인
        Debug.Log($"생성하기전, Selected Slot: {currentSlotNum+1}");
        selectPanel.SetActive(true);
    }

    //캐릭터생성취소 버튼
    public void OnClickCancelCharacterBtn()
    {
        selectPanel.SetActive(false);
    }

    // 생성할 캐릭터를 고르고, 생성버튼 클릭 => 닉네임 설정 패널띄우기
    public void OnClickSelectAndCreateBtn()
    {
        currentClassNum = SelectChar.CharNum;
        Debug.Log($"클래스넘버 : {currentClassNum+1}");
        nickNameIF.text = "";
        nickNamePanel.SetActive(true);
    }

    //닉네임결정안하고 뒤로가기(캐릭터 다시고르고 싶다던지)
    public void OnClickCancelNickBtn()
    {
        nickNamePanel.SetActive(false);
    }

    // 닉네임 결정완료 버튼
    public void OnClickDecideNickBtn()
    {
        currentSlotNum = SelectSlot.slotNum;
        characterNickName = nickNameIF.text;
        currentCharacterClass = SelectChar.currentCharacter;
        
        switch (currentCharacterClass) //Warrior, Gunner, Magician
        {
            case "Warrior":
                classNum = 0;
                break;
            case "Gunner":
                classNum = 1;
                break;
            case "Magician":
                classNum = 2;
                break;
            default:
                break;
        }
        Debug.Log(currentCharacterClass);
        StartCoroutine(CreateCharacter(currentSlotNum, characterNickName, currentCharacterClass, currentClassNum));
        Debug.Log("닉네임, 캐릭터생성 완료");
        loadPlayerInfo.LoadEverySlotData();

        // 나중에 이부분(슬롯에 sprite넣기) 게임로딩하고 슬롯에도 넣게끔 추가해보기
        slots[currentSlotNum].GetComponent<Image>().sprite = sprites[currentClassNum];
        selectPanel.SetActive(false);
        nickNamePanel.SetActive(false);
    }

    IEnumerator CreateCharacter(int slotNum, string nickName, string className, int classNum)
    {
        Debug.Log(nickName);
        Debug.Log(className);
        PlayerPrefs.SetString($"SlotNum_{slotNum}", slotNum.ToString());

        PlayerPrefs.SetString($"{slotNum}_NickName", nickName);
        PlayerPrefs.SetString($"{slotNum}_Class", className);
        PlayerPrefs.SetInt($"{slotNum}_ClassNum", classNum);
        PlayerPrefs.SetInt($"{slotNum}_Level", 1);
        PlayerPrefs.SetInt($"{slotNum}_MaxHp", 500);
        PlayerPrefs.SetInt($"{slotNum}_WeaponLevel", 1);
        PlayerPrefs.SetInt($"{slotNum}_ATK", 50);
        PlayerPrefs.SetInt($"{slotNum}_CriticalPer", 50);
        PlayerPrefs.SetInt($"{slotNum}_UserGold", 0);
        PlayerPrefs.SetInt($"{slotNum}_Material", 0);
        PlayerPrefs.SetInt($"{slotNum}_ExpPotion", 0);

        PlayerPrefs.Save();

        yield return null;
    }

    public void DeleteCharacter()
    {
        int slotNum = SelectSlot.slotNum;
        Debug.Log(slotNum);

        if (PlayerPrefs.HasKey($"SlotNum_{slotNum}"))
        {
            PlayerPrefs.DeleteKey($"{slotNum}_NickName");
            PlayerPrefs.DeleteKey($"{slotNum}_Class");
            PlayerPrefs.DeleteKey($"{slotNum}_ClassNum");
            PlayerPrefs.DeleteKey($"{slotNum}_Level");
            PlayerPrefs.DeleteKey($"{slotNum}_MaxHp");
            PlayerPrefs.DeleteKey($"{slotNum}_WeaponLevel");
            PlayerPrefs.DeleteKey($"{slotNum}_ATK");
            PlayerPrefs.DeleteKey($"{slotNum}_CriticalPer");
            PlayerPrefs.DeleteKey($"{slotNum}_UserGold");
            PlayerPrefs.DeleteKey($"{slotNum}_Material");
            PlayerPrefs.DeleteKey($"{slotNum}_ExpPotion");

            PlayerPrefs.Save();

            loadPlayerInfo.LoadEverySlotData();
        }
        else
        {
            Debug.Log("현재슬롯에 키값이 없음");
        }
    }

    // 만약 직업마다 스탯이 다르다면.. 여기서 switch로 클래스명받아와서 createcharacter에서 호출하게끔하기
    //void CreateDefaultPlayerData(string userID)
    //{
        // 예시로 대충 1레벨일때의 능력치.. 실제로 만들때 초기화하기
        //int defaultLevel = 1;
        //int defaultAtk = 10;

        //PlayerPrefs.SetInt(userID, defaultLevel);
        //PlayerPrefs.SetInt(userID, defaultAtk);
        //PlayerPrefs.Save();
    //}

    public void OnClickGoLoginSceneBtn() // 로비에서 연결끊고 나가는거 추가하기
    {
        SceneManager.LoadScene("Login");
    }
}
