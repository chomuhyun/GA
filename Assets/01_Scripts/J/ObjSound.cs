using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSound : MonoBehaviour
{
    public GameObject os;
    public AudioClip btnSound;
    AudioSource audioSource;
    [Header("PlayerSkillSound")]
    public AudioClip aP_qSound;
    public AudioClip aP_eSound;
    public AudioClip aP_rSound;
    public AudioClip aP_normalAtkSound;

    public AudioClip sP_qSound;
    public AudioClip sP_eSound;
    public AudioClip sP_rSound;
    public AudioClip sP_normalAtkSound;

    public AudioClip mP_qSound;
    public AudioClip mP_eSound;
    public AudioClip mP_rSound;
    public AudioClip mP_normalAtkSound;

    [Header("BossPatternSound")]
    public AudioClip bossFire;
    public AudioClip swordPattern;
    public AudioClip healthUpPattern;
    public AudioClip spinPattern;

    public AudioClip groundCrush;
         
    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("AM").GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    public void BtnSound()
    {
        audioSource.PlayOneShot(btnSound);
    }

    public void SplayerQskillSound()
    {
        audioSource.PlayOneShot(sP_qSound);
    }
    public void SplayerEskillSound()
    {
        audioSource.PlayOneShot(sP_eSound);
    }
    public void SplayerRskillSound()
    {
        audioSource.PlayOneShot(sP_rSound);
    }
    public void SplayerAtklSound()
    {
        audioSource.PlayOneShot(sP_normalAtkSound);
    }

    public void AplayerQskillSound()
    {
        audioSource.PlayOneShot(aP_qSound);
    }
    public void AplayerEskillSound()
    {
        audioSource.PlayOneShot(aP_eSound);
    }
    public void AplayerRskillSound()
    {
        audioSource.PlayOneShot(aP_rSound);
    }
    public void AplayerAtkSound()
    {
        audioSource.PlayOneShot(aP_normalAtkSound);
    }

    public void MplayerQskillSound()
    {
        audioSource.PlayOneShot(mP_qSound);
    }
    public void MplayerEskillSound()
    {
        audioSource.PlayOneShot(mP_eSound);
    }
    public void MplayerRskillSound()
    {
        audioSource.PlayOneShot(mP_rSound);
    }
    public void MplayerAtkSound()
    {
        audioSource.PlayOneShot(mP_normalAtkSound);
    }
    public void BossFirePatternSound()
    {
        audioSource.PlayOneShot(bossFire);
    }
    public void BossSwordPatternSound()
    {
        audioSource.PlayOneShot(swordPattern);
    }
    public void BossHealthUpPatternSound()
    {
        audioSource.PlayOneShot(healthUpPattern);
    }
    public void BossSpinPatternSound()
    {
        audioSource.PlayOneShot(spinPattern);
    }
    public void GroundCrush()
    {
        audioSource.PlayOneShot(groundCrush);
    }
}
