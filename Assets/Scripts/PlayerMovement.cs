using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Components
    private PlayerController pController;
    private Animator animator;
    private PlayerCombat pCombat;
    private Rigidbody2D rb;
    #endregion

    #region Horizontal Movement Values

    private Vector3 _velocity = Vector3.zero;
    [Range(0, 100f)][SerializeField] private float runSpeed = 35f;
    [Range(0, 10f)][SerializeField] public float airSpeed = 7f;
    [Range(0, .3f)][SerializeField] private float _movementSmoothing = 0.15f; 
    #endregion

    #region JumpValues
    [Range(0, 1000f)][SerializeField] private float JumpPower = 400f;
    public int JumpCount;
    public int _MaxJumpCount = 1;
    #endregion

    #region DashValues 
    public bool isDashing = false;
    #endregion 

    void Start()
    {
        pController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        pCombat = GetComponent<PlayerCombat>();
        rb = GetComponent<Rigidbody2D>();
        JumpCount = _MaxJumpCount;
    }


    void Update(){
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

    #region Horizontal Functions
    public void MoveHorizontal(float moveSpeed)
	{
		if (pController.IsGrounded())
		{
            // Hedef Velocityi bul. @Han
            Vector3 targetVelocity = new Vector2(moveSpeed * 10f, rb.velocity.y); //Karakterin Yatay Duzlemde Ulastigi Maksimum Hiz @Han
            // Buldugun Velocitiyi SmoothDamp ile uygula. @Han
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _velocity, _movementSmoothing);
		} 
        else if (!pController.IsGrounded())
        {
            // Hedef Velocityi bul. @Han
            Vector3 targetVelocity = new Vector2(moveSpeed * airSpeed, rb.velocity.y);
            // Buldugun Velocitiyi SmoothDamp ile uygula. @Han
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _velocity, _movementSmoothing);
        }

        if (moveSpeed > 0)
		{
			Flip('R');
		}else if (moveSpeed < 0)
		{
			Flip('L');
		}
    }
    
    #endregion

    #region Jump Mechanic

    public void Jump(){

        if (pController._CoyoteTime && Input.GetButtonDown("Jump") && rb.velocity.y < 0.01f)
        {
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;
            rb.AddForce(new Vector2(rb.velocity.x, JumpPower));
        }
        

    }

    #endregion

     public void Flip(char direction)
    {
        // Karakterin X scale degerini tersine cevir. @Han

        if (direction == 'L')
        {
            Vector3 theScale = transform.localScale;
            theScale.x = -1;
            transform.localScale = theScale;
        }
        else if(direction == 'R')
        {
            Vector3 theScale =  transform.localScale;
            theScale.x = 1;
            transform.localScale = theScale;
        }
        
    }
}
