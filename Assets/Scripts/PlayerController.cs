using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public string playerState;
	public TextMeshProUGUI textState;
	public TextMeshProUGUI textVerticalSpeedState;
	public TextMeshProUGUI textHorizontalSpeedState;
	private Rigidbody2D rb;
	private PlayerCombat pCombat;
	private PlayerMovement pMovement;

	private float _MaxCoyoteTimeValue = 0.25f;
	[SerializeField] private LayerMask _GroundLayers; 
	private Transform m_GroundCheck;
	public Transform m_GroundCheck_2;
	public bool _CoyoteTime; 
	public float coyoteTimeValue;
	private float fallMultiplier = 1.2f; 
	private float peakMultiplier = 0.5f;
	private float ascendMultiplier = 0.7f;
	private Animator animator;

	#region Dash Values

	private bool isDashing;

	#endregion
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		pCombat = GetComponent<PlayerCombat>();
		pMovement = GetComponent<PlayerMovement>();
		animator = GetComponent<Animator>();
		m_GroundCheck = transform.GetChild(0);
		textState = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		StateCheck();
		StateController();
		SetGravity();
		textState.text = playerState;
        textVerticalSpeedState.text = Input.GetAxisRaw("Horizontal").ToString();
        //textHorizontalSpeedState.text = rb.velocity.x.ToString();
    }

	 public void StateController(){

		if(IsGrounded())
		{
			Vector2 velocity =  rb.velocity;
			float magnitude = velocity.x;

			animator.SetBool(AnimationStrings.OnGround, true);
			
			if(magnitude > 0.2f || magnitude < -0.2f)
			{
				animator.SetBool(AnimationStrings.Moving, true);
			}
			else if(magnitude < 0.2f && magnitude > -0.2f)
			{
				animator.SetBool(AnimationStrings.Moving, false);
			}
		}
		else if(!IsGrounded())
		{

			Vector2 velocity =  rb.velocity;
			float magnitude = velocity.y;
			
			animator.SetBool(AnimationStrings.OnGround, false);

			if(magnitude <= 1f && magnitude > 0f)
			{

				pMovement.airSpeed = 9f;
				animator.SetBool(AnimationStrings.Ascend, false);
				animator.SetBool(AnimationStrings.Descend, false);
				playerState = "Peak";
			}
			else if(magnitude > 1f)
			{
				pMovement.airSpeed = 8f; 
				animator.SetBool(AnimationStrings.Ascend, true);
				animator.SetBool(AnimationStrings.Descend, false);
				playerState = "Ascend";
			}else if(magnitude < 0f){

				pMovement.airSpeed = 8f;
				animator.SetBool(AnimationStrings.Ascend, false);
				animator.SetBool(AnimationStrings.Descend, true);
				playerState = "Descend";
			}  
		}
	}

	public void SetGravity(){
		
		if(playerState == "Peak")
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (peakMultiplier - 1) * Time.deltaTime;
		}
		else if(playerState == "Descend"){

			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

		}else if(playerState == "Ascend"){

			rb.velocity += Vector2.up * Physics2D.gravity.y * (ascendMultiplier - 1) * Time.deltaTime;

		}
	}
	
	private void StateCheck(){


		if(IsGrounded()){

			coyoteTimeValue = _MaxCoyoteTimeValue;
		}else{
			coyoteTimeValue -= Time.deltaTime; 
		}
		
		if (coyoteTimeValue > 0f) 
		{
			_CoyoteTime = true;
		}
		else{
			_CoyoteTime = false;
		}
	}

	public bool IsGrounded()
	{
		return Physics2D.OverlapCircle(m_GroundCheck.position, 0.2f, _GroundLayers);
	}

	private void OnDrawGizmos()
	{
		// Draw a line from player to target in Scene view
		if (this.transform != null && this.transform != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(this.transform.position, m_GroundCheck_2.position);
		}
	}

}
