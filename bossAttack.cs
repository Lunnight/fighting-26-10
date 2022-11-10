using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//get rid of debug print statements when done

//credits:

//random num generation (mostly for how random works in c#):
//https://learn.microsoft.com/en-us/dotnet/api/system.random.next?redirectedfrom=MSDN&view=net-7.0#System_Random_Next_System_Int32_System_Int32_
//https://stackoverflow.com/questions/41924871/how-do-i-generate-random-number-between-0-and-1-in-c

public class bossAttack : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb2d;
    public Stopwatch cooldownTimer;
    long cooldownTime;
    int chosenAttack; // '0' = spin, '1' = normal
    public Transform player; // pig actor needs to be dragged into this
    public Rigidbody2D rbPlayer; //rigid body component of pig
    public float speed; //adjust speed in unity

    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        cooldownTimer = new Stopwatch();
        cooldownTimer.Start();

        //set boss to move around screen?
    }


    void FixedUpdate()
    {
       //if attack cooldown done then choose random attack and make boss pose
       if ( cooldownTimer.ElapsedMilliseconds >= 4000)
        {
            anim.SetTrigger("Pose");
            chosenAttack = Random.Range(0,1);
            print("posing");
            cooldownTime = cooldownTimer.ElapsedMilliseconds;
            
        }
        //if boss has posed for at least 1 second
        if( (cooldownTimer.ElapsedMilliseconds - cooldownTime) >= 1000 )
        {
            //spin attack chosen)
            if (chosenAttack == 0)
            {
                anim.SetBool("IsSpinning", true);
                anim.SetTrigger("SpinAttack");
                
                rb2d.velocity = new Vector2(0, 0);  //stops boss from moving
            }
            //normal attack chosen)
            else if (chosenAttack == 1)
            {
                anim.SetBool("NormalAttacking",true);
                anim.SetTrigger("NormalAttack");
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * 2);
                //get distance between player and boss and give boss force towards player
            }
            cooldownTimer.Restart();
            inProximity();
        }
        
        //else if (cooldown is greater than time for animation of spin and it spin)
        //change number of milliseconds for how long spin animation should last
        else if (cooldownTimer.ElapsedMilliseconds > 1000 && anim.GetBool("IsSpinning") == true)
        {
            anim.SetBool("IsSpinning",false);
        }
        //change number of milliseconds for how long normal attack animation should last
        else if (anim.GetBool("NormalAttacking") == true)
        {
            anim.SetBool("NormalAttacking", false);
        }

        //if boss not attacking or posing (use getBool)
        if (cooldownTimer.ElapsedMilliseconds >= 3000 && anim.GetBool("IsSpinning") == false && anim.GetBool("NormalAttacking") == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed);
        }
    }

    void inProximity()
    {
        float force;

        //if ([player is within a [] pixel raidus of the boss during the spin attack])
        //change the radius of the spin attack of the boss here
        if ((Vector2.Distance(transform.position, player.position) < 2) && (anim.GetBool("IsSpinning") == true))
        {
            //apply (strong) force to pig in correct direction
            if (transform.position.x < player.position.x)
            {
                force = 10;
            }
            else
            {
                force = -10;
            }
            rbPlayer.AddForce(new Vector2(force,0));
            print("spinning attack hit!!");
        }
        //else if (player within larger [] left or right distance and doing normal attack)
        else if ((Vector2.Distance(transform.position, player.position) < 1) && (anim.GetBool("NormalAttacking") == true))
        {
            //apply weaker force to pig in correct direction (left or right only)
            //stop velocity of boss charging
            if (transform.position.x < player.position.x)
            {
                force = 3;
            }
            else
            {
                force = -3;
            }
            rbPlayer.AddForce(new Vector2(force, 0));
            rb2d.velocity = new Vector2(0, 0);
            anim.SetBool("NormalAttacking", false);
            print("normal attack hit!");
        }
    }
}
