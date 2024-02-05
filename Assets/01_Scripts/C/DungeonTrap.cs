using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTrap : MonoBehaviour
{
    public int damage = 10;
    private bool playerInside = false;
    private float timer = 0f;

    private void Update()
    {
        if (playerInside)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                DealDamageToPlayer();
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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

    private void DealDamageToPlayer()
    {

        Debug.Log("장판 밟으면 초당 체력 10씩 까임.");

    }
}
