using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveZoneOut : MonoBehaviour
{
    public float DestroyTime = 11f;
    public Player player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, DestroyTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.isDeshInvincible = false;
            Destroy(gameObject);
        }
    }
}
