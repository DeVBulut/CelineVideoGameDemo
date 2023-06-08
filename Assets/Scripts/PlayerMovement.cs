using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator animator;
    private PlayerCombat pCombat;

    #region Horizontal Movement Values
    public float horizontalMove = 0f;
    [Range(0, 100f)][SerializeField] private float runSpeed = 30f;
    #endregion

    #region JumpValues
    [Range(0, 1000f)][SerializeField] private float JumpPower = 400f;
    private int JumpCount;
    private int _MaxJumpCount = 2;
    #endregion


    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        pCombat = GetComponent<PlayerCombat>();
        JumpCount = _MaxJumpCount;
    }

    void Update()
    {
        Jump();
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("IsJumping", true);
            if (_playerController._Grounded == true)
            {
                JumpCount = _MaxJumpCount;
                animator.SetBool("IsJumping", false);
            }

            if (JumpCount <= _MaxJumpCount - 1 && JumpCount > 0)
            {
                _playerController.Jump(JumpPower, true);
                JumpCount--;
            }
            else if (JumpCount > _MaxJumpCount - 1)
            {
                _playerController.Jump(JumpPower, false);
                JumpCount--;
            }
        }
    }


    private void FixedUpdate()
    {
        // bu kodun animasyon manager adli scriptin icinde bulunmasi gerekiyor
        // | | |
        // V V V
        //animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); // hiz - olunca mal olmamasi icin mathf'li @cag 
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime;
        if(!pCombat.isAttacking) {_playerController.Move(horizontalMove);}
    }



    
}
