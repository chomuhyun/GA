using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePortMgr : MonoBehaviour
{
    public Transform targetTransform; // 이동하려는 대상의 Transform
    public float moveSpeed = 5f; // 이동 속도

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = targetTransform.position;
        }
    }

}