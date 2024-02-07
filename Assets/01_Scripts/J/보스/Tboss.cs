using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Cinemachine;

public class Tboss : MonoBehaviourPunCallbacks, IPunObservable
{
    public ChaosDungeonMgr cDunMgr;
    public enum TBOSS
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        BREAK,
        DOWN,
        DIE,
        PAGE1,
        PAGE2,
        PAGE3,
    }

    public float detectionRadius = 10f;
    public string playerTag = "Player";
    private Transform currentTarget;
    Collider[] player_s;
    public GameObject[] nearbyPlayer;
    private Vector3 currPos;
    private Quaternion currRot;
    [Header("Move")]
    public float speed = 2.5f;
    public float rotSpeed = 5f;
    public float range = 8f;
    public float attakRange = 3f;
    public Transform targetPlayer;
    public bool isActivating;

    [Header("Component")]
    public CharacterController characterController;
    public TBOSS  tBOSS;
    public Animator tbanim;
    public StateManager stateManager;
    public Player player;
    public BoxCollider nem1Area;
    public PhotonView pv;
    [Header("AttackPattern")]
    public float p1;
    public float p2;
    public float p3;
    public float p4;
    public float p1check;
    public float p2check;
    public float p3check;
    public float p4check;
    public float page1;
    public float page2;
    public float page3;
    public float page1check;
    public float page2check;
    public float page3check;

    public bool p1Ready;
    public bool p2Ready;
    public bool p3Ready;
    public bool p4Ready;
    public bool down;
    public bool attacking;

    public float breakTime;
    public float breakCheck;
    public bool breakOn;
    public bool die;
    public bool sword;
    public bool twsword;
    public GameObject[] weapons;
    public GameObject downPattern;
    public GameObject[] page1Casting;
    private List<GameObject> gameObjects = new List<GameObject>();

    [Header("PatternEffect")]
    public GameObject[] patternE;
    public GameObject[] patternSkillActive;
    
    void Start()
    {
        cDunMgr = GameObject.Find("ChaosDungeonMgr").GetComponent<ChaosDungeonMgr>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        testGameMgr someComponent = GameObject.FindWithTag("Player").GetComponent<testGameMgr>();
        //if (someComponent != null)
        //{
        //someComponent.Starts();
        //}
        tBOSS = TBOSS.IDLE;
        characterController = GetComponent<CharacterController>();
        tbanim = GetComponent<Animator>();
        stateManager = GetComponent<StateManager>();
        if (pv.IsMine)
        {
            //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            //targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            //testGameMgr someComponent = GameObject.FindWithTag("Player").GetComponent<testGameMgr>();
            ////if (someComponent != null)
            ////{
            //    //someComponent.Starts();
            ////}
            //tBOSS = TBOSS.IDLE;
            //characterController = GetComponent<CharacterController>();
            //tbanim = GetComponent<Animator>();
            //stateManager = GetComponent<StateManager>();
        }
    }
    //테스트 용
    public void Starts()
    {
        {
            if (player != null)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            }
        }
    }
    void Update()
    {
        //if(pv.IsMine)
        //{
            if (!die)
            {
                NearByPlayer();
                    breakTiming();
                    BreakTime();
                    Dieing();
                    PatternTimeCheck();
                    NEM1();
                    NEM2();
                    switch (tBOSS)
                    {
                        case TBOSS.IDLE:
                            isActivating = false;
                            tbanim.SetTrigger("IDLE");
                            float dist = Vector3.Distance(targetPlayer.position, transform.position);
                            if (dist < range)
                            {
                                tBOSS = TBOSS.MOVE;
                            }
                            else
                            {
                                tBOSS = TBOSS.IDLE;
                            }
                            break;
                        case TBOSS.MOVE:
                            StartCoroutine(MoveDelay());
                            if (down)
                                return;
                            if (attacking)
                                return;
                            float dis = Vector3.Distance(targetPlayer.position, transform.position);
                            if (dis > 1f)
                            {
                                isActivating = false;
                            }
                            if (dis < attakRange)
                            {
                                tBOSS = TBOSS.ATTACK;
                            }
                            speed = 3f;
                            MoveTowardsTarget(true);
                            tbanim.SetTrigger("RUN");
                            float distan = Vector3.Distance(targetPlayer.position, transform.position);
                            if (distan > 18)
                            {
                                tBOSS = TBOSS.IDLE;
                            }
                            break;
                        case TBOSS.ATTACK:
                            isActivating = true;
                            speed = 0f;
                            float dists = Vector3.Distance(targetPlayer.position, transform.position);
                            if (p1Ready)
                            {
                                if (dists < attakRange)
                                {
                                    if (sword)
                                    {

                                        tbanim.SetTrigger("Pattern1");
                                        p1 = 0;
                                        p1Ready = false;
                                        attacking = true;
                                    }
                                }
                            }
                            if (p2Ready)
                            {
                                if (dists < attakRange)
                                {
                                    if (sword)
                                    {

                                        tbanim.SetTrigger("Pattern2");
                                        p2 = 0;
                                        p2Ready = false;
                                        attacking = true;
                                    }
                                }
                            }
                            if (p3Ready)
                            {
                                if (dists < attakRange)
                                {
                                    if (twsword)
                                    {

                                        tbanim.SetTrigger("Pattern3");
                                        p3 = 0;
                                        p3Ready = false;
                                        attacking = true;
                                    }
                                }
                            }
                            if (p4Ready)
                            {
                                if (dists < attakRange)
                                {
                                    if (twsword)
                                    {
                                        tbanim.SetTrigger("Pattern4");
                                        p4 = 0;
                                        p4Ready = false;
                                        attacking = true;
                                    }
                                }
                            }
                            if (dists > attakRange)
                            {
                                tBOSS = TBOSS.MOVE;
                            }
                            else
                            {
                                tBOSS = TBOSS.ATTACK;
                            }
                            break;
                        case TBOSS.BREAK:
                            isActivating = true;
                            speed = 0f;
                            breakTime = 0f;
                            StartCoroutine(breakTiming());
                            break;
                        case TBOSS.DOWN:
                            isActivating = true;
                            down = true;
                            speed = 0f;
                            float dista = Vector3.Distance(targetPlayer.position, transform.position);
                            tbanim.SetTrigger("DOWN");
                            if (dista > attakRange)
                            {
                                tBOSS = TBOSS.MOVE;
                            }
                            else
                            {
                                tBOSS = TBOSS.ATTACK;
                            }
                            break;
                        case TBOSS.DIE:
                            isActivating = true;
                            speed = 0;
                            cDunMgr.bossKilled++;
                            cDunMgr.ClearMidBoss();
                            tbanim.SetTrigger("DIE");           
                            break;
                        case TBOSS.PAGE1:
                            isActivating = true;
                            page1 = 0;
                            InvokeRepeating("Spawn", 0.01f, 0.2f);
                            StartCoroutine(Page1Start());
                            break;
                        case TBOSS.PAGE2:
                            page2 = 0;
                            isActivating = true;
                            
                            break;
                    }
            // }
        }
        void MoveTowardsTarget(bool stop)
        {
            if (isActivating)
                return;
            Vector3 directionToTarget = targetPlayer.transform.position - transform.position;
            directionToTarget.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotSpeed * Time.deltaTime);

            Vector3 moveDirection = directionToTarget.normalized;
            characterController.SimpleMove(moveDirection * speed);

            if (Vector3.Distance(transform.position, targetPlayer.transform.position) < 1.5f)
            {
                tBOSS = TBOSS.ATTACK;
            }
        }
        IEnumerator MoveDelay()
        {
            yield return new WaitForSeconds(4.5f);
            down = false;
            yield return new WaitForSeconds(3f);
            attacking = false;
        }

        void PatternTimeCheck()
        {
            p1 += Time.deltaTime;
            if (p1 >= p1check)
            {
                p1 = p1check;
                p1Ready = true;
            }
            p2 += Time.deltaTime;
            if (p2 >= p2check)
            {
                p2 = p2check;
                p2Ready = true;
            }
            p3 += Time.deltaTime;
            if (p3 >= p3check)
            {
                p3 = p3check;
                p3Ready = true;
            }
            p4 += Time.deltaTime;
            if (p4 >= p4check)
            {
                p4 = p4check;
                p4Ready = true;
            }
        }
    }

    void NearByPlayer()
    {
        nearbyPlayer = GameObject.FindGameObjectsWithTag("Player");
        targetPlayer = GetNearestPlayer(nearbyPlayer);
    }
    Transform GetNearestPlayer(GameObject[] players)
    {
        Transform nearestPlayer = null;
        float shortestDistance = float.MaxValue;

        foreach (var player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player.transform;
            }
        }

        return nearestPlayer;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (breakOn)
        {
            if (other.CompareTag("DownSkill"))
            {
                tBOSS = TBOSS.DOWN;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("TimeSlow"))
        {
            tbanim.speed = 0.15f;
            speed = 0.3f;
        }
        else
        {
            speed = 3f;
            tbanim.speed = 1f;
        }
    }
    void Dieing()
    {
        if (stateManager.hp <= 0)
        {
            die = true;
            tBOSS = TBOSS.DIE;
            characterController.enabled = false;
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            weapons[2].SetActive(false);
        }
    }
    void BreakTime()
    {
        breakTime += Time.deltaTime;
        if (breakTime >= breakCheck)
        {
            tbanim.SetTrigger("Break");
            tBOSS = TBOSS.BREAK;
            breakOn = true;
            breakTime = breakCheck;
        }
    }
    IEnumerator breakTiming()
    {
        yield return new WaitForSeconds(3f);
        float dista = Vector3.Distance(targetPlayer.position, transform.position);
        if (dista > attakRange)
        {
            tBOSS = TBOSS.MOVE;
        }
        else
        {
            tBOSS = TBOSS.ATTACK;
        }
        breakOn = false;
        downPattern.SetActive(true);
        yield return new WaitForSeconds(2f);
        downPattern.SetActive(false);
    }
    void WeaponChangeChecker()
    {
        sword = true;
        twsword = false;
    }
    void TWWeaponChangeChecker()
    {
        sword = false;
        twsword = true;
    }

    void WeaponChange()
    {
        if(twsword)
        {
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            weapons[2].SetActive(true);
        }
        if(sword)
        {
            weapons[0].SetActive(true);
            weapons[1].SetActive(true);
            weapons[2].SetActive(false);
        }
    }

    void NEM1()
    {
        page1 += Time.deltaTime;
        if(page1 >= page1check)
        {
            tBOSS = TBOSS.PAGE1;
            tbanim.SetTrigger("PAGE1");
        }
    }
    void NEM2()
    {
        page2 += Time.deltaTime;
        if(page2 >= page2check)
        {
            tBOSS = TBOSS.PAGE2;
            tbanim.SetTrigger("PAGE2");
        }
    }
    
    void Spawn()
    {

        int selection = Random.Range(0, page1Casting.Length);

        GameObject selectedPrefab = page1Casting[selection];

        Vector3 spawnPos = GetRandomPosition();
        Quaternion spawnRot = Quaternion.Euler(0,transform.rotation.y + Random.Range(0, 360), 0);
        GameObject instance = Instantiate(selectedPrefab, spawnPos, spawnRot);
        gameObjects.Add(instance);

        tBOSS = TBOSS.IDLE;
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = nem1Area.transform.position;

        Vector3 size = nem1Area.size;

        float posX = basePosition.x + Random.Range(-12, 12);
        float posY = basePosition.y + 0.1f;
        float posZ = basePosition.z + Random.Range(-12, 12);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);

        return spawnPos;
    }
    IEnumerator Page1Start()
    {
        yield return new WaitForSeconds(5f);
        CancelInvoke("Spawn");
        float distans = Vector3.Distance(targetPlayer.position, transform.position);
        if (distans > attakRange)
        {
            tBOSS = TBOSS.MOVE;
        }
        else
        {
            tBOSS = TBOSS.ATTACK;
        }
    }

    IEnumerator P1EffectOnOff()
    {
        patternE[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        patternE[0].SetActive(false);
    }

    IEnumerator BeamEffectLoding()
    {
        patternE[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        patternE[1].SetActive(false);
    }
    IEnumerator BeamInstans()
    {
        patternSkillActive[0].SetActive(true);
        yield return new WaitForSeconds(2.3f);
        patternSkillActive[0].SetActive(false);
        isActivating = false;
        float distans = Vector3.Distance(targetPlayer.position, transform.position);
        if (distans > attakRange)
        {
            tBOSS = TBOSS.MOVE;
        }
        else
        {
            tBOSS = TBOSS.ATTACK;
        }
    }
    IEnumerator DownPatternEttect()
    {
        downPattern.SetActive(true);
        yield return new WaitForSeconds(2.3f);
        downPattern.SetActive(false);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        //클론이 통신을 받는 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
