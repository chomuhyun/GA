using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NomalMonsterCtrl : MonoBehaviour
{
    public ChaosDungeonMgr cDunMgr;
    public GameObject bossDeathEffect;


    public enum GOLEM
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        HIT,
        DIE,
        NULL
    }

    [Header("Com")]
    public GOLEM golem;
    public Animator golemAnim;
    public Transform targetPlayer;
    public NavMeshAgent navAgent;
    public CapsuleCollider col;
    public BoxCollider rhitBox;
    public BoxCollider lhitBox;
    public StateManager golemStateManager;
    public GameObject originMesh;
    public SkinnedMeshRenderer dieMesh;

    [Header("Stet")]
    public float golemspeed; // ÀÜ¸÷Àº ´À¸²
    public float golemrange; //ÀÜ¸÷Àº ÇÑ¹ø Å¸°ÙÆÃ µÇ¸é Á×À» ¶§ ±îÁö Ã¼ÀÌ½Ì
    public float golemattackRange;// °ñ·½ ÆÈ ÂªÀ½
    public bool golemisDeath;
    public bool attacking;
    void Start()
    {
        golem = GOLEM.IDLE;
        golemAnim = GetComponent<Animator>();
        targetPlayer = GameObject.FindWithTag("Player").GetComponent<Transform>();
        navAgent = GetComponent<NavMeshAgent>(); 
        col = GetComponent<CapsuleCollider>();
        golemStateManager = GetComponent<StateManager>();
        originMesh.SetActive(true);
        dieMesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (!golemisDeath)
        {
            Die();
            switch (golem)
            {

                case GOLEM.IDLE:
                    golemAnim.SetInteger("GOLEM", 0);
                    navAgent.speed = 0;

                    float dist = Vector3.Distance(targetPlayer.position, transform.position);
                    if (dist < golemrange)
                    {
                        golem = GOLEM.MOVE;
                    }
                    else
                    {
                        golem = GOLEM.IDLE;
                    }
                    break;
                case GOLEM.MOVE:
                    golemAnim.SetInteger("GOLEM", 1);
                    navAgent.speed = golemspeed;

                    float distan = Vector3.Distance(targetPlayer.position, transform.position);
                    if (distan < golemattackRange)
                    {
                        golem = GOLEM.ATTACK;
                    }
                    else
                    {
                        navAgent.SetDestination(targetPlayer.position + new Vector3(-1.5f, 0, 0f));
                    }
                    if (distan > golemrange)
                    {
                        golem = GOLEM.IDLE;
                    }
                    break;
                case GOLEM.ATTACK:
                    golemAnim.SetInteger("GOLEM", 2);
                    navAgent.speed = 0;
                    attacking = true;
                    if (attacking == true)
                    {
                        navAgent.speed = 0;
                    }
                    else
                    {
                        navAgent.speed = golemspeed;
                    }
                    float dists = Vector3.Distance(targetPlayer.position, transform.position);
                    if (dists > golemattackRange)
                    {
                        attacking = false;
                        golem = GOLEM.MOVE;
                    }
                    else
                    {
                        golem = GOLEM.ATTACK;
                    }
                    break;
                case GOLEM.HIT:
                    golemAnim.SetInteger("GOLEM", 3);
                    navAgent.speed = 0;
                    if(golemStateManager.hp <=0)
                    {
                        Die();
                    }
                    float distans = Vector3.Distance(targetPlayer.position, transform.position);
                    if (distans > golemattackRange)
                    {
                        golem = GOLEM.IDLE;
                    }
                    else
                    {
                        golem = GOLEM.ATTACK;
                    }

                    break;
                case GOLEM.DIE:
                    StartCoroutine(DeadProcess(5f));
                    navAgent.enabled = false;
                    if(gameObject.tag == "Boss")
                    {
                        Instantiate(bossDeathEffect, transform);
                        cDunMgr = GameObject.Find("ChaosDungeonMgr").GetComponent<ChaosDungeonMgr>();
                        cDunMgr.ClearEndBoss();
                    }
                    break;
                case GOLEM.NULL:
                    break;

            }
        }
    }
    public void Die()
    {
        if(golemStateManager.hp <= 0)
        {
            navAgent.speed = 0;
            golemisDeath = true;
            col.enabled = false;
            StartCoroutine(DeathDelay());
        }
    }

    
    IEnumerator DeadProcess(float t)
    {
        rhitBox.enabled = false;
        lhitBox.enabled = false;
        originMesh.SetActive(false);
        dieMesh.enabled = true;
        yield return new WaitForSeconds(t);
        while (transform.position.y > -t)
        {
            Vector3 temp = transform.position;
            temp.y -= Time.deltaTime;
            transform.position = temp;
            yield return new WaitForEndOfFrame();
        }
        InitEnemy();
        gameObject.SetActive(false);
    }

    IEnumerator DeathDelay()
    {
        golemAnim.SetTrigger("DIE");
        golem = GOLEM.DIE;
        yield return null;
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(9f);
        gameObject.SetActive(false);
    }
    void InitEnemy()
    {
        golemStateManager.hp = 4000f;
        golem = GOLEM.IDLE;
        col.enabled = true;
        navAgent.enabled = true;
        golemisDeath = false;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        originMesh.SetActive(true);
        dieMesh.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            navAgent.speed = 0;
            golem = GOLEM.HIT;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("TimeSlow"))
        {
            golemAnim.speed = 0.15f;
            golemspeed = 1f;
        }
        else
        {
            golemspeed = 3f;
            golemAnim.speed = 1f;
        }
    }

    void AttackBoxActive(int isEnable)
    {
        if (rhitBox != null)
        {
            var rcol = rhitBox.GetComponent<BoxCollider>();
            var lcol = lhitBox.GetComponent<BoxCollider>();
            if (rcol != null && lcol != null)
            {
                if (isEnable == 1)
                {
                    rcol.enabled = true;
                    lcol.enabled = true;
                }
                else
                {
                    rcol.enabled = false;
                    rcol.enabled = false;
                }
            }
        }
    }

}

