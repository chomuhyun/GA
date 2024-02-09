using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBoxCollider : MonoBehaviour
{

    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = new Vector3(0, 1, 0);
            Debug.Log("SEs");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
