using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D ground;
    public GameObject player;

    public bool isOnGround = false;
    public int horizontalSpeed = 5;
    public int jumpSpeed = 20;

    private Rigidbody2D body;

    private int gravity = -1;
    private float yspeed = 0;
    private float xspeed = 0;
    private float xdir = 0;

    void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (body.IsTouching(ground))
        {
            yspeed = 0;
            isOnGround = true;
        }
        else isOnGround = false;

        Vector2 dir = player.transform.position - this.transform.position;

        xdir = dir.normalized.x;

        if (isOnGround)
        {
            xspeed = xdir * horizontalSpeed;
            if (dir.normalized.y > 0.7f)
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
