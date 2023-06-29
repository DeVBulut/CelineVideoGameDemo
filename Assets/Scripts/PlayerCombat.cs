using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController pController;
    private float cooldown = 0.25f;
    private float cooldownCounter;
    public bool isAttacking;
    public bool isAttackingSlow;
    private bool canAttack = true;
    private float originalGravity;
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
        if(Input.GetKeyDown(KeyCode.Mouse0) && cooldownCounter < 0) {  Slash(); }


        if(isAttackingSlow == true && pController.IsGrounded()){
                isAttackingSlow = false;
        }
    }

    public void Slash()
    {
        cooldownCounter = cooldown;
        #region SlashMovement

        if(canAttack){
            isAttacking = true;
            // Calculate the direction from the player to the mouse position
            Vector2 dashDirection = (GetPositionOfMouse() - transform.position).normalized;
            // Apply the dash force to the player
            rb.velocity = Vector2.zero;
            rb.AddForce(dashDirection * attackSpeed, ForceMode2D.Impulse);
            StartCoroutine(SlowDownAttack());
        }
        // else{
        //     // Calculate the direction from the player to the mouse position
        //     Vector2 dashDirection = (GetPositionOfMouse() - transform.position).normalized;
        //     // Apply the dash force to the player
        //     rb.AddForce(dashDirection, ForceMode2D.Impulse);
        // }

        #endregion

    }

    IEnumerator SlowDownAttack(){

    yield return new WaitForSeconds(0.1f); // Adjust this delay as needed

    isAttackingSlow = true;
    isAttacking = false;

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
            Gizmos.color = Color.blue; 

    }

}

