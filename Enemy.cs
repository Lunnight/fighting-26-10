using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int maxHealth = 100;
    int currentHealth;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt"); //trigger's hurt animation
        if (currentHealth<= 0)
        {
            animator.SetBool("IsDead", true);

            //removes enemy code when it dies
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;   //stops this script
        }
    }
}
