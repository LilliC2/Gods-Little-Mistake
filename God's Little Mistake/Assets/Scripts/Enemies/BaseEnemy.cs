using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : GameBehaviour
{
    public enum EnemyState
    {
        Patrolling, Chase, Attacking, Die
    }

    public EnemyState enemyState;

    public SpriteRenderer image;
    public EnemyStats stats;

    private void Start()
    {
        image = GetComponentInChildren<SpriteRenderer>();

    }

    private void Update()
    {
        HealthVisualIndicator(stats.health, stats.maxHP);

        //Health Manager
        if (stats.health <= 0)
        {
            enemyState = EnemyState.Die;
        }

    }

    void HealthVisualIndicator(float _health, float _maxHP)
    {
        float currentHPpercent = _health / _maxHP;

        float H, S, V;

        Color.RGBToHSV(image.color, out H, out S, out V);

        image.color = Color.HSVToRGB(H, currentHPpercent, V);
    }

    void Hit()
    {
        if (stats.health > 0)
        {
            stats.health -= _PC.dmg;
            //print(enemyStats.stats.health);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            print("hit");
            //Add hit code here;
            Hit();

            //destroy bullet that hit it
            Destroy(collision.gameObject);
        }
    }

    public void Die()
    {
        //eye is for testing
        Instantiate(_IG.GenerateItem(stats.category.ToString()), gameObject.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
