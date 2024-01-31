using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconsManager : MonoBehaviour
{
   // public StateManager stateManager;
    public Player player;
    

    public Image coolTImeFillQ; //아이콘에 필어마운트 가져올거
    public Image coolTImeFillE;
    public Image coolTImeFillR;

    public Image qskillIcon;
    public Image eskillIcon;
    public Image rskillIcon;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        if(player != null)
        {
            coolTImeFillQ = GameObject.Find("CoolTimeBGQ").GetComponent<Image>();
            coolTImeFillE = GameObject.Find("CoolTimeBGE").GetComponent<Image>();
            coolTImeFillR = GameObject.Find("CoolTimeBGR").GetComponent<Image>();

            qskillIcon = GameObject.Find("SkillIconQ").GetComponent<Image>();
            qskillIcon = GameObject.Find("SkillIconE").GetComponent<Image>();
            qskillIcon = GameObject.Find("SkillIconR").GetComponent<Image>();
        }
    }

    private void Start()
    {
        coolTImeFillQ.fillAmount = 0;
        coolTImeFillE.fillAmount = 0;
        coolTImeFillR.fillAmount = 0;
    }

    void Update()
    {
        if(!player.qisReady)
        {
            coolTImeFillQ.fillAmount = 1 - player.qskillcool / player.curQskillcool;
        }
        if(!player.eisReady)
        {
            coolTImeFillE.fillAmount = 1 - player.eskillcool / player.curEskillcool;
        }
        if (!player.risReady)
        {
            coolTImeFillR.fillAmount = 1 - player.rskillcool / player.curRskillcool;
        }
    }

    
}
