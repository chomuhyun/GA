using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidBossSpwan : MonoBehaviour
{
    public GameObject raidBoss;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            raidBoss.SetActive(true);
            Destroy(gameObject, 1f);
        }
    }
}
