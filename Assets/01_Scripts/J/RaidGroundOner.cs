using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidGroundOner : MonoBehaviour
{   
    public MeshRenderer ground;
    public MeshRenderer dissol;
    public GameObject Effect;
    public ObjSound os;
    public bool sound = true;
    void Start()
    { 
        dissol.enabled = false;
        os = GetComponent<ObjSound>();
    }

    // Update is called once per frame
   public void CrushOner()
    {
            {
                dissol.enabled = true;
            StartCoroutine(Delay());
            StartCoroutine(Delay2());
        }
    } 
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(2f);
        
        GameObject obj;
            obj = Instantiate(Effect, transform.position, transform.rotation);
            Destroy(obj, 3f);
    }
    IEnumerator Delay2()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    public void sounds()
    {
        if (sound)
        {
            os.GroundCrush();
            sound = false;
        }
    }
}
    
