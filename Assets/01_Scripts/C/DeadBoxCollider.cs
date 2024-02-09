using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DeadBoxCollider : MonoBehaviour
{
    public ChaosDungeonMgr cDungeonMgr;

    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = cDungeonMgr.spawnPoint[0].position;
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
