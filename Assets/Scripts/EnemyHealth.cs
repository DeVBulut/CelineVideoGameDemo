using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour
{
    [Range(1, 100)][SerializeField] int HealthPoints = 2; //enemy cani
    [Range(0, 5)][SerializeField] float KnockbackForce = 3;  //geri atlama kuveti
    Rigidbody2D rb = new Rigidbody2D();
    [SerializeField] private bool Armored = false; //ilerde armored enemy koyariz diye

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (this.gameObject.CompareTag("Drone")) { HealthPoints = 10; } //CAGAAAAAAAAAANNNNNNNNNNN okuduysan guzel bunu istersen 1 e cevirebilirsin mekanik denemek icin yaptim bunu

    }
    public void GetHit(int Damage, Vector3 Attacker)
    {
        HealthPoints -= Damage;
        Debug.Log("Enemy Health is " + HealthPoints);

        if(HealthPoints <= 0)
        {
            Destroy(this.gameObject); //can sifirsa vur gote
        }
        else
        {
            if (!Armored && !this.gameObject.CompareTag("Drone")) {
                Debug.Log("knockbacked");
                KnockBack(Attacker, Damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("attackHitbox")){
            GetHit(1, other.gameObject.transform.position);
            Debug.Log("Got Hit by " + other.gameObject.name);
        }
    }


    public void KnockBack(Vector3 attackerPosition, int Damage)
    {
        Vector3 knockbackDirection = transform.position - attackerPosition; //atagin nerden geldigini hesapla
        knockbackDirection.Normalize();  //bu neden yapiliyo bilmiyorum ama koyunca cok daha guzel oldu

        //Karakterin vurdugu hasara oranla knockbackforce artiyor
        float damagePercent = Damage / 5; 
        float scaledKnockbackForce = KnockbackForce + damagePercent;

        //yeni vektor tanimliyoruz force x direction ile.
        Vector3 knockbackForce = knockbackDirection * scaledKnockbackForce; // nice @cag

        rb.AddForce(knockbackForce, ForceMode2D.Impulse); //bum!
    }
}
