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
	public float dashSpeed;
	private float gravityScale;

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
		Dash();   
	}

	void FixedUpdate()
	{
		Flip();
		if(animator.GetBool(AnimationStrings.canMove) == true)
		{
        	MoveHorizontal(1);
		}
		else if(isDashing)
		{
			
		}
		else
		{
			MoveHorizontal(0);
		}
    }

	private void Dash(){
		if(Input.GetKeyDown(KeyCode.LeftShift)){
		isDashing = true;
		animator.SetBool(AnimationStrings.canMove, false);
		rb.velocity = Vector2.zero;
	    gravityScale = rb.gravityScale;
		rb.gravityScale = 0;
		rb.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);
		StartCoroutine(DashDelay());
		}
	}

	private IEnumerator DashDelay(){
		yield return new WaitForSeconds(.14f);
		rb.gravityScale = gravityScale;
		rb.velocity = Vector2.zero;
		isDashing = false;
		animator.SetBool(AnimationStrings.canMove, true);

	}

	#region Horizontal Functions
	public void MoveHorizontal(int canMove)
	{
		if(canMove == 0){
			rb.velocity = Vector2.zero;
		}
		float moveSpeed = Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime * canMove;
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
	}
	

	public void Flip(){
        float inputEntered = Input.GetAxisRaw("Horizontal");
        if (inputEntered > 0)
		{
			Flip('R');
		}else if (inputEntered < 0)
		{
			Flip('L');
		}
	}
	
	#endregion

	#region Jump Mechanic

	public void Jump(){

		if (pController._CoyoteTime && Input.GetButtonDown("Jump") && rb.velocity.y < 0.01f || pController._CoyoteTime && Input.GetKeyDown(KeyCode.W) && rb.velocity.y < 0.01f)
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
