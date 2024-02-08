using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeBossSpwan : MonoBehaviour
{
    public GameObject neBoss;
    public GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            neBoss.SetActive(true);
            Destroy(gameObject, 1f);
            wall.transform.position = new Vector3(transform.position.x, -Time.deltaTime, transform.position.z);
            if(wall.transform.position.y <= -3f)
            {
                Destroy(wall, 1f);
            }
        }
    }
}
