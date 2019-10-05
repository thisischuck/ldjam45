﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D ground;

    public bool isOnGround = false;
    public int horizontalSpeed = 5;
    public int jumpSpeed = 20;

    private Rigidbody2D body;

    private int gravity = -1;
    private float yspeed = 0;
    private float xspeed = 0;
    private float xdir = 0;

    public GameObject[] hitboxes;

    void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Attack();
        //! This is kinda slow. Needs to be a bit faster. Or i need to tweak the values
        if (body.IsTouching(ground))
        {
            yspeed = 0;
            isOnGround = true;
        }
        else isOnGround = false;

        xdir = Input.GetAxisRaw("Horizontal");

        if (isOnGround)
        {
            xspeed = xdir * horizontalSpeed;
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

    void Attack()
    {
        foreach (var tmp in hitboxes)
        {
            tmp.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            hitboxes[0].SetActive(true);
        }
    }
}
