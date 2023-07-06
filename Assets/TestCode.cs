using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{   
     public Animator animator;
     private void Update() {
        if(Input.GetKeyDown(KeyCode.F)){
                animator.SetTrigger(AnimationStrings.AttackTrigger);
        }
    }


   
}
