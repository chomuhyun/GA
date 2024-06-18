using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponsAllDie : MonoBehaviour
{
    public StateManager sm;
    public float atkPer;
    public Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (player.pv.IsMine)
        {
            if (player.isDeshInvincible == true)
                return;

            if (other.gameObject.CompareTag("Player"))
            {
                sm.DealDamage(other.GetComponent<StateManager>().gameObject, atkPer);
            }
        }
    }
}
