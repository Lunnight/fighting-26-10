using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//credits for how to follow the player: https://www.youtube.com/watch?v=dmQyfWxUNPw
//"follow ai" portion

public class FollowPlayer : MonoBehaviour
{
    public float speed;
    public Transform target;

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //fixed update for set speed updates
    void FixedUpdate()
    {
        //moves enemy towards player
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed);


        //checks if the animator tag thingy "IsDead" is on true
        if (anim.GetBool("IsDead") == true)
        {
            this.enabled = false;
        }
    }
}
