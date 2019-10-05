using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D body;
    public Collider2D ground;
    private int horizontalSpeed = 5;
    private int jumpSpeed = 20;
    private int gravity = -1;

    private float yspeed = 0;
    private float xspeed = 0;
    private float xdir = 0;

    public bool isOnGround = false;
    void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (body.IsTouching(ground))
        {
            isOnGround = true;
        }
        else isOnGround = false;

        xdir = Input.GetAxisRaw("Horizontal");

        if (isOnGround)
        {
            xspeed = xdir * horizontalSpeed;
            yspeed = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                yspeed += jumpSpeed;
                isOnGround = false;
            }
        }
        else
        {
            yspeed += gravity;
            xspeed += xdir * 0.1f;
        }

        body.velocity = new Vector2(xspeed, yspeed);
    }
}
