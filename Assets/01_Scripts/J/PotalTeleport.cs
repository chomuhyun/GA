using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;







// private void OnTriggerStay(Collider other)
//{
//   if(other.gameObject.CompareTag("Player"))
//   {
//       other.transform.position = teleportPos.position;
//   }
//}

//     loadPanel.fillAmount = -Time.deltaTime;
// if(loadPanel.fillAmount == 0)
//  {
//          Destroy(loadPanel);
//  }
public class MoveTowardsOtherTransform : MonoBehaviour
{
    public Transform targetTransform; // 이동하려는 대상의 Transform
    public float moveSpeed = 5f; // 이동 속도

    void Update()
    {
        // 만약 targetTransform이 존재하지 않는다면 리턴
        if (targetTransform == null)
        {
            Debug.LogWarning("Target Transform is not assigned.");
            return;
        }

        // 현재 위치에서 목표 위치로의 방향을 계산
        Vector3 direction = targetTransform.position - transform.position;

        // 목표 위치에 도달했는지 검사하고 이동 거리를 계산
        float distanceToMove = Mathf.Min(moveSpeed * Time.deltaTime, direction.magnitude);

        // 이동 거리가 0보다 클 경우에만 이동
        if (distanceToMove > 0f)
        {
            // 실제로 이동
            transform.position += direction.normalized * distanceToMove;
        }
    }
}

