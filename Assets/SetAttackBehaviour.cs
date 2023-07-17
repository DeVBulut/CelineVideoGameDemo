using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttackBehaviour : StateMachineBehaviour
{
    public string boolName;
    public bool valueOnEnter, valueOnExit;
    public short AttackSquence;
    private bool canAttack;


    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            if(stateInfo.IsName("Attack_0"))
            {
                AttackSquence = 1;
            }
            else if(stateInfo.IsName("Attack_1"))
            {
                AttackSquence = 2;
            }
            else if(stateInfo.IsName("Attack_2"))
            {
                AttackSquence = 3;
            }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
       
        animator.SetBool(boolName, valueOnEnter);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        AttackSquence = 0;        
        animator.SetBool(boolName, valueOnExit);
    
    }
}
