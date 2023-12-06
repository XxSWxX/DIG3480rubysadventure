using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public bool flying;
    public float changeTime = 3.0f;
    public float shootTime = 2.0f;

    public ParticleSystem smokeEffect;
    public ParticleSystem sparkEffect;

    public AudioClip brokenClip;
    public AudioClip flightClip;
    public AudioClip shootClip;
    public AudioClip hitClip1;
    public AudioClip hitClip2;

    public GameObject projectilePrefab;

    Rigidbody2D rigidbody2D;
    float wanderTimer;
    float shootTimer;
    public int direction = -1;
    bool broken = true;

    AudioSource audioSource;
    Animator animator;
    
    Vector2 lookDirection = new Vector2(1,0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        wanderTimer = changeTime;
        animator = GetComponent<Animator>();
        audioSource= GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        wanderTimer -= Time.deltaTime;
        shootTimer -= Time.deltaTime;

        if (flying)
        {
            if (shootTimer <0)
            {
                Launch();
                shootTimer = shootTime;
            }
        }

        if (wanderTimer < 0)
        {
            direction = -direction;
            wanderTimer = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            if(flying)
            {
            position.y = position.y + Time.deltaTime * speed * direction;
            position.x = position.x + Time.deltaTime * Mathf.Sin(Time.realtimeSinceStartup*2.0f) * 2;
            }
            else
            {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
            }

        }
        else
        {

            if(flying)
            {
            position.x = position.x + Time.deltaTime * speed * direction ;
            position.y = position.y + Time.deltaTime * Mathf.Sin(Time.realtimeSinceStartup*2.0f) * 2;
            }
            else
            {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);              
            }


        }
        
        rigidbody2D.MovePosition(position);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    
    void Launch()
    {
        PlaySound(shootClip);


        for (int i = 0; i < 4; ++i) 
        {
            
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.0f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            if(i == 0)
            {
                projectile.Launch(new Vector2(1,0), 200);
            } 
            if(i == 1)
            {
                projectile.Launch(new Vector2(0,1), 200);
            } 
            if(i == 2)
            {
                projectile.Launch(new Vector2(0,-1), 200);
            } 
            if(i == 3)
            {
                projectile.Launch(new Vector2(-1,0), 200);
            } 
            
        }

    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;

        //increse counter
        UIScore.instance.SetValue(1);

        audioSource.Stop();
        Random.InitState(System.DateTime.Now.Millisecond);
        int rand = Random.Range(0,2);
       
        if (rand == 0)
        {
        PlaySound(hitClip1);
        }
        else
        {
        PlaySound(hitClip2);
        }
        
        smokeEffect.Stop();
        sparkEffect.Stop();
        rigidbody2D.simulated = false;
        //optional if you added the fixed animation
        animator.SetTrigger("Fixed");
    }
}