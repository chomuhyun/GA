using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotClass
{
    Slot0, Slot1, Slot2
}

public class SelectSlot : MonoBehaviour
{
    public string currentSlot = null;
    public static int slotNum;
    public SlotClass slot;
    public SelectSlot[] chars;

    public void OnClickSlotBtn()
    {
        // 해당 버튼의 slot을 현재 스크립트의 currentSlot에 할당
        currentSlot = slot.ToString();
        // 슬롯번호를 다른스크립트에 넘겨주기
        switch (currentSlot)
        {
            case "Slot0":
                slotNum = 0;
                break;
            case "Slot1":
                slotNum = 1;
                break;
            case "Slot2":
                slotNum = 2;
                break;
            default:
                break;
        }
        // 할당된 슬롯번호 확인
        Debug.Log("Selected Slot: " + currentSlot);
    }
}
