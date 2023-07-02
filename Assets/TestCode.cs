using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{   
    Animator animator; 
    int count = 1;
    public bool isAttacking;
    private void Start() {
    animator = GetComponent<Animator>();
   }

   private void Update(){
        if(Input.GetKeyDown(KeyCode.F)){
                if(!animator.GetCurrentAnimatorStateInfo(0).IsName("hit1") && !animator.GetCurrentAnimatorStateInfo(0).IsName("hit2")){
                        Debug.Log("First Attack");
                        isAttacking = true;
                        StartCoroutine(Slash(1));
                }
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("hit1")){
                        Debug.Log("Second Attack");
                        isAttacking = true;
                        StartCoroutine(Slash(2));
                }
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("hit2")){
                        Debug.Log("Third Attack");
                        isAttacking = true;
                        StartCoroutine(Slash(3));
                }
        }

        if(AttackAnimatonPlaying() && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                isAttacking = false;
        }else{
                isAttacking = true;
        }
   }

   public bool AttackAnimatonPlaying(){
        return animator.GetCurrentAnimatorStateInfo(0).IsName("hit1") || animator.GetCurrentAnimatorStateInfo(0).IsName("hit2") || animator.GetCurrentAnimatorStateInfo(0).IsName("hit3");
   }

   private IEnumerator Slash(int count){
        if(AttackAnimatonPlaying()){
        yield return new WaitForSeconds(0.7f - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        animator.CrossFade("hit"+ count, 0, 0);
        }else {
        yield return new WaitForSeconds(0.005f);
        animator.CrossFade("hit"+ count, 0, 0);
        }

   }

}
