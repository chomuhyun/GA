using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    public GameObject wall;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Jun_TweenRuntime[] gameObject = wall.GetComponents<Jun_TweenRuntime>();
            gameObject[0].Play();
        }
    }
}