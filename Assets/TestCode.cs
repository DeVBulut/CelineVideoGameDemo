using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{   
     public Animator animator;
     public float countdownCounter;
     private void Update() {

        if(Input.GetKeyDown(KeyCode.F)){
                animator.SetTrigger(AnimationStrings.AttackTrigger);
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_0") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_2")){
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99){
                Debug.Log("Cooldown");
            }
        }

    }

   
}
