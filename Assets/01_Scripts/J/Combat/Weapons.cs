using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public enum Type {Melee, Range};
    public Type type;
    public int damage;
    public float rate;
    public GameObject weapons;
    public BoxCollider meleeArea;
    public TrailRenderer trailRenderer;
    public Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    public void WeaponUse()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }

    public void Attack1()
    {
        player.isAttack1 = true;
    }
    public void Attack2()
    {
        player.isAttack2 = true;
    }

    public void Attack3()
    {
        player.isAttack3 = true;
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        //meleeArea.enabled = true;
        trailRenderer.enabled = true;
        yield return new WaitForSeconds(0.3f);
        //meleeArea.enabled = false;
        yield return new WaitForSeconds(2.5f);
        trailRenderer.enabled = false;
    }

    public void EnableCollider(int isEnable)
    {
        if (weapons != null)
        {
            var col = weapons.GetComponent<BoxCollider>();
            if (col != null)
            {
                if (isEnable == 1)
                {
                    col.enabled = true;
                }
                else
                {
                    col.enabled = false;
                }
            }
        }
    }
}
