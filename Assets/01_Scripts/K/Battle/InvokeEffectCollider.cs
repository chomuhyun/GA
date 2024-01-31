using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeEffectCollider : MonoBehaviour
{
    public GameObject plusPrefab;
    public float genTime = 0.5f;

    void Start()
    {
        InvokeRepeating("MakeCube", 0f, genTime);
    }

    void MakeCube()
    {
        float distanceUpper = 0;
        float distanceForward = Random.Range(-1f, 1f);
        Vector3 spawnPosition = transform.position + transform.right * distanceForward + transform.forward * distanceForward + transform.up * distanceUpper;
        Instantiate(plusPrefab, spawnPosition, transform.rotation);
        plusPrefab.GetComponent<WeaponsAttribute>().atkPer = this.GetComponent<WeaponsAttribute>().atkPer;
    }
}
