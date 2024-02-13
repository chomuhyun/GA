using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class C_Spawn_collider : MonoBehaviour
{
    public ChaosDungeonMgr cDunMgr;

    public bool isUsed1;
    public bool isUsed2;
    public bool isUsed3;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUsed1 && gameObject.name == "Ground (1)")
        {
                cDunMgr.InstBoss1();
            
                isUsed1 = true;
        }
        if (other.CompareTag("Player") && !isUsed2 && gameObject.name == "Ground (2)")
        {
            if (cDunMgr.bossKilled == 1)
            {
                cDunMgr.InstBoss2();
                
                isUsed2 = true;
            }
        }
        if (other.CompareTag("Player") && !isUsed3 && gameObject.name == "Ground (3)")
        {
            if (cDunMgr.bossKilled == 2)
            {
                cDunMgr.InstBoss3();
                
                isUsed3 = true;
            }
        }
    }
}
