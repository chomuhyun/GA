using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.UI;
using System.Threading.Tasks;

public class LoadPlayerInfo : MonoBehaviour
{
    private FirebaseFirestore db;
    public string curUserEmail;

    public Text[] slot1Text;
    public Text[] slot2Text;
    public Text[] slot3Text;

    public CharacterCreate characterCreate;

    private void Start()
    {
        // �ٸ� ��ũ��Ʈ(LoginSystem ��)���� userEmail ��������
        curUserEmail = LoginSystem_test.userEmail;

        // Firebase Firestore �ʱ�ȭ
        db = FirebaseFirestore.DefaultInstance;

        characterCreate = GameObject.Find("CharacterCreate").GetComponent<CharacterCreate>();

        // �÷��̾� ���� �ε� �ڷ�ƾ ����
        StartCoroutine(LoadPlayerData(curUserEmail));
    }

    public IEnumerator LoadPlayerData(string userEmail)
    {
        // users �÷��ǿ��� userEmail(ID����)�� �� �̸��� ���� �б�
        DocumentReference userDocRef = db.Collection("users").Document(userEmail);
        AggregateQuery cnt = userDocRef.Collection(userEmail).Count;
        // ����� ������ �÷��ǵ� ��������
        yield return userDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            Debug.Log(snapshot.ToDictionary().Count); //���߿� ī��Ʈ��ŭ ������Ʈ������ɵ�.
            if (snapshot.Exists)
            {
                //Debug.Log("Document ID: " + snapshot.Id);

                #region ĳ����1
                //Debug.Log($"for�� 1��°");
                // userEmail �������� ĳ����1 �÷����� Info���� ����
                DocumentReference character1InfoDocRef = userDocRef.Collection("ĳ����1").Document("Info");

                // ������ �б�
                character1InfoDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError($"ĳ����1�� ���� �б����." + task.Exception);
                        return;
                    }

                    DocumentSnapshot characterSnapshot = task.Result;
                    Debug.Log($"ĳ����1�� ���� �б⼺��.");
                    if (characterSnapshot.Exists)
                    {
                        //Debug.Log("characterSnapshot.Exists == true");
                        // ĳ���� ���� ��������
                        IDictionary<string, object> characterData = characterSnapshot.ToDictionary();
                        slot1Text[0].text = characterData["NickName"].ToString();
                        slot1Text[1].text = characterData["Class"].ToString();
                        slot1Text[2].text = "���� " + characterData["CharacterLevel"].ToString();
                    }
                    else
                    {
                        Debug.Log($"ĳ����1�� ���� ã������.");
                    }
                });
                #endregion

                #region ĳ����2
                //Debug.Log($"for�� 2��°");
                // userEmail �������� ĳ����1 �÷����� Info���� ����
                DocumentReference character2InfoDocRef = userDocRef.Collection("ĳ����2").Document("Info");

                // ������ �б�
                character2InfoDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError($"ĳ����2�� ���� �б����." + task.Exception);
                        return;
                    }

                    DocumentSnapshot characterSnapshot = task.Result;
                    Debug.Log($"ĳ����2�� ���� �б⼺��.");
                    if (characterSnapshot.Exists)
                    {
                        Debug.Log("characterSnapshot.Exists == true");
                        // ĳ���� ���� ��������
                        IDictionary<string, object> characterData = characterSnapshot.ToDictionary();
                        slot2Text[0].text = characterData["NickName"].ToString();
                        slot2Text[1].text = characterData["Class"].ToString();
                        slot2Text[2].text = "���� " + characterData["CharacterLevel"].ToString();
                    }
                    else
                    {
                        //Debug.Log($"ĳ����2�� ���� ã������.");
                    }
                });
                #endregion

                #region ĳ����3
                //Debug.Log($"for�� 3��°");
                // userEmail �������� ĳ����1 �÷����� Info���� ����
                DocumentReference character3InfoDocRef = userDocRef.Collection("ĳ����3").Document("Info");

                // ������ �б�
                character3InfoDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError($"ĳ����3�� ���� �б����." + task.Exception);
                        return;
                    }

                    DocumentSnapshot characterSnapshot = task.Result;
                    //Debug.Log($"ĳ����3�� ���� �б⼺��.");
                    if (characterSnapshot.Exists)
                    {
                        Debug.Log("characterSnapshot.Exists == true");
                        // ĳ���� ���� ��������
                        IDictionary<string, object> characterData = characterSnapshot.ToDictionary();
                        slot3Text[0].text = characterData["NickName"].ToString();
                        slot3Text[1].text = characterData["Class"].ToString();
                        slot3Text[2].text = "���� " + characterData["CharacterLevel"].ToString();
                    }
                    else
                    {
                        Debug.Log($"ĳ����3�� ���� ã������.");
                    }
                });
                #endregion
            }
        });
    }
}