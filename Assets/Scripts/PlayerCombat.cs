using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
		private Rigidbody2D rb;
		private PlayerController pController;
		public Transform attackColliderPos_1;
		public Transform attackColliderPos_2;

		public bool isAttacking;
		private float originalGravity;
		[SerializeField] private LayerMask HittableLayers;
		[Range(0, 10)][SerializeField] private int AttackDamage;    

		public float cooldownTime = 2f; 
		private float nextFireTime = 0.2f;
		public int noOFClicks = 0;
		float lastClickedTime = 0;
		float maxComboDelay = 0.3f;

		Animator animator;               

		private void Start(){
				rb = GetComponent<Rigidbody2D>();
				originalGravity = rb.gravityScale;
				animator = GetComponent<Animator>();
		}

		void Update()
		{
				Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

				 if(Input.GetKeyDown(KeyCode.F))
				 {
				
						if(!animator.GetCurrentAnimatorStateInfo(0).IsName("hit1") && !animator.GetCurrentAnimatorStateInfo(0).IsName("hit2")){
										Debug.Log("First Attack");
										isAttacking = true;
										StartCoroutine(Attack(1));
						}
						if(animator.GetCurrentAnimatorStateInfo(0).IsName("hit1")){
										Debug.Log("Second Attack");
										isAttacking = true;
										StartCoroutine(Attack(2));
						}
						if(animator.GetCurrentAnimatorStateInfo(0).IsName("hit2")){
										Debug.Log("Third Attack");
										isAttacking = true;
										StartCoroutine(Attack(3));
						}
						
				}

				if(AttackAnimatonPlaying() && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.5)
				{
							isAttacking = false;
				}
				
			
		}
		
	public bool AttackAnimatonPlaying(){
				return animator.GetCurrentAnimatorStateInfo(0).IsName("hit1") || animator.GetCurrentAnimatorStateInfo(0).IsName("hit2") || animator.GetCurrentAnimatorStateInfo(0).IsName("hit3");
	 }

	private IEnumerator Attack(int count)
	{
				if(AttackAnimatonPlaying())
				{
				yield return new WaitForSeconds(0.8f - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
						isAttacking = true;
						animator.CrossFade("hit"+ count, 0, 0);
				}
				else 
				{
				isAttacking = true;
				animator.CrossFade("hit"+ count, 0, 0);
				}
	 }		


		public void Slash()
		{
				Vector2 attackPos_1 = new Vector2(attackColliderPos_1.position.x, attackColliderPos_1.position.y);
				Vector2 attackPos_2 = new Vector2(attackColliderPos_2.position.x, attackColliderPos_2.position.y);
				Collider2D[] objectsInCollider = Physics2D.OverlapAreaAll(attackPos_1, attackPos_2, HittableLayers);
				foreach (var hittedObject in objectsInCollider)
				{
						EnemyHealth enemyHealth = hittedObject.GetComponent<EnemyHealth>();
						enemyHealth.GetHit(5, this.transform.position);
				}
		}

		private Vector3 GetPositionOfMouse()
		{
				// Get the position of the mouse in screen space
				Vector3 mousePosition = Input.mousePosition;
				// Set the z coordinate to the distance from the camera to the game object
				mousePosition.z = -Camera.main.transform.position.z;
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
