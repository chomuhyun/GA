using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
public class CameraFollow : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 currPos;
    private Quaternion currRot;
    private Transform tr;
    public PhotonView pv;
    public Transform playerTransform;
    public Vector3 Offset; 
    public bool IsTransparent { get; private set; } = false;
    MeshRenderer[] renderers;
    WaitForSeconds delay = new WaitForSeconds(0.001f);
    WaitForSeconds resetDelay = new WaitForSeconds(0.005f);
    const float THRESHOLD_ALPHA = 0.25f;
    const float THRESHOLD_MAX_TIMER = 0.5f;
    
    bool isReseting = false;
    float timer = 0f;
    Coroutine timeCheckCoroutine;
    Coroutine resetCoroutine;
    Coroutine becomeTransparentCoroutine;

    public Transform CameraArm;
    private float xRotateMove, yRotateMove;
    
    public float rotateSpeed = 500.0f;
    void Awake()
    {
        if(playerTransform != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        
        //Offset = transform.position - playerTransform.position;     
        renderers = GetComponentsInChildren<MeshRenderer>();
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            check();
        }
        lookAround();
    }

    void check()
    {
        transform.position = playerTransform.position;
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("Filed"));
        for (int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();

            for (int j = 0; j < obj.Length; j++)
            {
                obj[j]?.BecomeTransparent();
            }
        }
    }
    void lookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = CameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        CameraArm.rotation = Quaternion.Euler(0, camAngle.y + mouseDelta.x, camAngle.z);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        //클론이 통신을 받는 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        
    }
}

   