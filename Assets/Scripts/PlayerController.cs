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
	private Rigidbody2D rb;//YOk ARTIIK RIGIDBODY!!! @Han
    private PlayerCombat pCombat;
    private PlayerMovement pMovement;
    //Daha Yumusak Gitmesini Sagliyo bu deger. @Han
    private float _MaxCoyoteTimeValue = 0.3f;
    [SerializeField] private LayerMask _GroundLayers; //Ground Layerlari
    private Transform m_GroundCheck;
    public bool _Grounded; //Karakterin Ground Layer  olan objelere Dokunup Dokunmadigini Gosteren Deger. @Han
    public bool _CoyoteTime; //coyote time 
    private float coyoteTimeValue = 0.3f;
    private float fallMultiplier = 1.5f; 
    private float fallFastMultiplier = 1f; 
    private float peakMultiplier = 0.6f; 

    #region Dash Values
    
    private bool isDashing;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pCombat = GetComponent<PlayerCombat>();
        pMovement = GetComponent<PlayerMovement>();
        m_GroundCheck = transform.GetChild(0);
        textState = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        
    }

    private void Update()
	{
        StateCheck();
        StateController();
        SetGravity();
        textState.text = playerState;
        textVerticalSpeedState.text = rb.velocity.y.ToString();
        textHorizontalSpeedState.text = rb.velocity.x.ToString();
    }

     public void StateController(){

        if(pCombat.isAttacking){

            playerState = "Attacking";
        }else if(pMovement.isDashing){

            playerState = "Dashing";
        }
        else if(_Grounded)
        {

            Vector2 velocity =  rb.velocity;
            float magnitude = velocity.x;

            if(magnitude > 0.1f || magnitude < -0.1f){
                
                playerState = "Run";
            }
            else if(magnitude < 0.1f && magnitude > -0.1f){

                playerState = "Idle";
            }
        }
        else if(!_Grounded)
        {

            Vector2 velocity =  rb.velocity;
            float magnitude = velocity.y;


            if(magnitude <= 1f && magnitude >= -1f){

                pMovement.airSpeed = 9f;
                playerState = "Peak";
            }
             else if(magnitude > 1f){

                pMovement.airSpeed = 8f; 
                playerState = "Ascend";
            }else if(magnitude < -1f){

                pMovement.airSpeed = 8f;
                playerState = "Des_Fast";
            }
            else if(magnitude < -3.5f){

                pMovement.airSpeed = 7f;
                playerState = "Descend";
            }       
        }
    }

    public void SetGravity(){
        if(playerState == "Peak"){

            rb.velocity += Vector2.up * Physics2D.gravity.y * (peakMultiplier - 1) * Time.deltaTime;

        }
        else if (playerState == "descendFast"){

            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallFastMultiplier - 1) * Time.deltaTime;

        }
        else if(playerState == "Descend"){

            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        }
        else if(playerState == "Ascend"){

        }

    }
      private void StateCheck(){

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.2f, _GroundLayers);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _Grounded = true;
                _CoyoteTime = true;
                coyoteTimeValue = _MaxCoyoteTimeValue;
            }
        }

        if (colliders.Length <= 0)
        {
            _Grounded = false;
            coyoteTimeValue -= 1 * Time.deltaTime;
            if (coyoteTimeValue < 0f)
            {
                _CoyoteTime = false;
            }
        }
    }

}
