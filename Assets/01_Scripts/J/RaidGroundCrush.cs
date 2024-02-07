using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidGroundCrush : MonoBehaviour
{
    public RaidBossCtrl raid;
    void Start()
    {
        if(raid != null)
        {
            raid = GameObject.FindGameObjectWithTag("Boss").GetComponent<RaidBossCtrl>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(raid.GetComponent<StateManager>().hp < raid.GetComponent<StateManager>().maxhp / 2)
        {

        }
         
    }
}
