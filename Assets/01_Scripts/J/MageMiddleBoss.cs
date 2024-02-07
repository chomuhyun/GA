using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMiddleBoss : MonoBehaviour
{

    public ChaosDungeonMgr cDunMgr;
    public enum MAGEBOSS
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
    public MAGEBOSS mageBoss;
    public Animator mageAnim;
    public StateManager stateManager;
    public ChaosPlayerCtlr player;
    
    [Header("AttackPattern")]

    public bool down;
    public bool attacking;
    public bool die;

    [Header("PatternEffect")]
    public Transform shotPoint;
    public Transform JumpShotPoint;
    public GameObject[] patternE;
    public GameObject[] patternSkillActive;
    void Start()
    {
        cDunMgr = GameObject.Find("ChaosDungeonMgr").GetComponent<ChaosDungeonMgr>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ChaosPlayerCtlr>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mageBoss = MAGEBOSS.IDLE;
        characterController = GetComponent<CharacterController>();
        mageAnim = GetComponent<Animator>();
        stateManager = GetComponent<StateManager>();
    }
    void Update()
    {
            if (!die)
            {
                Dieing();
                switch (mageBoss)
                {
                    case MAGEBOSS.IDLE:
                        isActivating = false;
                    mageAnim.SetTrigger("IDLE");
                        float dist = Vector3.Distance(targetPlayer.position, transform.position);
                        if (dist < range)
                        {
                        mageBoss = MAGEBOSS.MOVE;
                        }
                        else
                        {
                        mageBoss = MAGEBOSS.IDLE;
                        }
                        break;
                    case MAGEBOSS.MOVE:
                        StartCoroutine(MoveDelay());
                        if (attacking)
                            return;
                        float dis = Vector3.Distance(targetPlayer.position, transform.position);
                        if (dis > 1f)
                        {
                            isActivating = false;
                        }
                        if (dis < attakRange)
                        {
                        mageBoss = MAGEBOSS.ATTACK;
                        }
                        speed = 3f;
                        MoveTowardsTarget(true);
                    mageAnim.SetTrigger("RUN");
                        float distan = Vector3.Distance(targetPlayer.position, transform.position);
                        if (distan > 15)
                        {
                        mageBoss = MAGEBOSS.IDLE;
                        }
                        break;
                    case MAGEBOSS.ATTACK:
                    mageAnim.SetTrigger("ATTACK");
                        isActivating = true;
                        speed = 0f;
                        float dists = Vector3.Distance(targetPlayer.position, transform.position);
                        if (dists > attakRange)
                        {
                        mageBoss = MAGEBOSS.MOVE;
                        }
                        else
                        {
                        mageBoss = MAGEBOSS.ATTACK;
                        }
                        break;
                    case MAGEBOSS.DIE:
                        isActivating = true;
                        speed = 0;
                        mageAnim.SetTrigger("DIE");
                        cDunMgr.bossKilled++;
                        cDunMgr.ClearMidBoss();
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
                mageBoss = MAGEBOSS.ATTACK;
            }
        }
        IEnumerator MoveDelay()
        {
            yield return new WaitForSeconds(4.5f);
            down = false;
            yield return new WaitForSeconds(3f);
            attacking = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("TimeSlow"))
        {
            mageAnim.speed = 0.15f;
            speed = 0.3f;
        }
        else
        {
            speed = 3f;
            mageAnim.speed = 1f;
        }
    }
    void Dieing()
    {
        if (stateManager.hp <= 0)
        {
            die = true;
            mageBoss = MAGEBOSS.DIE;
            characterController.enabled = false;
        }
    }

    void P1effect()
    {
        GameObject obj;
        obj = Instantiate(patternE[0], shotPoint.position, shotPoint.rotation);
        Destroy(obj, 3f);
    }
    void P2effect()
    {
        GameObject obj;
        obj = Instantiate(patternE[1], transform.position, transform.rotation);
        Destroy(obj, 0.8f);
    }
    void P2bigerEffect()
    {
        GameObject obj;
        obj = Instantiate(patternE[2], transform.position, transform.rotation);
        Destroy(obj, 1f);
    }

    void P3Effect()
    {
        GameObject obj;
        obj = Instantiate(patternE[3], shotPoint.position, shotPoint.rotation);
        Destroy(obj, 3f);
    }
    void P4Effect()
    {
        GameObject obj;
        obj = Instantiate(patternE[4], transform.position, transform.rotation);
        Destroy(obj, 1.3f);
    }
    void P5Effect()
    {
        GameObject obj;
        Vector3 spwanPos = new Vector3(0, 1f, 0);
        obj = Instantiate(patternE[5], transform.position + spwanPos, transform.rotation);
        Destroy(obj, 4f);
    }
    void P6Effect()
    {
        GameObject obj;
        Quaternion rot = Quaternion.Euler(135f, 0, 0);
        obj = Instantiate(patternE[6], JumpShotPoint.position, JumpShotPoint.rotation);
        Destroy(obj, 3f);
    }
    void CastingEffectOn()
    {
        patternE[7].SetActive(true);
    }
    void CastingEffectOff()
    {
        patternE[7].SetActive(false);
    }

}

