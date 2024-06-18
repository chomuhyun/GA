using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAttribute  : MonoBehaviour
{
    public StateManager sm;
    public float atkPer;

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss") && sm != null)
            {
               sm.DealDamage(other.GetComponent<StateManager>().gameObject, atkPer);
            }
        if (other.gameObject.CompareTag("NomalMonster") && sm != null)
        {
            sm.DealDamage(other.GetComponent<StateManager>().gameObject, atkPer);
        }
    }
}
