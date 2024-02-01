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
    public Text nickNameTxt;
    public ChatManager chatManager;
    public bool allowMove = false;
    // 여기 위에를 추가했음. 현창

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
    Animator animator;
    TrailRenderer trailRenderer;
    Weapons weapons;
    private PlayableDirector PD;
    public TimelineAsset[] Ta;
    Boss boss;
    StateManager stateManager;
    MeshRenderTail meshRenderTail;
    HUDManager hudManager;
    private new Camera camera;
    PhotonView pv;
    PhotonAnimatorView pav;
    CinemachineVirtualCamera cvc;
     UIMgr uimgr;
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
    public Image[] skillIcon;
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

    
    void Awake()
    {
        camera = Camera.main;
        isFireReady = true;
        weapons = GetComponentInChildren<Weapons>();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        stateManager = GetComponent<StateManager>();
        hudManager = GetComponent<HUDManager>();
        uimgr = GameObject.Find("UIMgr").GetComponent<UIMgr>();
        chargingSlider = GameObject.FindGameObjectWithTag("Heal").GetComponent<Slider>();
        cvc = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        if (PhotonNetwork.IsConnected && photonView.IsMine)
        {
            cvc.GetComponent<ThirdPersonOrbitCamBasicA>().player = transform;
        }
        cvc.GetComponent<ThirdPersonOrbitCamBasicA>().Starts();
        if (skillIcon != null)
        {
            skillIcon[0] = GameObject.Find("CoolTimeBGQ").GetComponent<Image>();
            skillIcon[1] = GameObject.Find("CoolTimeBGE").GetComponent<Image>();
            skillIcon[2] = GameObject.Find("CoolTimeBGR").GetComponent<Image>();
        }
        if (boss != null)
        {
            boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        }
    }

    private void Start()
    {
        chatManager = GetComponent<ChatManager>();
        Canvas nickCanvas = GetComponentInChildren<Canvas>();
        nickNameTxt = nickCanvas.GetComponentInChildren<Text>();
        // 여기 위에를 추가했음. 현창

        pv = GetComponent<PhotonView>();
        pav = GetComponent<PhotonAnimatorView>();
        plane = new Plane(transform.up, transform.position);
        skillIcon[0].fillAmount = 0;
        skillIcon[1].fillAmount = 0;
        skillIcon[2].fillAmount = 0;
        
        if (pv.IsMine)
        {
            cvc.Follow = transform;
            cvc.LookAt = transform;
            SKilliconOn();
        }
        //chatManager.StartCoroutine(chatManager.CheckEnterKey());
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
    // Update is called once per frame
    void FixedUpdate() // 원래 FixedUpdate였음
    {
       
        if (pv.IsMine)
        {
            nickNameTxt.text = PhotonNetwork.NickName + " (나)"; //여기 추가했음. 현창
            nickNameTxt.color = Color.white;

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
                    UIctrl();
                }
            }
        }
        else
        {
            nickNameTxt.text = pv.Owner.NickName;
            nickNameTxt.color = Color.red;
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
        if (!qisReady)
        {
            skillIcon[0].fillAmount = 1 - qskillcool /curQskillcool;
        }
        if (!eisReady)
        {
            skillIcon[1].fillAmount = 1 - eskillcool / curEskillcool;
        }
        if (!risReady)
        {
            skillIcon[2].fillAmount = 1 - rskillcool / curRskillcool;
        }
    }

    
    void Attack()
    {
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
        }
    }

    IEnumerator DeathDelay()
    {
        animator.SetTrigger("isDeath");
        yield return null;
    }

    void SkillOn()
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
        if (onMagic)
        {
            ob[0] = GameObject.FindGameObjectWithTag("Heal").GetComponent<GameObject>();
            if (rischarging)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    animator.SetTrigger("SkillR");
                    Skill[2].SetActive(true);
                    ob[0].SetActive(true);
                    chargingSlider.value += Time.deltaTime * 0.35f;

                    if (chargingSlider.value == 1)
                    {
                        Skill[2].SetActive(false);
                        ob[0].SetActive(false);
                        rischarging = false;
                    }
                }
                else
                {
                Skill[2].SetActive(false);
                ob[0].SetActive(false);
                
                rischarging = false;
                chargingSlider.value = 0;
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
        if (other.CompareTag("SaveZone"))
            isDeshInvincible = true;
        if (other.CompareTag("NPCQ"))
            uimgr.npcPanel[0].SetActive(true);
        if (other.CompareTag("NPCW"))
            uimgr.npcPanel[1].SetActive(true);
        if (other.CompareTag("NPCL"))
            uimgr.npcPanel[2].SetActive(true);
        //if (other.CompareTag("NPCP"))
        //uimgr.npcPanel[5].SetActive(true);
        //if (other.CompareTag("NPCA"))
        //uimgr.npcPanel[1].SetActive(true);


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
            stateManager.hp += 5;
            hudManager.ChangeUserHUD();
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
        if (other.CompareTag("NPCQ"))
            uimgr.npcPanel[0].SetActive(false);
        if (other.CompareTag("NPCW"))
            uimgr.npcPanel[1].SetActive(false);
        if (other.CompareTag("NPCL"))
            uimgr.npcPanel[2].SetActive(false);
    }

    void UIctrl()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isInven)
            {
                uimgr.npcPanel[3].SetActive(true);
                isInven = true;
            }
            else
            {
                uimgr.npcPanel[3].SetActive(false);
                isInven = false;
            }
        }
    }
    void SKilliconOn()
    {
        if (sPlayer)
        {
            uimgr.playerSkillIcon[0].SetActive(true);
            uimgr.playerSkillIcon[1].SetActive(false);
            uimgr.playerSkillIcon[2].SetActive(false);
        }
        if(aPlayer)
        {
            uimgr.playerSkillIcon[0].SetActive(false);
            uimgr.playerSkillIcon[1].SetActive(true);
            uimgr.playerSkillIcon[2].SetActive(false);
        }
        if(mPlayer)
        {
            uimgr.playerSkillIcon[0].SetActive(false);
            uimgr.playerSkillIcon[1].SetActive(false);
            uimgr.playerSkillIcon[2].SetActive(true);
        }
            
    }    



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
        Vector3 spawnRotation = new Vector3 (0, 90, 0);
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
        Skill[2].SetActive(true);
        if (chargingSlider.value == 1)
        {
            Skill[2].SetActive(false);
            chargingSlider.value = 0;
        }
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
