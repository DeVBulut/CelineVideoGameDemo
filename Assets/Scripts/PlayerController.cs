using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerState;
    public TextMeshProUGUI textState;
    public TextMeshProUGUI textSpeedState;
	private Rigidbody2D rb;//YOk ARTIIK RIGIDBODY!!! @Han
    private PlayerCombat pCombat;
    //Daha Yumusak Gitmesini Sagliyo bu deger. @Han
    private float _MaxCoyoteTimeValue = 0.3f;
    [SerializeField] private LayerMask _GroundLayers; //Ground Layerlari
    private Transform m_GroundCheck;
    public bool _Grounded; //Karakterin Ground Layer  olan objelere Dokunup Dokunmadigini Gosteren Deger. @Han
    public bool _CoyoteTime; //coyote time 
    private float coyoteTimeValue = 0.3f;
    private float fallMultiplier = 1.5f; 
    private float fallFastMultiplier = 1f; 
    private float peakMultiplier = 0.7f; 

    #region Dash Values
    
    private bool isDashing;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pCombat = GetComponent<PlayerCombat>();
        m_GroundCheck = transform.GetChild(0);
        textState = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        
    }

    private void Update()
	{
        StateCheck();
        StateController();
        SetFriction();
        textState.text = playerState;
        textSpeedState.text = rb.velocity.y.ToString();
    }

     public void StateController(){

        if(pCombat.isAttacking){

            playerState = "Attacking";
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


            if(magnitude <= 1.5f && magnitude >= -1.5f){

                playerState = "Peak";
            }
            else if(magnitude > 1.5f){
                
                playerState = "Ascend";
            }else if(magnitude < -1.5f){

                playerState = "descendFast";
            }
            else if(magnitude < -3.5f){

                playerState = "Descend";
            }
        
        }
    }

    public void SetFriction(){
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
