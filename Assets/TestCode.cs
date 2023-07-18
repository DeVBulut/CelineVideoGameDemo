using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestCode : MonoBehaviour
{   
    public Animator animator;
    public TextMeshProUGUI AttackCooldownStatus;
    private float countdownNormal = .05f;
    public float countdownCounter = .05f;
    public SetAttackBehaviour st;
    private bool hasExecuted = false;


    private void Update() {

        AttackCooldownStatus.text = countdownCounter.ToString();
        countdownCounter -= Time.deltaTime;

        if(countdownCounter < 0)
        {
            animator.SetBool("canAttack", true);
        }
        else
        {
            animator.SetBool("canAttack", false);
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_2") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            countdownCounter = countdownNormal;
        }

        if(animator.GetBool(AnimationStrings.canMove))
        {
            if (!hasExecuted)
            {
                countdownCounter = countdownNormal;
                hasExecuted = true;
            }
        }
        else
        {
            hasExecuted = false;
        }

        if(Input.GetMouseButtonDown(0) && countdownCounter < 0){

            animator.SetTrigger(AnimationStrings.AttackTrigger);
        }
    }
}
