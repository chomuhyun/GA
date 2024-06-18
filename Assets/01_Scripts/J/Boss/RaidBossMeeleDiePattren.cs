using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidBossMeeleDiePattren : MonoBehaviour
{
    public GameObject bombArea;
    public float castingTime;
    Animator anim;
    public GameObject Effect;
    float size = 1f;
    public float offPos;
    Vector3 maxSize;
    Vector3 originSize;
    public ObjSound objSound;
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>();
        originSize = bombArea.transform.localScale;
        objSound = GameObject.FindGameObjectWithTag("OS").GetComponent<ObjSound>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Casting());
    }

    IEnumerator Casting()
    {
        while (bombArea.transform.localScale.z < size)
        {
            castingTime += Time.deltaTime;
            float speed = 0.00001f;
            bombArea.transform.localScale += new Vector3(0, 0, castingTime * speed * 3f * Time.deltaTime);
            
            if (bombArea.transform.localScale.z >= size)
            {
                GameObject effcet;
                Vector3 Pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + offPos);
                effcet = Instantiate(Effect, Pos, transform.rotation);
                effcet.GetComponent<BossWeapons>().sm = GameObject.FindGameObjectWithTag("Boss").GetComponent<StateManager>();
                castingTime = 0;
                Destroy(gameObject, 0.1f);
                Destroy(effcet,3f);
                objSound.BossSwordPatternSound();
                break;
            }
            yield return null;
        }
    }
}
