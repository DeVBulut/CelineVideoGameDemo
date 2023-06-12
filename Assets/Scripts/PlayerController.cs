using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerState;
    public TextMeshProUGUI textState;
	private Rigidbody2D _Rigid2D = new Rigidbody2D(); //YOk ARTIIK RIGIDBODY!!! @Han
    private PlayerCombat pCombat;
    //Daha Yumusak Gitmesini Sagliyo bu deger. @Han
    private float _MaxCoyoteTimeValue = 0.3f;
    [SerializeField] private LayerMask _GroundLayers; //Ground Layerlari
    private Transform m_GroundCheck;
    private Transform canvas;
    public bool _Grounded; //Karakterin Ground Layer  olan objelere Dokunup Dokunmadigini Gosteren Deger. @Han
    private bool _CoyoteTime; //coyote time 
    private float coyoteTimeValue = 0.3f;

    #region Dash Values
    
    private bool isDashing;

    #endregion

    void Start()
    {
        _Rigid2D = GetComponent<Rigidbody2D>();
        m_GroundCheck = transform.GetChild(0);
        pCombat = GetComponent<PlayerCombat>();
        textState = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        
    }

    private void FixedUpdate()
	{
        if(isDashing){ return; }

        StateCheck();
        StateController();
        textState.text = playerState;

    }

    public void StateController(){


        if(pCombat.isAttacking){
            playerState = "Attacking";
        }
        else if(_Grounded){

            Vector2 velocity =  _Rigid2D.velocity;
            float magnitude = velocity.x;

            if(magnitude > 0.1f || magnitude < -0.1f){
                
                playerState = "Run";
            }
            else if(magnitude < 0.1f && magnitude > -0.1f){

                playerState = "Idle";
            }
        }
        else if(!_Grounded){

            Vector2 velocity =  _Rigid2D.velocity;
            float magnitude = velocity.y;

             if(magnitude >= 0f){
                
                playerState = "Ascend";
            }
            else if(magnitude < 0f){

                playerState = "Descend";
            }
        }

    }

    public void JumpMechanic(float JumpForce, bool overpower)
    {
        //overpower true ise coyotime degerini bypass ediyor.
        if (_CoyoteTime || overpower == true)
        {
            _Rigid2D.AddForce(new Vector2(_Rigid2D.velocity.x, JumpForce));
        }
    }
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

    private void StateCheck(){

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.2f, _GroundLayers);
        //Burasi Karakter Ground Layerlarina degiyorsa Calisiyor @Han
        // | | | |
        // v v v v 
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _Grounded = true;
                _CoyoteTime = true;
                coyoteTimeValue = _MaxCoyoteTimeValue;

                #region AttackStateControl
                if(pCombat.canAttack == false){
                    pCombat.canAttack = true;
                }
                #endregion
            }
        }

        //Burasi Karakter Ground Layerlarina degmiyorsa Calisiyor @Han
        // | | | |
        // v v v v 
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
