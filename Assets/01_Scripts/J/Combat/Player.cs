using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Cinemachine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    float gravity = -9.8f;
    public Text nickNameTxt;
    public ChatManager chatManager;
    public bool allowMove = false;
    public QuestPopUpManager questPopUpManager;
    // 여기 위에를 추가했음. 현창

    public Canvas canvas;
    private Vector3 currPos;
    private Quaternion currRot;
    private Transform tr;
   
    [Header("Shop")]
    private GameObject nearObject;
    public int coin;

    [Header("Move")]
    public float speed;
    public float moveSpeed = 8f;
    public float turn;
    public bool Desh;
    float DeshCool;
    float CurDeshCool = 8f;
    public bool isDeshInvincible;

    float chargingTime;
    float curchargingTime = 3f;
    float hAxis;
    float vAxis;
    Vector3 moveVec;
    private Plane plane;
    private Ray ray;
    private Vector3 hitPosition;

    [Header("Component")]
    CharacterController characterController;
    Rigidbody rigid;
    public Animator animator;
    TrailRenderer trailRenderer;
    Weapons weapons;
    private PlayableDirector PD;
    public TimelineAsset[] Ta;
    Boss boss;
    StateManager stateManager;
    MeshRenderTail meshRenderTail;
    HUDManager hudManager;
    private new Camera camera;
    public PhotonView pv;
    PhotonAnimatorView pav;
    public CinemachineVirtualCamera cvc;
    UIMgr uimgr;
    MageHealSkill magehealSkill;
    [Header("CamBat")]
    public bool isAttack;
    public bool isAttack1;
    public bool isAttack2;
    public bool isAttack3;
    float fireDelay;
    public bool isFireReady;
    public bool isDeath;
    public bool downing;

    [Header("SkillEffect")]
    public GameObject[] Skill;

    [Header("Shot or Active Point")]
    public Transform[] Point;

    [Header("Guns or Object")]
    public GameObject[] ob;

    [Header("Skill CoolTime")]
    public Image[] skillICoolicon;
    public GameObject CoolTimeZip;
    public GameObject playerSkillIcon;
    public bool skillUse;
    public bool qisReady;
    public bool eisReady;
    public bool risReady;
    public bool rischarging;
    public bool onMagic;
    public float qskillcool;
    public float eskillcool;
    public float rskillcool;
    public float curQskillcool;
    public float curEskillcool;
    public float curRskillcool;
    public Slider chargingSlider;
    public float originalTimeScale;
    public bool sPlayer;
    public bool aPlayer;
    public bool mPlayer;

    [Header("NPC")]
    public GameObject[] getNPC;

    [Header("UIctrl")]
    public bool isInven;
    [SerializeField] private float rotCamXAxisSpeed = 500f;
    [SerializeField] private float rotCamYAxisSpeed = 3f;
    internal string NickName;
    RaidBossCtrl raidBoss;
    Tboss tboss;
    public bool npcAttackStop;
    ThirdPersonOrbitCamBasicA camBasicA;
    HidingObjCamera hidingObjCamera;
    RaidGroundOner groundOner;


    void Awake()
    {
        questPopUpManager = GameObject.Find("WorldCanvas/UIMgr/QuestPopUp").GetComponentInChildren<QuestPopUpManager>();
        // 위에 추가함 현창
        camera = Camera.main;
        isFireReady = true;
        weapons = GetComponentInChildren<Weapons>();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        stateManager = GetComponent<StateManager>();
        hudManager = GetComponent<HUDManager>();
        uimgr = GameObject.Find("UIMgr").GetComponent<UIMgr>();
        //cvc = GameObject.FindGameObjectWithTag("CVC").GetComponent<CinemachineVirtualCamera>();
        if (PhotonNetwork.IsConnected && photonView.IsMine)
        {
            cvc = GameObject.FindGameObjectWithTag("CVC").GetComponent<CinemachineVirtualCamera>();
            cvc.GetComponent<ThirdPersonOrbitCamBasicA>().player = transform;
            hidingObjCamera = GameObject.FindGameObjectWithTag("CVC").GetComponent<HidingObjCamera>();
            hidingObjCamera.GetComponent<HidingObjCamera>().targetPlayer = this.gameObject;
            //groundOner = GameObject.Find("Ground2F").GetComponent<RaidGroundOner>();
        } 
        if (boss != null)
        {
            boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        }
        skillICoolicon[0] = GameObject.Find("CoolTimeBGQ").GetComponent<Image>();
        skillICoolicon[1] = GameObject.Find("CoolTimeBGE").GetComponent<Image>();
        skillICoolicon[2] = GameObject.Find("CoolTimeBGR").GetComponent<Image>();
    }
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        Canvas obj = transform.Find("WCan").GetComponent<Canvas>();
        nickNameTxt = obj.GetComponentInChildren<Text>();

        if (pv.IsMine)
        {
            questPopUpManager.UpdateQuestStatus(); // 여기 추가했음 현창
            nickNameTxt.text = PhotonNetwork.NickName + " (나)"; //여기 추가했음. 현창
            nickNameTxt.color = Color.white;

            cvc.Follow = transform;
            cvc.LookAt = transform;
            chatManager = GetComponent<ChatManager>();

            // 여기 위에를 추가했음. 현창

            pv = GetComponent<PhotonView>();
            pav = GetComponent<PhotonAnimatorView>();
            plane = new Plane(transform.up, transform.position);
        }
        else
        {
            nickNameTxt.text = pv.Owner.NickName;
            nickNameTxt.color = Color.red;
        }
        skillICoolicon[0].fillAmount = 0;
        skillICoolicon[1].fillAmount = 0;
        skillICoolicon[2].fillAmount = 0;
        //chatManager.StartCoroutine(chatManager.CheckEnterKey());
    }

    private void LateUpdate()
    {
        if(pv.IsMine)
        {
            hidingObjCamera.RefreshHiddenObjects();
        }
    }


    //"��������"
    void moves()
    {
        if (skillUse == true)
            return;
        if (isFireReady == false)
            return;
        if (downing == true)
            return;
        if (isDeath == true)
            return;
        if (downing)
            return;

        Vector2 moveinput = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * 1.5f, Input.GetAxis("Vertical") * Time.deltaTime * 1.5f);
        bool ismove = moveinput.magnitude != 0;
        animator.SetBool("isRun", ismove);



        if (ismove)
        {
            Vector3 lookForward = new Vector3(cvc.transform.forward.x, 0f, cvc.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cvc.transform.right.x, 0f, cvc.transform.right.z).normalized;
            Vector3 moveDir = lookForward * moveinput.y + lookRight * moveinput.x;

            transform.forward = moveDir;
            transform.position += moveDir * Time.deltaTime * 0.01f;
            characterController.Move(moveDir * 5f);
        }
        if (!characterController.isGrounded)
        {
            Vector3 gravityVector = Vector3.down * -gravity * Time.deltaTime;
            characterController.Move(gravityVector);
        }
    }

    void Interation()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && nearObject != null && nearObject.tag == "Shop")
        {
            Shop shop = nearObject.GetComponent<Shop>();
            if (shop != null)
            {
                shop.Enter(this);
            }
        }
    }

    void FixedUpdate() // 원래 FixedUpdate였음
    {
        if (pv.IsMine)
        {
            if (PhotonNetwork.IsConnected && photonView.IsMine)
            {
                cvc = GameObject.FindGameObjectWithTag("CVC").GetComponent<CinemachineVirtualCamera>();
                cvc.GetComponent<ThirdPersonOrbitCamBasicA>().player = transform;
                hidingObjCamera = GameObject.FindGameObjectWithTag("CVC").GetComponent<HidingObjCamera>();
                hidingObjCamera.GetComponent<HidingObjCamera>().targetPlayer = this.gameObject;
            }
            nickNameTxt.text = PhotonNetwork.NickName + " (나)"; //여기 추가했음. 현창
            nickNameTxt.color = Color.white;
            Vector3 offset = new Vector3(0f, 2f, 0f);
            nickNameTxt.transform.position = transform.position + offset;

            originalTimeScale = Time.timeScale * Time.unscaledDeltaTime;

            if (!isDeath)
            {
                //Debug.Log("현재의 allowMove는 " + allowMove);
                if (allowMove)
                {
                    GetinPut();
                    moves();
                    Attack();
                    SkillOn();
                    Death();
                    Deshs();
                    Interation();
                    SkillCoolTime();
                    KnowingBoss();
                    KnowAnim();
                }
            }
        }
        else
        {
            nickNameTxt.text = pv.Owner.NickName;
            nickNameTxt.color = Color.red;
            Vector3 offset = new Vector3(0f, 2f, 0f);
            nickNameTxt.transform.position = transform.position + offset;
        }
    }

    void GetinPut()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        turn = Input.GetAxisRaw("Mouse X");
        isAttack = Input.GetButtonDown("Fire");
    }

    void Deshs()
    {
        DeshCool += Time.deltaTime;
        if (DeshCool >= CurDeshCool)
        {
            Desh = true;
            DeshCool = CurDeshCool;
        }
        if (Desh)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("isDesh");
                DeshCool = 0;
                isDeshInvincible = true;
            }
        }
    }

    void SkillCoolTime()
    {
        if (pv.IsMine)
        {
            if (!qisReady)
            {
                skillICoolicon[0].fillAmount = 1 - qskillcool / curQskillcool;
            }
            if (!eisReady)
            {
                skillICoolicon[1].fillAmount = 1 - eskillcool / curEskillcool;
            }
            if (!risReady)
            {
                skillICoolicon[2].fillAmount = 1 - rskillcool / curRskillcool;
            }
            if (sPlayer)
            {
                uimgr.playerSkillIcon[0].SetActive(true);
                uimgr.playerSkillIcon[1].SetActive(false);
                uimgr.playerSkillIcon[2].SetActive(false);
            }
            if (aPlayer)
            {
                uimgr.playerSkillIcon[0].SetActive(false);
                uimgr.playerSkillIcon[1].SetActive(true);
                uimgr.playerSkillIcon[2].SetActive(false);
            }
            if (mPlayer)
            {
                uimgr.playerSkillIcon[0].SetActive(false);
                uimgr.playerSkillIcon[1].SetActive(false);
                uimgr.playerSkillIcon[2].SetActive(true);
            }
        }
    }

    void Attack()
    {
        if (npcAttackStop)
            return;
        //chargingTime += Time.deltaTime;
        fireDelay += Time.deltaTime;
        isFireReady = weapons.rate < fireDelay;

        if (isAttack && isFireReady)
        {
            weapons.WeaponUse();
            animator.SetTrigger("Attack");
            fireDelay = 0;
        }
        if (isAttack1)
        {
            isAttack3 = false;
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetTrigger("Smash1");
                isAttack1 = false;
            }
        }
        if (isAttack2)
        {
            isAttack1 = false;
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetTrigger("Smash2");
                isAttack2 = false;
            }
        }
        if (isAttack3)
        {
            isAttack2 = false;
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetTrigger("Smash3");
                isAttack3 = false;
                //if(Input.GetMouseButton(1))
                //{
                //if(chargingTime >= curchargingTime)
                // {
                // animator.SetTrigger("Charging");
                // isAttack3 = false;
                // }
                // } 차징 스킬 구현중
            }

        }
    }

    public void Death()
    {
        if (stateManager.hp <= 0)
        {
            isDeath = true;
            characterController.enabled = false;
            StartCoroutine(DeathDelay());
            transform.GetComponent<StateManager>().dataMgrDontDestroy.playerDie = true;
        }
    }

    IEnumerator DeathDelay()
    {
        animator.SetTrigger("isDeath");
        yield return null;
    }

    void SkillOn()
    {
        if (pv.IsMine)
        {
            qskillcool += Time.deltaTime;

            if (qskillcool >= curQskillcool)
            {
                qskillcool = curQskillcool;
                qisReady = true;
            }

            eskillcool += Time.deltaTime;

            if (eskillcool >= curEskillcool)
            {
                eskillcool = curEskillcool;
                eisReady = true;
            }
            rskillcool += Time.deltaTime;

            if (rskillcool >= curRskillcool)
            {
                rskillcool = curRskillcool;
                risReady = true;
                rischarging = true;
            }

            if (qisReady)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    animator.SetTrigger("SkillQ");
                    qskillcool = 0;
                    qisReady = false;
                }
            }

            if (eisReady)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    animator.SetTrigger("SkillE");
                    eskillcool = 0;
                    eisReady = false;
                }
            }

            if (risReady)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    animator.SetTrigger("SkillR");
                    rskillcool = 0;
                    risReady = false;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DownPattern"))
        {
            if (isDeshInvincible == true)
                return;
            animator.SetTrigger("Down");
            StartCoroutine(DownDelay());
        }
    }     

    IEnumerator DownDelay()
    {
        downing = true;
        yield return new WaitForSeconds(4f);
        downing = false;
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Shop")
            nearObject = other.gameObject;
        if (other.tag == "HealArea")
        {
            if (stateManager.hp >= stateManager.maxhp)
                return;

            stateManager.hp += 1;
            hudManager.ChangeUserHUD();
        }
        if (other.CompareTag("NPC"))
        {
            npcAttackStop = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (other.CompareTag("SaveZone"))
        {
            isDeshInvincible = true;
        }
    }

    [PunRPC]
    void KnowAnim()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (pv != null && pv.IsMine)
            {
                if(raidBoss != null)
                {
                    raidBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<RaidBossCtrl>();
                    raidBoss.GroundOner();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shop")
        {

            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            nearObject = null;
        }
        if (other.CompareTag("NPC"))
        {
            npcAttackStop = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (other.CompareTag("SaveZone"))
        {
            isDeshInvincible = false;
        }

    }


    public void KnowingBoss()
    {
        if (raidBoss != null)
        {
            if (pv.IsMine)
            {
                testGameMgr someComponent = GameObject.FindWithTag("Player").GetComponent<testGameMgr>();
                someComponent.Starts();
                raidBoss.characterController = raidBoss.GetComponent<CharacterController>();
                raidBoss.anim = raidBoss.GetComponent<Animator>();
                stateManager = raidBoss.GetComponent<StateManager>();
                raidBoss.player = this;
                raidBoss.targetPlayer = transform;
            }
        }
    }
    #region // 스킬 함수들
    void SkillUsing()
    {
        skillUse = true;
    }
    void SkillClose()
    {
        skillUse = false;
    }


    void SwordSkill_Q()
    {
        GameObject obj;

        obj = Instantiate(Skill[0], Point[0].position, Point[0].rotation);
        obj.GetComponent<WeaponsAttribute>().sm = transform.GetComponent<StateManager>();
        Destroy(obj, 2f);
    }
    void SwordSkill_E()
    {
        GameObject obj;

        obj = Instantiate(Skill[1], Point[0].position, Point[0].rotation);
        obj.GetComponent<WeaponsAttribute>().sm = transform.GetComponent<StateManager>();
        Destroy(obj, 2f);
    }

    void SwordSkill_R()
    {
        GameObject obj;

        obj = Instantiate(Skill[2], Point[0].position, Point[0].rotation);
        obj.GetComponent<WeaponsAttribute>().sm = transform.GetComponent<StateManager>();
        Destroy(obj, 2f);
    }

    void A_LfireAttack()
    {
        Vector3 spawnRotation = new Vector3(0, 90, 0);
        Instantiate(Skill[4], Point[5].transform.position, Point[5].transform.rotation);
    }
    void A_RfireAttack()
    {
        Vector3 spawnRotation = new Vector3(0, 90, 0);
        Instantiate(Skill[4], Point[5].transform.position, Point[5].transform.rotation);
    }

    void A_SkillQ()
    {
        GameObject obj;

        obj = Instantiate(Skill[0], Point[0].position, Point[0].rotation);
        obj.GetComponent<WeaponsAttribute>().sm = transform.GetComponent<StateManager>();
        Destroy(obj, 1.7f);
    }
    void A_SkillE()
    {
        ob[3].SetActive(true);
        StartCoroutine(eskillDelay());
    }
    void A_SkillEclose()
    {
        ob[3].SetActive(false);
    }
    IEnumerator eskillDelay()
    {
        yield return new WaitForSeconds(1.8f);
        GameObject obj;
        obj = Instantiate(Skill[1], Point[2].position, Point[2].rotation);
        obj.GetComponent<WeaponsAttribute>().sm = transform.GetComponent<StateManager>();
        Destroy(obj, 2f);
    }
    void A_SkillR()
    {
        GameObject obj;
        GameObject objs;

        obj = Instantiate(Skill[2], Point[3].position, Point[3].rotation);
        objs = Instantiate(Skill[3], Point[4].position, Point[4].rotation);
        obj.GetComponent<WeaponsAttribute>().sm = transform.GetComponent<StateManager>();
        Destroy(obj, 1.3f);
        Destroy(objs, 1.3f);
    }

    IEnumerator M_SkillQ()
    {
        Skill[0].SetActive(true);
        yield return new WaitForSeconds(4f);
        Skill[0].SetActive(false);
    }

    void M_SkillE()
    {
        GameObject obj;

        obj = Instantiate(Skill[1], transform.position, transform.rotation);
        Destroy(obj, 15f);
    }

    void M_SkillR()
    {
        GameObject obj;
        obj = Instantiate(Skill[2], transform.position, transform.rotation);
        Destroy(obj, 6f);
    }

    void ActiveRifle()
    {
        ob[2].SetActive(true);
    }
    void HidingRifle()
    {
        ob[2].SetActive(false);
    }

    void HandGunActive()
    {
        ob[1].SetActive(true);
        ob[0].SetActive(true);
    }
    void HidingHandGun()
    {
        ob[1].SetActive(false);
        ob[0].SetActive(false);
    }

    IEnumerator LeffectDelay()
    {

        ob[4].SetActive(true);
        Instantiate(Skill[4], Point[5].transform.position, Point[5].transform.rotation);
        yield return new WaitForSeconds(0.3f);
        ob[4].SetActive(false);
    }
    IEnumerator ReffectDelay()
    {

        ob[5].SetActive(true);
        Instantiate(Skill[4], Point[5].transform.position, Point[5].transform.rotation);
        yield return new WaitForSeconds(0.3f);
        ob[5].SetActive(false);
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
#endregion // 스킬 함수들