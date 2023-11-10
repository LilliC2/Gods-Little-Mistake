using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : GameBehaviour
{
    [SerializeField]
    GameObject explosionAnimOB;
    [SerializeField]
    public GameObject image;
    [SerializeField]
    Animator explosionAnim;
    Rigidbody rb;
    [SerializeField]
    AudioSource explosionSound;

    public float initialDamage = 50f; 
    public float damageDecayRate = 0.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        explosionAnim = explosionAnimOB.GetComponent<Animator>();
    }

    private void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {

            // how much damage falls off
            float currentDamage = initialDamage * (1.0f - damageDecayRate);

            // so it doesnt do 0 damage
            currentDamage = Mathf.Max(currentDamage, 0.0f);
            Debug.Log("Damage dealt: " + currentDamage);

            //explosionAnimOB.SetActive(true);

            if (explosionSound != null) explosionSound.Play();

            image.SetActive(false);
            print("Destroy Projectile");
            //ooze animation
            explosionAnim.SetTrigger("Death");


            rb.velocity = Vector3.zero;
            Destroy(this.gameObject);

            //get dmg from enemy
            Hit(collision.collider.gameObject.GetComponent<BaseEnemy>().stats.dmg);
        }
    }
}
