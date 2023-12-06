using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
//MAIN SCRIPT
﻿public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;  
    float timeBoosted = 5;
    float boostTimer;
    public float speedBoost = 7;
    public bool boosting = false;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    public int health { get { return currentHealth; }}
    public int currentHealth;
    static public bool gameOver = false;

    public ParticleSystem hitEffect;
    public ParticleSystem healEffect;
    public AudioClip hitClip;
    public AudioClip throwClip;
    public GameObject projectilePrefab;
    public GameObject gameOverScreen;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    


    AudioSource audioSource;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioSource= GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if (boosting)
        boostTimer += Time.deltaTime;
        Debug.Log(timeBoosted);
        {
            if (boostTimer >timeBoosted)
            {
                speed = 3;
                boostTimer = timeBoosted;
                boosting = false;
            
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "SpeedPotion")
        {

           boosting = true;
           speed = 7;

           Destroy(other.gameObject);

        }

        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }  
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
            gameOver = false;
            }
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            //Vector2 position = rigidbody2d.position;
            PlaySound(hitClip);
            ParticleSystem hitParticle = Instantiate(hitEffect, rigidbody2d.position, Quaternion.identity);
            isInvincible = true;
            invincibleTimer = timeInvincible;
    

        }
        else
        {
            ParticleSystem healParticle = Instantiate(healEffect, rigidbody2d.position+new Vector2(0,1), healEffect.transform.rotation);
        }
        

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth == 0)
        {
            speed = 0.0f;
            gameOver = true;
            gameOverScreen.SetActive(true);
        }

    }
    
    void Launch()
    {
        PlaySound(throwClip);

        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
}