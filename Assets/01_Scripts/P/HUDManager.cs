using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Cinemachine;

public class HUDManager : MonoBehaviourPunCallbacks
{
    private Vector3 currPos;
    private Quaternion currRot;
    public PhotonView pv;
    public StateManager stateManager;
    public Slider HpSlider;
    public Text HpText;
   
    public Image DHpBar;

    //여기 함수를 피격판정에서 불러온다!
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        stateManager = gameObject.GetComponent<StateManager>();

        HpSlider = GameObject.Find("WorldCanvas/PlayerHUD").GetComponent<Slider>();
        HpText = GameObject.Find("WorldCanvas/PlayerHUD/MainHP").GetComponent<Text>();
        DHpBar = GameObject.Find("WorldCanvas/PlayerHUD/DecreasePBar").GetComponent<Image>();

        InitHP();
    }


    public void InitHP()
    {
        DHpBar.fillAmount = 1;
        HpSlider.value = (stateManager.hp / stateManager.maxhp);
        HpText.text = ((int)stateManager.hp + "/" + (int)stateManager.maxhp).ToString();
    }

    public void InitHPBtn()
    {
        stateManager.hp -= 20f;
        HpSlider.value = (stateManager.hp / stateManager.maxhp);
        HpText.text = ((int)stateManager.hp + "/" + (int)stateManager.maxhp).ToString();
    }
    public void ChangeUserHUD()
    {
        HpSlider.value = (stateManager.hp / stateManager.maxhp);
        HpText.text = ((int)stateManager.hp + "/" + (int)stateManager.maxhp).ToString();

        if (stateManager.hp <= 0)
        {
            HpText.text = ("0" + "/" + (int)stateManager.maxhp).ToString();
        }
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            ChangeUserHUD();
            float targetFillAmount = Mathf.InverseLerp(0, stateManager.maxhp, stateManager.hp);

            if (DHpBar.fillAmount > targetFillAmount)
            {
                DHpBar.fillAmount -= 0.1f * Time.deltaTime;
                DHpBar.fillAmount = Mathf.Max(DHpBar.fillAmount, targetFillAmount);
            }
            if (DHpBar.fillAmount < targetFillAmount)
            {
                DHpBar.fillAmount += 0.1f * Time.deltaTime;
                DHpBar.fillAmount = Mathf.Max(DHpBar.fillAmount, targetFillAmount);
            }
        }
    }
    public void ChangeMainHP()
    {

    }

}
