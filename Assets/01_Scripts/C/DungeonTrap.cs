using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTrap : MonoBehaviour
{
    public int damage = 10;
    private bool playerInside = false;
    private float timer = 0f;

    public GameObject trappedPlayer;


    private void Update()
    {
        if (playerInside)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                trappedPlayer.GetComponent<StateManager>().hp -= 10;
                Debug.Log("장판 밟으면 초당 체력 10씩 까임.");
                trappedPlayer.GetComponent<HUDManager>().ChangeUserHUD();
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
