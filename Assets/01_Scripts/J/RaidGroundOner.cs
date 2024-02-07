using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidGroundOner : MonoBehaviour
{
    public MeshRenderer ground;
    public MeshRenderer dissol;
    public GameObject Effect;

    void Start()
    { 
        dissol.enabled = false;
    }

    // Update is called once per frame
   public void CrushOner()
    {
            {
                dissol.enabled = true;
                StartCoroutine(Delay());
                Destroy(gameObject, 2.5f);
            }
    }
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(2f);
            GameObject obj;
            obj = Instantiate(Effect, transform.position, transform.rotation);
            Destroy(obj, 3f);
        }
}
    
