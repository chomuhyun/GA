using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnScipt : MonoBehaviourPunCallbacks
{
    public GameObject[] cH;
    //public GameObject vc;
    public int classNum;
    //GameObject player;
    //public ThirdPersonOrbitCamBasicA thrid;

    private void Start()
    {
        //thrid = GameObject.Find("Virtual Camera").GetComponent<ThirdPersonOrbitCamBasicA>();
        StartCoroutine(SpawnPlayer());
    }
    IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(0.01f);
        CreatePlayer();
    }
    public void CreatePlayer()
    {
        int curSlotNum = SelectSlot.slotNum;
        int classNum = PlayerPrefs.GetInt($"{curSlotNum}_ClassNum");
        Debug.Log(classNum);
        Debug.Log("spawnScriptø°º≠ curSlotNum¿∫ "+curSlotNum);
        if (PhotonNetwork.IsConnected)
        {
            Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
            int idx = Random.Range(1, points.Length);
            //GameObject obj;
            //obj = PhotonNetwork.Instantiate(cH[classNum].name, points[idx].position, points[idx].rotation, 0);
            PhotonNetwork.Instantiate(cH[classNum].name, points[idx].position, points[idx].rotation, 0);
        }
    }
}
