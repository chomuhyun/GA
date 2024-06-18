using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapons : MonoBehaviour
{
    public StateManager sm;
    public float atkPer;
    public Player player;
    Tboss tboss;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player.isDeshInvincible == true)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            sm.DealDamage(other.GetComponent<StateManager>().gameObject, atkPer);
        }
    }
}
