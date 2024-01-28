using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Auth;
using System;
using UnityEngine.UI;
using Firebase;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CharacterCreate : MonoBehaviour
{
    private FirebaseFirestore db;
    private FirebaseAuth auth;

    public GameObject selectPanel;
    public GameObject nickNamePanel;

    public InputField nickNameIF;
    public string characterNickName;

    public string userEmail;

    public static string currentCharacterClass;

    public static int currentClassNum;
    public static int currentSlotNum;
    public Sprite[] sprites;
    public Button[] slots;

    LoadPlayerInfo loadPlayerInfoInstance;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        selectPanel.SetActive(false);
        nickNamePanel.SetActive(false);
        userEmail = LoginSystem_test.userEmail;
        loadPlayerInfoInstance = GameObject.Find("LoadPlayerInfo").GetComponent<LoadPlayerInfo>();
        Debug.Log(userEmail);
    }

    IEnumerator CreateCharacter(string userEmail, string characterNickName, string className)
    {
        Debug.Log("�ڷ�ƾ ����");
        Debug.Log(className);

        // Firestore�� ����� ������ �߰�
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        // ĳ���� 1, 2, 3�÷��� ���� ������ ����.
        DocumentReference docRef = db.Collection("users").Document(userEmail).Collection($"ĳ����{currentClassNum+1}").Document("Info");
        Dictionary<string, object> user = new Dictionary<string, object>
        {
            {"SlotNum", currentSlotNum},
            {"NickName", characterNickName},
            {"Class", className},
            {"CharacterLevel", 1},
            {"MaxHp", 100},
            {"WeaponLevel", 1},
            {"ATK", 100},
            {"CriticalPer", 50},
            {"UserGold", 0},
            {"Material", 0},
            {"ExpPotion", 0},
            {"UpdateTime", FieldValue.ServerTimestamp}
        };

        yield return docRef.SetAsync(user).ContinueWithOnMainThread(task =>
        {
            Debug.Log("�������ۼ� ����");
            if (task.IsFaulted)
            {
                foreach (Exception exception in task.Exception.InnerExceptions)
                {
                    if (exception is FirebaseException firebaseException)
                    {
                        Debug.LogError($"FirebaseException: {firebaseException.ErrorCode} - {firebaseException.Message}");
                    }
                    else
                    {
                        Debug.LogError($"Exception: {exception}");
                    }
                }
                Debug.Log("�������ۼ� ����");
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("�������ۼ� ���");
            }
            else
            {
                Debug.Log($"{characterNickName} �� ĳ���ͻ����� �Ϸ�Ǿ����ϴ�...");
                Debug.Log("�������ۼ� ��");
            }
        });
        Debug.Log("�ڷ�ƾ ����");
    }

    // ���� ���� Ŭ�� ��, ĳ���ͻ�����ư Ŭ�� => �гζ���
    public void OnClickPanelActiveBtn()
    {
        currentSlotNum = SelectSlot.slotNum;
        // �Ҵ�� ���Թ�ȣ Ȯ��
        Debug.Log($"�����ϱ���, Selected Slot: {currentSlotNum+1}");
        selectPanel.SetActive(true);
    }

    //ĳ���ͻ������ ��ư
    public void OnClickCancelCharacterBtn()
    {
        selectPanel.SetActive(false);
    }

    // ������ ĳ���͸� ����, ������ư Ŭ�� => �г��� ���� �гζ���
    public void OnClickSelectAndCreateBtn()
    {
        currentClassNum = SelectChar.CharNum;
        Debug.Log($"Ŭ�����ѹ� : {currentClassNum+1}");
        nickNameIF.text = "";
        nickNamePanel.SetActive(true);
    }

    //�г��Ӱ������ϰ� �ڷΰ���(ĳ���� �ٽð��� �ʹٴ���)
    public void OnClickCancelNickBtn()
    {
        nickNamePanel.SetActive(false);
    }

    // �г��� �����Ϸ� ��ư
    public void OnClickDecideNickBtn()
    {
        characterNickName = nickNameIF.text;
        currentCharacterClass = SelectChar.currentCharacter;
        Debug.Log(currentCharacterClass);
        StartCoroutine(CreateCharacter(userEmail, characterNickName, currentCharacterClass));
        Debug.Log("�г���, ĳ���ͻ��� �Ϸ�");

        Debug.Log("userEmail : " + userEmail);
        StartCoroutine(loadPlayerInfoInstance.LoadPlayerData(userEmail));

        slots[currentSlotNum].GetComponent<Image>().sprite = sprites[currentClassNum];
        selectPanel.SetActive(false);
        nickNamePanel.SetActive(false);
    }

    public void OnClickGoLoginSceneBtn()
    {
        SceneManager.LoadScene("Login");
    }
}
