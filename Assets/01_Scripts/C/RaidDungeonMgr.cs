using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RaidDungeonMgr : MonoBehaviourPunCallbacks
{
    public int clearCut = 1;
    public RewardMgr rewardMgr;
    public RaidBossCtrl boss;
    public DataMgrDontDestroy dataMgrDontDestroy;
    public RaidQuestManager raidQuestManager;
    private void Start()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
    }

    private void Update()
    {
        clear();
    }
    public void ClearEndBoss()
    {
        rewardMgr.ShowReward();
    }
        void clear()
    {
        if(rewardMgr.clearCut == 1)
        {
            if (boss.GetComponent<RaidBossCtrl>().die == true)
            {
                ClearEndBoss();
                raidQuestManager.ClearEndBs(3);
            }
        }
    }
    public void MoveTown()
    {
        dataMgrDontDestroy.DungeonSortIdx = 0;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("DungeonLoadingScene");
    }
}
