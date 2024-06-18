using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderOner : MonoBehaviour
{
    NomalMonsterCtrl nomalMonster;
    StateManager stateManager;
    void Start()
    {
        nomalMonster = GetComponentInChildren<NomalMonsterCtrl>();
        stateManager = GetComponentInChildren<StateManager>();
        nomalMonster.originMesh.SetActive(true);
        nomalMonster.dieMesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator DeadProcess(float t)
    {
        nomalMonster.originMesh.SetActive(false);
        nomalMonster.dieMesh.enabled = true;
        yield return new WaitForSeconds(4f);
        while (transform.position.y > -t)
        {
            Vector3 temp = transform.position;
            temp.y -= Time.deltaTime;
            transform.position = temp;
            yield return new WaitForEndOfFrame();

        }
        Destroy(gameObject, 6f);

    }
}
