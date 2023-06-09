using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private float _AttackRange = 1.5f; //mouseun icinde olupta attack yapabilecegii maksimum menzil
    private float mouseSnapRange = 0.6f; //mouseun etrafindaki alan
    private float deflectRange = 0.7f; //Deflect alani
    [Range(0, 20f)][SerializeField] private float deflectSpeedMultiplier = 10f;
    [Range(0, 20f)][SerializeField] private float additonalDeflectForce = 4f;
    [Range(0, 200f)][SerializeField] private float dashAttackSpeed = 4f;
    private Vector3 xnf;
    private Vector3 positionBehindEnemy;

    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private LayerMask bulletLayers;
    [Range(0, 10)][SerializeField] private int AttackDamage;
    public bool isAttacking;
    private bool positionBehindEnemyBoolean;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        GetPositionofMouse();

        CheckifDashedThrough();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Deflect();
        }

    }
    public void Attack()
    {
        // Play an attack animation
        animator.SetTrigger("Attack");

        #region Attack Movement
        isAttacking = true;
        StartCoroutine(FailSafe(0.25f));

        // Calculate the direction from the player to the mouse position
        Vector2 dashDirection = (GetPositionofMouse() - transform.position).normalized;

        // Apply the dash force to the player
        rb.AddForce(dashDirection * 10f, ForceMode2D.Impulse);
        #endregion

        #region Damage the Enemy
        //Bu attack range mouse bunun disindayken attack komutu calismiyor.
        Collider2D[] AttackRangeLimit = Physics2D.OverlapCircleAll(transform.position, _AttackRange);
        //buda mousein kendi icindeki alani attack yapmak icin. buyuk bir alan varki oyuncu biraz kenara bile tiklasa dusmana atak yapsin
        Collider2D[] enemiesInRangeOfMouse = Physics2D.OverlapCircleAll(GetPositionofMouse(), mouseSnapRange, enemyLayers); // center, radius, checking layer @cag

        foreach (Collider2D collider in enemiesInRangeOfMouse)
        {
            //Burda hem rangein hem de mouse alaninin icinde mi diye kontrol ediyor.
            if (AttackRangeLimit.Contains(collider))
            {
                if (collider.gameObject.CompareTag("Drone")) {
                    Debug.Log("Enemy Drone Hit");
                    DashAttack(collider.transform, 2);
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
    }
    public void Deflect()
    {
        //Bu attack range mouse bunun disindayken attack komutu calismiyor.
        Collider2D[] AttackRangeLimit = Physics2D.OverlapCircleAll(transform.position, _AttackRange);

        //buda mousein kendi icindeki alani attack yapmak icin. buyuk bir alan varki oyuncu biraz kenara bile tiklasa dusmana atak yapsin
        Collider2D[] bulletsInRangeOfMouse = Physics2D.OverlapCircleAll(GetCollusionPoint(GetPositionofMouse()), deflectRange, bulletLayers); // center, radius, checking layer @cag

        foreach (Collider2D collider in AttackRangeLimit)
        {
            //Burda hem rangein hemde mouse alaninin icinde mi diye kontrol ediyor.
            if (bulletsInRangeOfMouse.Contains(collider))
            {
                GameObject bullet = collider.gameObject;

                if (bullet.CompareTag("Bullet"))
                {
                    Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

                    bulletRigidbody.velocity = -bulletRigidbody.velocity;

                    bullet.transform.Rotate(Vector3.forward, 180f);
                    Vector3 theScale = bullet.transform.localScale;
                    theScale.y *= -1;
                    bullet.transform.localScale = theScale;

                    bulletRigidbody.velocity *= additonalDeflectForce;
                }
            }
        }
    }
    public void DashAttack(Transform enemyTransform, float distanceBehind)
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
        StartCoroutine(FailSafe(0.25f));
        rb.velocity = Vector2.zero;
        rb.AddForce(force * dashAttackSpeed, ForceMode2D.Impulse);

        //bu gizmoslar icin
        xnf = positionBehindEnemy;
    }
    private void CheckifDashedThrough(){
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
    private void OnDrawGizmos()
    {
        //burasi editorde attack range ve mouse range'i gormek icin kodlari bulunduruyor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _AttackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetPositionofMouse(), mouseSnapRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetCollusionPoint(GetPositionofMouse()), mouseSnapRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(xnf, 0.4f);
    }
    public Vector3 GetPositionofMouse(){
        
        // Get the position of the mouse in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Set the z coordinate to the distance from the camera to the game object
        mousePosition.z = -Camera.main.transform.position.z; // amacini anlamadim @cag

        // Convert the position from screen space to world space
        Vector3 worldPositionofMouse = Camera.main.ScreenToWorldPoint(mousePosition);

        return worldPositionofMouse;
    }
    private Vector2 GetCollusionPoint(Vector2 mousePosition)
    {
        Vector2 center = transform.position;
        Vector2 direction = mousePosition - center;
        float distance = Mathf.Min(direction.magnitude, _AttackRange);

        Vector2 collisionPoint = center + direction.normalized * distance;

        return collisionPoint;
    }
    private IEnumerator FailSafe(float delay)
    {
        //bazi durumlarda karakterin arkasindaki lokasyona carpmiyor o yuzden failsafe mekanizmasi
        yield return new WaitForSeconds(delay);
        if (isAttacking == false) { yield break; }
        else
        {
            isAttacking = false;
            rb.velocity = Vector2.zero;
        }
    }
}