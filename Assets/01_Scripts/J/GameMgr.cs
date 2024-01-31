using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public GameObject[] players;

    public GameObject[] skillZip;

    public bool sActive;
    public bool aActive;
    public bool mAcive;
    void Start()
    {
        skillZip[0].SetActive(false);
        skillZip[1].SetActive(false);
        skillZip[2].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Splayer()
    {
        sActive = true;
        if(sActive)
        {
            players[0].SetActive(true);
            players[1].SetActive(false);
            players[2].SetActive(false);
            aActive = false;
            mAcive = false;
            skillZip[0].SetActive(true);
            skillZip[1].SetActive(false);
            skillZip[2].SetActive(false);
        }
    }
    public void Aplayer()
    {
        aActive = true;
        if(aActive)
        {
            players[0].SetActive(false);
            players[1].SetActive(true);
            players[2].SetActive(false);
            sActive = false;
            mAcive = false;
            skillZip[0].SetActive(false);
            skillZip[1].SetActive(true);
            skillZip[2].SetActive(false);
        }
    }
    public void Mplayer()
    {
        mAcive = true;
        if(mAcive)
        {
            players[0].SetActive(false);
            players[1].SetActive(false);
            players[2].SetActive(true);
            sActive = false;
            aActive = false;
            skillZip[0].SetActive(false);
            skillZip[1].SetActive(false);
            skillZip[2].SetActive(true);
        }
    }
}
