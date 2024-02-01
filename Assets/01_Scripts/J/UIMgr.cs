using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    Player player;

    public GameObject[] npcPanel;
    public GameObject[] npcCH;
    public GameObject[] playerSkillIcon;

    void Start()
    {
        UIstartOff();
        if (player != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }

    void UIstartOff()
    {
        foreach (GameObject obj in npcPanel)
        {
            obj.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
