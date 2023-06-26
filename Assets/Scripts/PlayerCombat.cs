using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerController pController;
    [Range(0, 1f)]  [SerializeField] private float slashCooldown = 0.2f;
    [Range(0, 20f)] [SerializeField] private float additonalDeflectForce = 4f;
    [Range(0, 200f)][SerializeField] private float dashAttackSpeed = 4f;

    private Vector3 xnf;
    private Vector3 positionBehindEnemy;
    private Vector2 dashDirection;

    private float _AttackRange = 1.6f; //mouseun icidnde olupta attack yapabilecegii maksimum menzil
    private float deflectRange = 0.75f; //Deflect alani
    private float cooldown = 0.25f;
    public bool isAttacking, slowAttack;
    private bool positionBehindEnemyBoolean;
    public bool canAttack = true;

    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private LayerMask bulletLayers;
    [Range(0, 10)][SerializeField] private int AttackDamage;                    

    private void Start(){

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pController = GetComponent<PlayerController>();
    }

    void Update()
    {
        GetPositionOfMouse();
        //DroneDashCheck();
        cooldown = cooldown - Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Mouse0) && cooldown < 0) {  Slash(); }

    }

    public void Slash()
    {
        #region SlashMovement

        if(canAttack){
            isAttacking = true;
            // Calculate the direction from the player to the mouse position
            dashDirection = (GetPositionOfMouse() - transform.position).normalized;
            // Apply the dash force to the player
            rb.velocity = Vector2.zero;
            rb.AddForce(dashDirection * 10f, ForceMode2D.Impulse);
        }
        else{
            // Calculate the direction from the player to the mouse position
            dashDirection = (GetPositionOfMouse() - transform.position).normalized;
            // Apply the dash force to the player
            rb.AddForce(dashDirection, ForceMode2D.Impulse);
        }

        #endregion
       
        #region EnemyDetection
        //Bu attack range mouse bunun disindayken attack komutu calismiyor.
        Collider2D[] AttackRangeLimit = Physics2D.OverlapCircleAll(transform.position, _AttackRange);

        //buda mousein kendi icindeki alani attack yapmak icin. buyuk bir alan varki oyuncu biraz kenara bile tiklasa dusmana atak yapsin
        Collider2D[] EntitiesInsideMouseRange = Physics2D.OverlapCircleAll(DeflectLocationFind(GetPositionOfMouse()), deflectRange, enemyLayers); // center, radius, checking layer @cag

        foreach (Collider2D collider in AttackRangeLimit)
        {
            //Burda hem rangein hemde mouse alaninin icinde mi diye kontrol ediyor.
            if (EntitiesInsideMouseRange.Contains(collider))
            {
                if (collider.CompareTag("Bullet"))
                {
                    Rigidbody2D bulletRigidbody = collider.GetComponent<Rigidbody2D>();
                    bulletRigidbody.velocity = -bulletRigidbody.velocity;
                    collider.transform.Rotate(Vector3.forward, 180f);
                    Vector3 theScale = collider.transform.localScale;
                    theScale.y *= -1;
                    collider.transform.localScale = theScale;
                    bulletRigidbody.velocity *= additonalDeflectForce;
                }
                else if (collider.gameObject.CompareTag("Drone"))
                {
                    Debug.Log("Enemy Drone Hit");
                    DashSlash(collider.transform, 2);
                    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
                    enemyHealth.GetHit(AttackDamage, this.gameObject.transform.position);
                }
                else
                {
                    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
                    enemyHealth.GetHit(AttackDamage, this.gameObject.transform.position);
                }
            }
        }
        #endregion
       
        cooldown = slashCooldown;
    }

    #region Dash-Slash Function
    public void DashSlash(Transform enemyTransform, float distanceBehind) 
    {
        //burasi klasik self explanatory
        positionBehindEnemyBoolean = true;
        Vector3 direction = (this.transform.position - enemyTransform.position).normalized;
        positionBehindEnemy = enemyTransform.position - (direction * distanceBehind);

        #region ColliderDisable

        //collideri kapatiyorum icinden gecebileyim diye
        Collider2D pCollider = this.gameObject.GetComponent<BoxCollider2D>();
        Collider2D eCollider = enemyTransform.gameObject.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(pCollider, eCollider, true);

        #endregion

        Vector3 force = positionBehindEnemy - this.transform.position;

        //z eksenine kuvveti kesiyorumki ebenin amina ucmasin
        force.z = 0f; 
        isAttacking = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(force * dashAttackSpeed, ForceMode2D.Impulse);
        Debug.Log("Dashed Through");
        //bu gizmoslar icin
        xnf = positionBehindEnemy;
    }
    private void DroneDashCheck()
    {
        Collider2D[] DroneCircle = Physics2D.OverlapCircleAll(positionBehindEnemy, 0.4f);

        if (positionBehindEnemyBoolean) 
        { foreach (Collider2D collider in DroneCircle)
         {
             if (collider.gameObject.CompareTag("Player"))
          { 
            positionBehindEnemyBoolean = false; isAttacking = false; rb.velocity = Vector2.zero; 
            } 
          }
        }
    }
    #endregion
    #region Mouse Functions
    private Vector3 GetPositionOfMouse()
    {
        
        // Get the position of the mouse in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Set the z coordinate to the distance from the camera to the game object
        mousePosition.z = -Camera.main.transform.position.z; // amacini anlamadim @cag

        // Convert the position from screen space to world space
        Vector3 worldPositionofMouse = Camera.main.ScreenToWorldPoint(mousePosition);

        return worldPositionofMouse;
    }

    private Vector2 DeflectLocationFind(Vector2 mousePosition)
    {
        Vector2 center = transform.position;
        Vector2 direction = mousePosition - center;
        float distance = Mathf.Min(direction.magnitude, _AttackRange);

        Vector2 collisionPoint = center + direction.normalized * distance;

        return collisionPoint;
    }
    
    #endregion 
    private void OnDrawGizmos()
    {
        
        //burasi editorde attack range ve mouse range'i gormek icin kodlari bulunduruyor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _AttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(DeflectLocationFind(GetPositionOfMouse()), deflectRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(xnf, 0.4f);
    }
}

