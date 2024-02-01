using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillbording : MonoBehaviour //HUDCanvas(World space������)�� �޸�, ������ �ٲ� HUD�� ī�޶� ��� �ֽ���. 
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