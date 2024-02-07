using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public DataMgrDontDestroy dataMgrDontDestroy;
    public GameObject tutorialPanel;

    private void Start()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        if (dataMgrDontDestroy.IsFirst)
        {
            tutorialPanel.SetActive(true);
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void OnClickCloseBtn()
    {
        dataMgrDontDestroy.IsFirst = false;
        tutorialPanel.SetActive(false);
    }
}
