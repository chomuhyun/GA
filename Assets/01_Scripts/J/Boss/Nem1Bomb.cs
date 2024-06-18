using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nem1Bomb : MonoBehaviour
{
    
    public GameObject gameObject;
    public GameObject bombArea;
    public float castingTime;

    public GameObject Effect;
    float size = 1f;
    Vector3 maxSize;
    Vector3 originSize;
    void Start()
    {
        originSize = bombArea.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Up());
    }

    IEnumerator casting()
    {
        castingTime += Time.deltaTime;
        bombArea.transform.localScale += new Vector3(castingTime , 0, castingTime );
        
        yield return new WaitForSeconds(0.05f);

        if(bombArea.transform.localScale == new Vector3(1,0,1))
        {
            Debug.Log("?");
            
        }
    }

    IEnumerator Up()
    {
        while (bombArea.transform.localScale.x < size)
        {
            castingTime += Time.deltaTime;
            float speed = 0.0001f;
            bombArea.transform.localScale += new Vector3(castingTime * speed * 0.001f, 0, castingTime * speed * 0.001f);
            

            if (bombArea.transform.localScale.x >= size)
            {
                GameObject effcet;
                effcet = Instantiate(Effect, transform.position, transform.rotation);
                effcet.GetComponent<BossWeapons>().sm = GameObject.FindGameObjectWithTag("Boss").GetComponent<StateManager>();
                castingTime = 0;
                Destroy(gameObject, 0.5f);
                Destroy(effcet, 0.5f);
                break;
            }
            yield return null;
        }
    }
}
