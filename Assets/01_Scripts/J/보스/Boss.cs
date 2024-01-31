using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public enum BOSSSTATE
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        DOWN,
        DIE,
        NEM1,
        NEM2,
        NEM3
    }

    [Header("Com")]
    public BOSSSTATE bossState;
    public Animator bossAnim;
    public Transform targetPlayer;
    public NavMeshAgent nvAgent;
    public CharacterController characterController;
    public Rigidbody rigidbody;
    public BoxCollider hitBox;
    public BoxCollider neM1area;
    private List<GameObject> gameObjects = new List<GameObject>();
    public GameObject[] prefabs;
    public Player player;
    StateManager stateManager;
    public GameObject nem2Area;
    public GameObject safeZoneBall;

    [Header("Stet")]
    public float speed;
    public float range;
    
    public float attackRange;
    public int bossDamage;

    public bool isDeath;

    [Header("Tech")]
    public bool attacking;
    public float patternTime;
    public float nem2PatternTime;
    public float originalTimeScale;
    public List<GameObject> collidedEnemies = new List<GameObject>();
    void Start()
    {
        bossAnim = GetComponent<Animator>();
        nvAgent = GetComponent<NavMeshAgent>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        bossState = BOSSSTATE.IDLE;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        stateManager = GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        patternTime += Time.deltaTime;
        nem2PatternTime += Time.deltaTime;
        NemStart();
        Nem2Start();
        if (isDeath != true)
        {
            Die();

            switch (bossState)
            {
                case BOSSSTATE.IDLE:
                    bossAnim.SetInteger("BOSSSTATE", 0);
                    nvAgent.speed = 0;

                    float dist = Vector3.Distance(targetPlayer.position, transform.position);
                    if (dist < range)
                    {
                        bossState = BOSSSTATE.MOVE;
                    }
                    else
                    {
                        bossState = BOSSSTATE.IDLE;
                    }


                    break;
                case BOSSSTATE.MOVE:
                    bossAnim.SetInteger("BOSSSTATE", 1);
                    nvAgent.speed = speed;

                    float distan = Vector3.Distance(targetPlayer.position, transform.position);
                    if (distan < attackRange)
                    {
                        bossState = BOSSSTATE.ATTACK;
                    }
                    else
                    {
                        nvAgent.SetDestination(targetPlayer.position + new Vector3(0, 0, -2f));
                    }
                    if (distan > range)
                    {
                        bossState = BOSSSTATE.IDLE;
                    }


                    break;
                case BOSSSTATE.ATTACK:
                    bossAnim.SetInteger("BOSSSTATE", 2);
                    nvAgent.speed = 0;
                    attacking = true;
                    if (attacking == true)
                    {
                        nvAgent.speed = 0;
                    }
                    else
                    {
                        nvAgent.speed = speed;

                    }
                    float dists = Vector3.Distance(targetPlayer.position, transform.position);
                    if (dists > attackRange)
                    {
                        attacking = false;
                        bossState = BOSSSTATE.MOVE;
                    }
                    else
                    {
                        bossState = BOSSSTATE.ATTACK;
                    }


                    break;
                case BOSSSTATE.DOWN:
                    bossAnim.SetInteger("BOSSSTATE", 3);
                    nvAgent.speed = 0;
                    attacking = true;
                    if (attacking == true)
                    {
                        nvAgent.speed = 0;
                    }
                    else
                    {
                        nvAgent.speed = speed;

                    }
                    StartCoroutine(StandUp());

                    break;
                case BOSSSTATE.DIE:


                    break;

                case BOSSSTATE.NEM1:
                    bossAnim.SetInteger("BOSSSTATE", 5);
                    if (patternTime > Random.Range(30, 39))
                    {
                        StartCoroutine(Nem1delay());
                        Spawn();
                    }
                    break;
                case BOSSSTATE.NEM2:
                    bossAnim.SetInteger("BOSSSTATE", 5);
                    
                    Nem2();
                    
                    break;
                default:
                    break;
            }
        }
    }

    public void Die()
    {
        if(stateManager.hp <=0)
        {
            nvAgent.speed = 0;
            isDeath = true;
            characterController.enabled = false;
            StartCoroutine(DeathDelay());
        }
    }
    IEnumerator DeathDelay()
    {
        bossAnim.SetTrigger("Die");
        yield return null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DownSkill"))
        {
            nvAgent.speed = 0;
            bossState = BOSSSTATE.DOWN;
        }
    }
     void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("TimeSlow"))
        {
            bossAnim.speed = 0.15f;
            speed = 1f;
        }
        else
        {
            speed = 3f;
            bossAnim.speed = 1f;
        }
    }
    

    IEnumerator StandUp() 
    {
        yield return new WaitForSeconds(3f);
        float distans = Vector3.Distance(targetPlayer.position, transform.position);
        if (distans > attackRange)
        {
            attacking = false;
            bossState = BOSSSTATE.MOVE;
        }
        else
        {
            bossState = BOSSSTATE.ATTACK;
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;

        Vector3 size = neM1area.size;

        float posX = basePosition.x + Random.Range(-size.x, size.x);
        float posY = basePosition.y + Random.Range(0,size.y);
        float posZ = basePosition.z + Random.Range(-size.z, size.z);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);

        return spawnPos;
    }
    private void NemStart()
    {
        if (patternTime >= 30)
        {
            bossState = BOSSSTATE.NEM1;
        }
    }
    void Nem2Start()
    {
        if (nem2PatternTime >= 120f)
        {
            Nem2();
            nem2PatternTime = 0;
        }
    }
    void Spawn()
    {

        int selection = Random.Range(0, prefabs.Length);

        GameObject selectedPrefab = prefabs[selection];

        Vector3 spawnPos = GetRandomPosition();

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.Euler(0, 0, 0));
        gameObjects.Add(instance);

        bossState = BOSSSTATE.IDLE;
    }

    private Vector3 SaveBallPosition()
    {
        Vector3 basePosition = transform.position;

        Vector3 size = neM1area.size;

        float posX = basePosition.x + Random.Range(-size.x, size.x);
        float posY = basePosition.y + 1f;
        float posZ = basePosition.z + Random.Range(-size.z, size.z);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);

        return spawnPos;
    }
    void Nem2()
    {
        Vector3 spawnPos = SaveBallPosition();

        Instantiate(safeZoneBall, spawnPos, Quaternion.Euler(0, 0, 0));
        Instantiate(nem2Area, transform.position, transform.rotation);
       
    }
    IEnumerator Nem1delay()
    {
        yield return new WaitForSeconds(4.5f);
        patternTime = 0;
    }
}
