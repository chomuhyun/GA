using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWave : MonoBehaviour
{
    public GameObject wave;
    public RaidBossCtrl raidBoss;

    public float rotationSpeed = 200f;
    public float targetRotationAngle = 45.0f;
    public float maxRotationAngle = 45.0f;
    private bool rotateClockwise = true;
    private bool rotateClockwises = true;
    private float initialRotation;

    void Start()
    {
        initialRotation = wave.transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("WavePattern", 2f);
        if (raidBoss.GetComponent<StateManager>().hp <= 0)
        {
            CancelInvoke("WavePattern");
        }
    }

    void WavePattern()
    {
        
        //if (raidBoss.GetComponent<StateManager>().hp <= raidBoss.GetComponent<StateManager>().maxhp / 2)
        {
            wave.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            float rotationDirection = rotateClockwise ? 1.0f : -1.0f;
            wave.transform.Rotate(Vector3.forward, rotationDirection * rotationSpeed * Time.deltaTime);
            if (Mathf.Abs(wave.transform.rotation.eulerAngles.z) >= maxRotationAngle)
            {
                rotateClockwise = !rotateClockwise;
            }
        }
    }
}
