using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUDManager : MonoBehaviour
{

    public StateManager stateManager;
    public Slider HpSlider;
    public Text HpText;


    public Image DHpBar;




    //여기 함수를 피격판정에서 불러온다!
    private void Start()
    {
        stateManager = GetComponent<StateManager>();
        InitHP();
    }

    public void InitHP()
    {
        DHpBar.fillAmount = 1;
        HpSlider.value = (stateManager.hp / stateManager.maxhp);
        HpText.text = ((int)stateManager.hp + "/" + (int)stateManager.maxhp).ToString();
    }

    public void InitHPBtn()
    {
        stateManager.hp -= 20f;
        HpSlider.value = (stateManager.hp / stateManager.maxhp);
        HpText.text = ((int)stateManager.hp + "/" + (int)stateManager.maxhp).ToString();
    }
    public void ChangeUserHUD()
    {
        HpSlider.value = (stateManager.hp / stateManager.maxhp);
        HpText.text = ((int)stateManager.hp + "/" + (int)stateManager.maxhp).ToString();

        if (stateManager.hp <= 0)
        {
            Debug.Log("여기서 캐릭터 죽고 다음캐릭터로 강제로 넘겨야 함!!!");
        }
    }

    private void Update()
    {
        float targetFillAmount = Mathf.InverseLerp(0, stateManager.maxhp, stateManager.hp);

        if (DHpBar.fillAmount > targetFillAmount)
        {
            DHpBar.fillAmount -= 0.2f * Time.deltaTime;
            DHpBar.fillAmount = Mathf.Max(DHpBar.fillAmount, targetFillAmount);
        }
    }
    public void ChangeMainHP()
    {

    }





}