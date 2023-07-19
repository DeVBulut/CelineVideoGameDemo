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
                hasExecuted = true;
                //countdownCounter = countdownNormal;
                StartCoroutine(AttackSquenceDelay());
            }
        }
        else
        {
            hasExecuted = false;
        }

        if(Input.GetMouseButtonDown(0) && countdownCounter < 0){
            
            if(animator.GetInteger(AnimationStrings.AttackSquence) == 0){
            animator.SetTrigger(AnimationStrings.AttackTrigger + "_1");
            }
            if(animator.GetInteger(AnimationStrings.AttackSquence) == 1){
            animator.SetTrigger(AnimationStrings.AttackTrigger + "_2");
            }
            if(animator.GetInteger(AnimationStrings.AttackSquence) == 2){
            animator.SetTrigger(AnimationStrings.AttackTrigger + "_3");
            }
            if(animator.GetInteger(AnimationStrings.AttackSquence) == 3){
            animator.SetTrigger(AnimationStrings.AttackTrigger + "_1");
            }
        }
    }

    private IEnumerator AttackSquenceDelay()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Squence Reset");
        if(animator.GetBool(AnimationStrings.canMove)){
        animator.SetInteger(AnimationStrings.AttackSquence, 0);
        }
        
    }
}
