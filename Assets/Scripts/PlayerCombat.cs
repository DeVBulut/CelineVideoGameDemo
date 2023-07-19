using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
	private Animator animator;
    private bool hasExecuted = false;

	private void Start(){
		animator = GetComponent<Animator>();
		
	}

    private void Update() {

        AttackSquence();

        if(Input.GetMouseButtonDown(0))
		{
            SetAnimatorAttackTriggers();
        }
    }

	private void AttackSquence(){

		if(animator.GetBool(AnimationStrings.canMove))
        {
            if (!hasExecuted)
            {
                hasExecuted = true;
                StartCoroutine(AttackSquenceDelay());
            }
        }
        else
        {
            hasExecuted = false;
        }
	}

	private void SetAnimatorAttackTriggers()
	{
			if(animator.GetInteger(AnimationStrings.AttackSquence) == 0)
			{
				animator.SetTrigger(AnimationStrings.AttackTrigger + "_1");
			}
            if(animator.GetInteger(AnimationStrings.AttackSquence) == 1)
			{
				animator.SetTrigger(AnimationStrings.AttackTrigger + "_2");
			}
            if(animator.GetInteger(AnimationStrings.AttackSquence) == 2)
			{
				animator.SetTrigger(AnimationStrings.AttackTrigger + "_3");
			}
            if(animator.GetInteger(AnimationStrings.AttackSquence) == 3)
			{
				animator.SetTrigger(AnimationStrings.AttackTrigger + "_1");
			}
	}
    private IEnumerator AttackSquenceDelay()
    {
        yield return new WaitForSeconds(1.3f);
        if(animator.GetBool(AnimationStrings.canMove)){
        animator.SetInteger(AnimationStrings.AttackSquence, 0);
        }
        
    }
}
