using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.UI;
using Firebase;
using UnityEngine.SceneManagement;

public class LoginSystem_test : MonoBehaviour
{
    public static string userEmail;
    public string password;
    public InputField emailInput;
    public InputField pwInput;

    public Text outputText;

    public bool isExist = false;
    private FirebaseAuth auth; // �α��� or ȸ������ � ���
    private FirebaseUser user; // ������ �Ϸ�� ���� ����

    PhotonManager photonManager;

    void Start()
    {
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
        LoginState += OnChangedState;
        Init();
    }

    public string UserId => user.UserId;

    public Action<bool> LoginState;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;

        // �ӽ� ó��
        if (auth.CurrentUser != null)
        {
            Logout();
        }
        auth.StateChanged += OnChanged;
    }

    private void OnChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = auth.CurrentUser != user && auth.CurrentUser != null;
            if (!signed && user != null)
            {
                Debug.Log("�α׾ƿ�");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("�α���");
                LoginState?.Invoke(true);
            }
        }
    }
    
    // �ű����� ������ FireStore�� �ۼ�
    IEnumerator CreateUserInFirestore(string userEmail, string userPassword)
    {
        Debug.Log("�ڷ�ƾ ����");

        // Firestore�� ����� ������ �߰�
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("users").Document(userEmail);
        Dictionary<string, object> user = new Dictionary<string, object>
        {
            {"UserPw", userPassword },
            {"UpdateTime", FieldValue.ServerTimestamp },
            {"UID", FirebaseAuth.DefaultInstance.CurrentUser.UserId} // ���� ������� UID �߰�
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
                Debug.Log($"{userEmail} �� ȸ�������� �Ϸ�Ǿ����ϴ�...");
                Debug.Log("�������ۼ� ��");
            }
        });
        Debug.Log("�ڷ�ƾ ����");
    }

    public void OnClickCreateBtn()
    {
        CheckingEmailAndPw();

        auth.CreateUserWithEmailAndPasswordAsync(userEmail, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("ȸ������ ���");
                return;
            }
            if (task.IsFaulted)
            {
                AggregateException exception = task.Exception;
                if (exception != null)
                {
                    foreach (Exception innerException in exception.InnerExceptions)
                    {
                        if (innerException is FirebaseException firebaseException)
                        {
                            // FirebaseException�� ErrorCode �� Message�� ����� �α׿� ���
                            Debug.LogError($"FirebaseException: {firebaseException.ErrorCode} - {firebaseException.Message}");
                        }
                        else
                        {
                            // ��Ÿ ������ ��� �޽����� ���
                            Debug.LogError($"Exception: {innerException.Message}");
                        }
                    }
                }

                // ȸ������ ���� ���� => �̸����� ������ / ��й�ȣ�� �ʹ� ���� / �̹� ���Ե� �̸��� ���..
                Debug.Log("ȸ������ ����");
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            Debug.Log("ȸ������ �Ϸ�");

            // ȸ�������� �Ϸ�� �Ŀ� �ٸ� ������ ������ �� ����
            StartCoroutine(CreateUserInFirestore(userEmail, password));
        });
    }

    // �������� ������ �ҷ�����
    IEnumerator ReadUserData(string userEmail)
    {
        //Debug.Log("�ڷ�ƾ ����");
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("users").Document(userEmail);
        yield return docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists) // �̸����� �ִٸ�...
            {
                //Debug.Log($"{snapshot.Id}");
                isExist = true;
                Dictionary<string, object> doc = snapshot.ToDictionary();

                foreach (KeyValuePair<string, object> pair in doc)
                {
                    if (pair.Key == "UserPw")
                    {
                        //Debug.Log("Password :: " + pair.Value.ToString());
                    }
                    if (pair.Key == "UID")
                    {
                        //Debug.Log("UID :: " + pair.Value.ToString());
                    }
                }
            }
            else
            {
                Debug.Log($"{userEmail} �� �������� �ʽ��ϴ�...");
                outputText.text = $"{userEmail} �� �������� �ʽ��ϴ�...";
                isExist = false;
            }
        });
        //Debug.Log("�ڷ�ƾ ����");
    }

    public void OnClickLoginBtn()
    {
        Debug.Log("�α��ι�ư ����");

        CheckingEmailAndPw();
        auth.SignInWithEmailAndPasswordAsync(userEmail, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("�α��� ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("�α��� ����");
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            //Debug.Log("�α��� �Ϸ�");

            StartCoroutine(ReadUserDataAndLoadScene(userEmail));
        });


    }

    IEnumerator ReadUserDataAndLoadScene(string userEmail)
    {
        yield return StartCoroutine(ReadUserData(userEmail));
        Debug.Log("JoinHome ����");
        photonManager.JoinHome();
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("�α׾ƿ�");
    }

    public void CheckingEmailAndPw()
    {
        userEmail = emailInput.text;
        password = pwInput.text;
    }

    private void OnChangedState(bool sign)
    {
        outputText.text = sign ? "�α��� : " : "�α׾ƿ� : ";
        outputText.text += UserId;
    }
}
