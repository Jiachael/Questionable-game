
using System;
using System.Collections;
using System.Data;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private TrailRenderer tr;
    private Rigidbody2D body;
    private bool grounded = true;
    private bool doublejump = true;
    private bool isAvaliable; 

    private bool canDash = true;
    private bool isDash;
    private float dashPower = 30f;
    private float dashTime = 0.3f;
    private float dashCool = 1f;
    private float jumpCool = 1f;
        

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

    }

    //player updates
    private void Update()
    {
        if (isDash)
        {
            return;
        }

    //gets user input on the keyboard and moves
    float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        //player fliping when turning
        if (horizontalInput > 0.01f)
        { transform.localScale = Vector3.one; }

        else if (horizontalInput < -0.01f)
        { transform.localScale = new Vector3(-1, 1, 1); }
        // dashes

        if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }





        //jumping



        if (Input.GetKey(KeyCode.Space))

            if (grounded)
            {
                jump();
                coolDown();
                if (Input.GetKey(KeyCode.Space))
                {
                    if(doublejump) {
                        doubleJump();
                    }
 
                }
            }
                


    }
    private void jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed * 2);
        grounded = false;
       

    }
    private void doubleJump()
    {
        body.velocity = new Vector4(body.velocity.x, speed * 2);
        doublejump = false;
        tr.emitting = true;
        


    }

    private IEnumerator coolDown()
    {
        isAvaliable = true;
        yield return new WaitForSeconds(jumpCool);
        isAvaliable = false;
    }   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        { grounded = true;
            doublejump = true; 
        }
    }
    
    private IEnumerator Dash()
    {
        canDash = false;
        isDash = true;
        float OriginalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        body.gravityScale = OriginalGravity;
        isDash = false;
        yield return new WaitForSeconds(dashCool);
        canDash = true;
    }
}
    