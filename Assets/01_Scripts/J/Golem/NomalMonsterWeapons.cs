using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalMonsterWeapons : MonoBehaviour
{
    public StateManager sm;
    public float atkPer;
    public Player player;
  

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        sm = GetComponentInParent<StateManager>();
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
