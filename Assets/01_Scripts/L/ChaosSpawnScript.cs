using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosSpawnScipt : MonoBehaviour
{
    public DataMgrDontDestroy dataMgrDontDestroy;
    public GameObject[] cH;
    public int classNum;

    private void Start()
    {
        dataMgrDontDestroy = DataMgrDontDestroy.Instance;
        StartCoroutine(SpawnPlayer());
    }
    IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(0.01f);
        CreatePlayer();
    }
    public void CreatePlayer()
    {
        int curSlotNum = SelectSlot.slotNum;
        int classNum = PlayerPrefs.GetInt($"{curSlotNum}_ClassNum");
        Debug.Log(classNum);
        Debug.Log("spawnScriptø°º≠ curSlotNum¿∫ " + curSlotNum);

        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        Instantiate(cH[classNum], points[idx].position, points[idx].rotation);
    }
}