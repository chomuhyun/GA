using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Cinemachine;

public class CrushSaveBall : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 currPos;
    private Quaternion currRot;
    public GameObject SaveZone;
    public StateManager Sm;
    void Start()
    {
        Sm = GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Sm.hp <= 0)
        {
            Instantiate(SaveZone, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        Destroy(gameObject, 15f);
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
}
