using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTrap : MonoBehaviour
{

    private bool playerInside = false;
    private float timer = 0f;

    public GameObject trappedPlayer;
    public StateManager sm;


    private void Start()
    {
        sm = GetComponent<StateManager>();
    }

    private void Update()
    {
        if (playerInside)
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                trappedPlayer.GetComponent<HUDManager>().ChangeUserHUD();

                if (trappedPlayer.gameObject.CompareTag("Player"))
                {
                    sm.DealDamage(trappedPlayer.GetComponent<StateManager>().gameObject, 100);
                }
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trappedPlayer = other.gameObject;
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            playerInside = false;
            timer = 0f;
        }
    }
}
