using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    Blue, Red, Yellow
}

public class DataMgr : MonoBehaviour
{
    // ΩÃ±€≈Ê
    public static DataMgr instance;

    private void Awake()
    {
        #region ΩÃ±€≈Ê
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
