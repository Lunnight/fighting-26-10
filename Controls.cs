using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Unity.VisualScripting;

public class Controls : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;  //rb as in rigidbody
    float inputX;

    public float speed = 8f;

    //for attack function
    public LayerMask enemyLayer;    //makes sure the player attacks the enemy (right layer)
    public int attackDamage = 40;
    public float attackRange = 0.5f;
    public Transform attackPoint;   //ig to move the attack point when the player moves?

    //cooldown on attack;
    public Stopwatch timing;
    public long cooldown;


    // Start is called before the first frame update
    void Start()
    {
        //grabbing components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        timing = new Stopwatch();
        timing.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //grabs horizontal movement input
        inputX = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown("space"))
        {
            timing.Stop();
            cooldown = timing.ElapsedMilliseconds;
            if (cooldown >= 1000)
            {
                Attack();
                timing.Reset();
            }
            timing.Start();
        }
    }

    //putting physics + animation triggers in fixed update
    //FixedUpdate is called every frame
    private void FixedUpdate()
    {
        //flips sprite on x-axis when changing direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //give character velocity to move
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);

        //set animation triggers for run (positive) and idle (negative)
        if (Mathf.Abs(inputX) > 0)
        {
            animator.SetTrigger("Run");
        }
        else
        {
            animator.SetTrigger("Idle");
        }

    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        //[] means array
        //creates array with every layer an enemy could be on and if they collide with our attack point
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in enemies) //for each possible enemy collision
        {
            print("we hit" + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage); //make the enemy take damage using TakeDamage() function
        }
 
    }
}