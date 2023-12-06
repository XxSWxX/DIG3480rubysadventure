using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    public bool enemy;
    float bulletLifetime = 1.5f;
    float bulletLifetimer;

    Rigidbody2D rigidbody2d;
    
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        bulletLifetimer = bulletLifetime;
    }
    
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    
    void Update()
    {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }

        bulletLifetimer -= Time.deltaTime;

        if (bulletLifetimer < 0)
        {
            bulletLifetimer = bulletLifetime;
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (enemy != true)
        {
            EnemyController e = other.collider.GetComponent<EnemyController>();
            if (e != null)
            {
                e.Fix();
            }
            Destroy(gameObject);
        }
        else
        {
            RubyController r = other.collider.GetComponent<RubyController>();
            if (r != null)
            {
                r.ChangeHealth(-1);
                
            }
            Destroy(gameObject);
        }

    }
}