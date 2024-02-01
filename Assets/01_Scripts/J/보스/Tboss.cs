using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tboss : MonoBehaviour
{
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
    [Header("AttackPattern")]
    public float p1;
    public float p2;
    public float p3;
    public float p4;
    public float p1check;
    public float p2check;
    public float p3check;
    public float p4check;
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
    void Start()
    {
        tBOSS = TBOSS.IDLE;
        characterController = GetComponent<CharacterController>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        tbanim = GetComponent<Animator>();
        stateManager = GetComponent<StateManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    void Update()
    {
        if (!die)
        {
            breakTiming();
            BreakTime();
            Dieing();
            PatternTimeCheck();
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
                    break;
                case TBOSS.PAGE1:
                    break;


            }
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


}
