using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    Blue, Red, Yellow
}

public class DataMgr : MonoBehaviour
{
    // �̱���
    public static DataMgr instance;

    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion
    }

    public Character currentCharacter;

}
