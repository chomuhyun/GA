using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RaidBossCtrl : MonoBehaviour
{
    public enum RAIDBOSS
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
    public RAIDBOSS raidBoss;
    public bool isActivating;

    [Header("Component")]
    public CharacterController characterController;
    public Animator anim;
    public StateManager stateManager;
    public VisualEffect healthUp;
    public Player player;

    [Header("AttackPattern")]
    public float p1;
    public float p2;
    public float p3;
    public float p4;
    public float p5;
    public float dieNowPattern;
    public float healthUpingTime;
    public float healUseingTime;
    public float p1check;
    public float p2check;
    public float p3check;
    public float p4check;
    public float p5check;
    public float dieNowPatternCheck;
    public float healthUpingTimeCheck;
    public float healUseingTimeCheck;

    public bool p1Ready;
    public bool p2Ready;
    public bool p3Ready;
    public bool p4Ready;
    public bool p5Ready;
    public bool down;
    public bool attacking;
    public GameObject dieNowPatternEffect;
    public GameObject DownPattern;
    public float breakTime;
    public float breakCheck;
    public bool breakOn;
    public bool die;
    public bool healthUpCheck;

    [Header("Ettect")]
    public GameObject p5PatternE;
    public GameObject downPatterE;
    public GameObject p2EttectE;
    public GameObject p1EttectE;
    public GameObject healEttetE;
    void Start()
    {
        raidBoss = RAIDBOSS.IDLE;
        characterController = GetComponent<CharacterController>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        stateManager = GetComponent<StateManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (!die)
        {
            BreakTime();
            PatternTimeCheck();
            Dieing();
            DieNowPatternt();
            healthUpPattern();
            switch (raidBoss)
            {
                case RAIDBOSS.IDLE:
                    isActivating = false;
                    anim.SetTrigger("IDLE");
                    float dist = Vector3.Distance(targetPlayer.position, transform.position);
                    if (dist < range)
                    {
                        raidBoss = RAIDBOSS.MOVE;
                    }
                    else
                    {
                        raidBoss = RAIDBOSS.IDLE;
                    }
                    break;
                case RAIDBOSS.MOVE:
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
                    if(dis < attakRange)
                    {
                        raidBoss = RAIDBOSS.ATTACK;
                    }
                    speed = 3f;
                    anim.SetTrigger("RUN");
                    MoveTowardsTarget(true);
                    float distan = Vector3.Distance(targetPlayer.position, transform.position);
                    if (distan > 18)
                    {
                        raidBoss = RAIDBOSS.IDLE;
                    }
                    break;
                case RAIDBOSS.ATTACK:
                    isActivating = true;
                    speed = 0f;
                    float dists = Vector3.Distance(targetPlayer.position, transform.position);
                    if (p1Ready)
                    {
                        if (dists < attakRange)
                        {
                            anim.SetTrigger("Pattern1");
                            p1 = 0;
                            p1Ready = false;
                            attacking = true;
                        }
                    }
                    if (p2Ready)
                    {
                        if (dists < attakRange)
                        {
                            anim.SetTrigger("Pattern2");
                            p2 = 0;
                            p2Ready = false;
                            attacking = true;
                        }
                    }
                    if (p3Ready)
                    {
                        if (dists < attakRange)
                        {
                            anim.SetTrigger("Pattern3");
                            p3 = 0;
                            p3Ready = false;
                            attacking = true;
                        }
                    }
                    if (p4Ready)
                    {
                        if (dists < attakRange)
                        {
                            anim.SetTrigger("Pattern4");
                            p4 = 0;
                            p4Ready = false;
                            attacking = true;
                        }
                    }
                    if (p5Ready)
                    {
                        if (dists < attakRange)
                        {
                            anim.SetTrigger("Pattern5");
                            p5 = 0;
                            p5Ready = false;
                            attacking = true;
                        }
                    }
                    if (dists > attakRange)
                    {
                        raidBoss = RAIDBOSS.MOVE;
                    }
                    else
                    {
                        raidBoss = RAIDBOSS.ATTACK;
                    }
                    break;
                case RAIDBOSS.BREAK:
                    isActivating = true;
                    speed = 0f;
                    breakTime = 0f;
                    StartCoroutine(breakTiming());
                    break;
                case RAIDBOSS.DOWN:
                    isActivating = true;
                    down = true;
                    speed = 0f;
                    float dista = Vector3.Distance(targetPlayer.position, transform.position);
                    anim.SetTrigger("Down");
                    if (dista > attakRange)
                    {
                        raidBoss = RAIDBOSS.MOVE;
                    } 
                    else
                    {
                        raidBoss = RAIDBOSS.ATTACK;
                    }
                    break;
                case RAIDBOSS.DIE:
                    isActivating = true;
                    speed = 0;
                    anim.SetTrigger("Die");
                    break;
                case RAIDBOSS.PAGE1:
                    break;
            }
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
            raidBoss = RAIDBOSS.ATTACK;
        }
    }
    IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(4.5f);
        down = false;
        yield return new WaitForSeconds(3f);
        attacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(breakOn)
        {
            if(other.CompareTag("DownSkill"))
            {
                raidBoss = RAIDBOSS.DOWN;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("TimeSlow"))
        {
            anim.speed = 0.15f;
            speed = 0.3f;
        }
        else
        {
            speed = 3f;
            anim.speed = 1f;
        }
    }

    void PatternTimeCheck()
    {
        p1 += Time.deltaTime;
        if(p1 >= p1check)
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
        p5 += Time.deltaTime;
        if (p5 >= p5check)
        {
            p5 = p5check;
            p5Ready = true;
        }
    }
    void BreakTime()
    {
        breakTime += Time.deltaTime;
        if(breakTime >= breakCheck)
        {
            anim.SetTrigger("Break");
            raidBoss = RAIDBOSS.BREAK;
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
            raidBoss = RAIDBOSS.MOVE;
        }
        else
        {
            raidBoss = RAIDBOSS.ATTACK;
        }
        breakOn = false;
        DownPattern.SetActive(true);
        yield return new WaitForSeconds(2f);
        DownPattern.SetActive(false);
    }

    void Dieing()
    {
        if(stateManager.hp <= 0)
        {
            die = true;
            raidBoss = RAIDBOSS.DIE;
        }
    }

    void DieNowPatternt()
    {
        dieNowPattern += Time.deltaTime;
        if(dieNowPattern >= dieNowPatternCheck)
        {
            Vector3 Pos = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z - 1f);
            GameObject obj;
            obj = Instantiate(dieNowPatternEffect, Pos, transform.rotation);
            Destroy(obj, 3.8f);
            dieNowPattern = 0;
            anim.SetTrigger("Rolling");
        }
    }

    void healthUpPattern()
    {
        healthUpingTime += Time.deltaTime;
        if (healthUpingTime >= healthUpingTimeCheck)
        {
            anim.SetTrigger("HealthUp");
            healthUpCheck = true;
            if (healthUpCheck)
            {
                InvokeRepeating("EffectInvoke", 0, 2);
                stateManager.hp += 3000f;
                stateManager.atk += 50;
                healthUpingTime = 0;
                StartCoroutine(EffectDelay());
            }
        }
    }

    void EffectInvoke()
    {
        healthUp.Play();
    }
    IEnumerator EffectDelay()
    {
        yield return new WaitForSeconds(12f);
        stateManager.atk -= 50;
        CancelInvoke();
        healthUpCheck = false;
    }

    IEnumerator P5patternEttect()
    {
        p5PatternE.SetActive(true);
        yield return new WaitForSeconds(2f);
        p5PatternE.SetActive(false) ;
    }
    IEnumerator DownPatternEttect()
    {
        downPatterE.SetActive(true);
        yield return new WaitForSeconds(2.3f);
        downPatterE.SetActive(false);
    }
    IEnumerator P2EttectEttect()
    {
        p2EttectE.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        p2EttectE.SetActive(false);
    }
    IEnumerator P1Ettect()
    {
        p1EttectE.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        p1EttectE.SetActive(false);
    }
    IEnumerator HealEttect()
    {
        healEttetE.SetActive(true);
        yield return new WaitForSeconds(5f);
        healEttetE.SetActive(false);
    }
}
