using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController pController;
    public Transform attackColliderPos_1;
    public Transform attackColliderPos_2;
    private float cooldown = 0.25f;
    private float cooldownCounter;

    private float AttackTimer;
    private float AttackTimerCooldown = 0.3f;
    private bool AttackTimerBool = false;
    public bool isAttacking;
    public bool isAttackingSlow;
    private bool canAttack = true;
    private float originalGravity;
    private bool HitSquence;
    [SerializeField] private float attackSpeed = 500f;
    [SerializeField] private LayerMask HittableLayers;
    [Range(0, 10)][SerializeField] private int AttackDamage;                    

    private void Start(){
        cooldownCounter = cooldown;
        rb = GetComponent<Rigidbody2D>();
        pController = GetComponent<PlayerController>();
        originalGravity = rb.gravityScale;
        
    }

    void Update()
    {
        cooldownCounter = cooldownCounter - Time.deltaTime;
        if(AttackTimerBool == true){
            AttackTimer = AttackTimer - Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) && cooldownCounter < 0) {  Attack(); }
    }

    public void Attack()
    {
        cooldownCounter = cooldown;
        #region SlashMovement

        if(canAttack){
            isAttacking  = true;
            rb.gravityScale *= 0f;
            rb.velocity = Vector2.zero;
            AttackTimerBool = true;
            Slash();
            StartCoroutine(ComboAttack());
            while(AttackTimer > 0){
                if(Input.GetKeyDown(KeyCode.Mouse0)){
                    Slash();
                    HitSquence = true;
                    StartCoroutine(ComboAttack());
                }
            }
            
        }

        #endregion

    }

    public void Slash(){
        Vector2 attackPos_1 = new Vector2(attackColliderPos_1.position.x, attackColliderPos_1.position.y);
        Vector2 attackPos_2 = new Vector2(attackColliderPos_2.position.x, attackColliderPos_2.position.y);
        Collider2D[] objectsInCollider = Physics2D.OverlapAreaAll(attackPos_1, attackPos_2, HittableLayers);
        foreach(var hittedObject in objectsInCollider){
            EnemyHealth enemyHealth = hittedObject.GetComponent<EnemyHealth>();
            enemyHealth.GetHit(5, this.transform.position);
        }
    }
    IEnumerator ComboAttack(){
        yield return new WaitForSeconds(0.3f);
        if(HitSquence != true){
            rb.gravityScale = originalGravity;
            isAttacking = false;
        }
    }



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

    private void OnDrawGizmos() {
            Vector2 attackPos_1 = new Vector2(attackColliderPos_1.position.x, attackColliderPos_1.position.y);
            Vector2 attackPos_2 = new Vector2(attackColliderPos_2.position.x, attackColliderPos_2.position.y);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((attackPos_1 + attackPos_2) * 0.5f, attackPos_2 - attackPos_1);
    }

}

