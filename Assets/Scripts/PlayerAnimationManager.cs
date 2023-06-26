using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
   PlayerController pCont; 
   Animator animator; 

    void Start(){
        pCont = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        switch(pCont.playerState){

            case "Attacking" :
            animator.CrossFade("Player_Attack", 0, 0);
            break;
            
            case "Run" :
            animator.CrossFade("Player_Run", 0, 0);
            break;

            case "Idle" :
            animator.CrossFade("Player_Idle", 0, 0);
            break;

            case "Ascend" :
            animator.CrossFade("Player_Ascend", 0, 0);
            break;

            case "Descend" :
            animator.CrossFade("Player_Descend", 0, 0);
            break;
    
            case "Des_Fast" :
            animator.CrossFade("Player_Descend", 0, 0);
            break;
        }
    }
}
