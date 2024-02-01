using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillbording : MonoBehaviour //HUDCanvas(World space설정함)에 달면, 각도가 바뀌어도 HUD가 카메라를 계속 주시함. 
{
    private CinemachineVirtualCamera cam;

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        transform.forward = cam.transform.forward;
    }
}