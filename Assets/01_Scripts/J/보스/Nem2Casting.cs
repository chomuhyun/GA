using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nem2Casting : MonoBehaviour
{

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
        StartCoroutine(Casting());
    }


    IEnumerator Casting()
    {
        while (bombArea.transform.localScale.x < size)
        {
            castingTime += Time.deltaTime;
            float speed = 0.000001f;
            bombArea.transform.localScale += new Vector3(castingTime * speed * 0.03f * Time.deltaTime, 0, castingTime * speed * 0.03f * Time.deltaTime);


            if (bombArea.transform.localScale.x >= size)
            {
                GameObject effcet;
                effcet = Instantiate(Effect, transform.position, transform.rotation);
                //effcet.GetComponent<BossWeapons>().sm = GameObject.FindGameObjectWithTag("Boss").GetComponent<StateManager>();
                castingTime = 0;
                Destroy(gameObject, 0.1f);
                Destroy(effcet, 9f);
                break;
            }
            yield return null;
        }
    }
}
