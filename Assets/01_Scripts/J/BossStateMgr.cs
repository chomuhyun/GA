using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossStateMgr : MonoBehaviourPunCallbacks
{
    public DataMgrDontDestroy dataMgrDontDestroy;

    [Header("Stet")]
    // 플레이어의 스텟!!!!
    public int level = 1;
    public int exp;
    public float maxhp;
    public float hp;
    public int weaponLevel;
    public int attackPower;
    public int criChance;
    public float criDamage;
    //public int classNum; // 클래스 변경시, 이 부분도 신경써주세요 1전사 2원딜 3마법

    [Space(10)]
    [Range(0, 100)]

    [HideInInspector]

    public HUDManager hudManager;
    public PhotonView pv;

    private void Start()
    {
        hudManager = gameObject.GetComponent<HUDManager>();
        pv = GetComponent<PhotonView>();
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;

        hp = maxhp;
    }


    #region 대미지 계산식######
    //공격력 30 100%
    // 스킬공 60 200 %= 200 / 100
    //내가 float값에 200 넣어서 60 딜이 나오려면
    //30 x 200 x n = 60
    //200n = 2
    //100n = 1
    //n = 0.01
    // 따라서 공격력 x 스킬대미지 x 0.01f 해야 퍼센트대미지가 나온다!
    // skillDMG는 기본적으로 100 줘야함
    /// atk * ( skillDMG * 0.01f); 를 하면, 150%로 줬을 때, 1.5배의 대미지가 들어간다!

    #endregion

    public void DealDamage(GameObject target, float skillDMG)//딜 계산, 105% 느낌으로 할 것!
    {
        Color popupColorsend = Color.gray;
        var monster = target.GetComponent<StateManager>();
        if (monster != null)
        {
            float totalDamage = attackPower * (skillDMG * Random.Range(0.005f, 0.01f));
            if (Random.Range(0f, 100f) <= criChance)
            {
                totalDamage *= criDamage * 0.02f;
                popupColorsend = Color.red;
            }

            monster.TakeDamage((int)totalDamage, popupColorsend);
        }
    }
    public void TakeDamage(int hit, Color popupColor) // 딜 팝업
    {
        hp -= hit;
        Vector3 randomness = new Vector3(Random.Range(-0.45f, 0.45f), Random.Range(-0.45f, 0.45f), Random.Range(0f, 0.25f));
        // hit - (hit*def/100)

        DamagePopUpGenerator.current.CreatePopup(transform.position + randomness, hit.ToString(), popupColor);
        //hudManager.ChangeUserHUD();
    }
}