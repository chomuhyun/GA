using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObj : MonoBehaviour
{
    public float floatSpeed = 1.0f; // 상하 운동 속도
    public float floatHeight = 1.0f; // 물체가 떠 있는 높이

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 물체를 일정한 속도로 상하 운동시킴
        transform.position = startPosition + new Vector3(0.0f, Mathf.Sin(Time.time * floatSpeed) * floatHeight, 0.0f);
    }
}
