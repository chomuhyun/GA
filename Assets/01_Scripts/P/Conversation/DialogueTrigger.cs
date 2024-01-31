using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue info;

    public void Trigger()
    {
        var system = FindObjectOfType<TalkMgr>();
        system.Begin(info);
    }

    public void EndTrigger()
    {
        var system = FindObjectOfType<TalkMgr>();
        system.RealEnd();
    }
}
// 해당 객체의 스크립트의 Trigger 함수를 불러오면 스크립트에 있는 대화를 순서대로 가져옴.