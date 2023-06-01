using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D _Rigid2D = new Rigidbody2D(); //YOk ARTIIK RIGIDBODY!!! @Han

    [Range(0, .3f)][SerializeField] private float _MovementSmoothing = .05f; //Daha Yumusak Gitmesini Sagliyo bu deger. @Han
    private float _MaxCoyoteTimeValue = 0.3f;
    [SerializeField] private LayerMask _GroundLayers; //Ground Layerlari
    private Transform m_GroundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private float wallSlidingSpeed = 2f;



    private Vector3 _Velocity = Vector3.zero; //bu neden burda bilmiyom bosver bunu. @Han
    public bool _Grounded; //Karakterin Ground Layer  olan objelere Dokunup Dokunmadigini Gosteren Deger. @Han
    private bool _CoyoteTime; //coyote time 
    private float coyoteTimeValue = 0.3f;

    void Start()
    {
        _Rigid2D = GetComponent<Rigidbody2D>();
        m_GroundCheck = transform.GetChild(0);
    }

    private void Update()
    {
        PlayerMovement playerMovement = new PlayerMovement();
    }

    private void FixedUpdate()
	{
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

	public void Move(float moveSpeed)
	{
		if (_Grounded)
		{
            // Hedef Velocityi bul. @Han
            Vector3 targetVelocity = new Vector2(moveSpeed * 10f, _Rigid2D.velocity.y); //Karakterin Yatay Duzlemde Ulastigi Maksimum Hiz @Han
            // Buldugun Velocitiyi SmoothDamp ile uygula. @Han
            _Rigid2D.velocity = Vector3.SmoothDamp(_Rigid2D.velocity, targetVelocity, ref _Velocity, _MovementSmoothing);

            //Gittigi yone gore karakteri cevir @Han
            if (moveSpeed > 0)
			{
				Flip('R');
			}else if (moveSpeed < 0)
			{
				Flip('L');
			}

		}else if (!_Grounded)
        {
            // Hedef Velocityi bul. @Han
            Vector3 targetVelocity = new Vector2(moveSpeed * 10f, _Rigid2D.velocity.y);
            // Buldugun Velocitiyi SmoothDamp ile uygula. @Han
            _Rigid2D.velocity = Vector3.SmoothDamp(_Rigid2D.velocity, targetVelocity, ref _Velocity, _MovementSmoothing);

            //Gittigi yone gore karakteri cevir @Han
            if (moveSpeed > 0)
            {
                Flip('R');
            }
            else if (moveSpeed < 0)
            {
                Flip('L');
            }
        }
    }

    public void Jump(float JumpForce, bool overpower)
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

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
}
