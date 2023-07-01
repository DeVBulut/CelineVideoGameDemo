using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{   
    Animator animator; 
    private void Start() {
    animator = GetComponent<Animator>();
   }

   private void Update(){
    if(Input.GetKeyDown(KeyCode.F)){
            animator.CrossFade("Player_Attack", 0, 0);
    }
   }

}
