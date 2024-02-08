using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidQuestManager : MonoBehaviour
{
    public DataMgrDontDestroy dataMgrDontDestroy;
    public QuestPopUpManager questPopUpManager;
    public GameObject questPopUpPanel;

    private void Start()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
    }
    // Start is called before the first frame update
    public void ClearEndBs(int questIndex)
    {
        Debug.Log("Clear End Boss ½ÇÇà");
        dataMgrDontDestroy.QuestCurCnt++;        
        if(dataMgrDontDestroy.QuestCurCnt == dataMgrDontDestroy.QuestMaxCnt)
        {
            dataMgrDontDestroy.isCompleted = true;
        }
        questPopUpManager.UpdateQuestStatus();
    }
}
