using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUDChanger : MonoBehaviour
{
    public DataMgrDontDestroy dataMgr;

    public Text plyaerHUDLv;
    public Text playerHUDName;
    public Image playerHUDFace;
    public Sprite[] playerHUDFaceSet;


    private void Awake()
    {
        dataMgr = DataMgrDontDestroy.Instance;

        plyaerHUDLv.text = $"Lv {dataMgr.level.ToString()}";
        playerHUDName.text = dataMgr.nickName;

        if (dataMgr.classNum == 0)
            playerHUDFace.sprite = playerHUDFaceSet[1];
        if (dataMgr.classNum == 1)
            playerHUDFace.sprite = playerHUDFaceSet[2];
        if (dataMgr.classNum == 2)
            playerHUDFace.sprite = playerHUDFaceSet[3];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
