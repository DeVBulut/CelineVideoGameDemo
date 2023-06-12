using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator animator;
    private PlayerCombat pCombat;
    private Rigidbody2D _rigid;

    #region Horizontal Movement Values

    private Vector3 _velocity = Vector3.zero;
    [Range(0, 100f)][SerializeField] private float runSpeed = 30f;
    [Range(0, .3f)][SerializeField] private float _movementSmoothing = .05f; 
    [Range(0, 2f)][SerializeField] private float _movementSmoothingonAir = .05f; 
    #endregion

    #region JumpValues
    [Range(0, 1000f)][SerializeField] private float JumpPower = 400f;
    private int JumpCount;
    private int _MaxJumpCount = 1;
    #endregion
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        pCombat = GetComponent<PlayerCombat>();
        _rigid = GetComponent<Rigidbody2D>();
        JumpCount = _MaxJumpCount;
    }

    void Update()
    {
        Jump();   
    }
    void FixedUpdate()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime;
        if(!pCombat.isAttacking) 
        {
            MoveHorizontal(horizontalMove);
        }
    }

    public void MoveHorizontal(float moveSpeed)
	{
		if (_playerController._Grounded)
		{
            // Hedef Velocityi bul. @Han
            Vector3 targetVelocity = new Vector2(moveSpeed * 10f, _rigid.velocity.y); //Karakterin Yatay Duzlemde Ulastigi Maksimum Hiz @Han
            // Buldugun Velocitiyi SmoothDamp ile uygula. @Han
            _rigid.velocity = Vector3.SmoothDamp(_rigid.velocity, targetVelocity, ref _velocity, _movementSmoothing);

            //Gittigi yone gore karakteri cevir @Han
            if (moveSpeed > 0)
			{
				_playerController.Flip('R');
			}else if (moveSpeed < 0)
			{
				_playerController.Flip('L');
			}

		}

        else if (!_playerController._Grounded)
        {
            // Hedef Velocityi bul. @Han
            Vector3 targetVelocity = new Vector2(moveSpeed * 10f, _rigid.velocity.y);
            // Buldugun Velocitiyi SmoothDamp ile uygula. @Han
            _rigid.velocity = Vector3.SmoothDamp(_rigid.velocity, targetVelocity, ref _velocity, _movementSmoothingonAir);

            //Gittigi yone gore karakteri cevir @Han
            if (moveSpeed > 0)
            {
                _playerController.Flip('R');
            }
            else if (moveSpeed < 0)
            {
                _playerController.Flip('L');
            }
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("IsJumping", true);
            if (_playerController._Grounded == true)
            {
                JumpCount = _MaxJumpCount;
            }

            if (JumpCount <= _MaxJumpCount - 1 && JumpCount > 0)
            {
                _playerController.JumpMechanic(JumpPower, true);
                JumpCount--;
            }
            else if (JumpCount > _MaxJumpCount - 1)
            {
                _playerController.JumpMechanic(JumpPower, false);
                JumpCount--;
            }
        }
    }
}
