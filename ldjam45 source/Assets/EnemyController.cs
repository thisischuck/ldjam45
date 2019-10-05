using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //! Code Enemy Behaviours. And put randomness to them.
    //- different speeds can be a start

    public enum Type
    {
        Normal, //1 hit 
        Boss,  //2 hits
        Defensive //3 hits
    }

    public Type tp;

    public Collider2D ground;
    public GameObject player;
    public GameScriptableObject g;

    public bool isOnGround = false;
    public int horizontalSpeed = 5;
    public int jumpSpeed = 20;
    public bool low_hit;
    public int hp;

    private Rigidbody2D body;

    private int gravity = -1;
    private float yspeed = 0;
    private float xspeed = 0;
    private float xdir = 0;


    private string hurt_tag;

    private int fly = 0;


    void Start()
    {
        player = GameObject.Find("Player");
        ground = GameObject.Find("Ground").GetComponent<Collider2D>();


        body = this.GetComponent<Rigidbody2D>();

        ChangeHitBox();

        if (fly != 0)
            xspeed = 30 * fly;

        if (tp == Type.Normal) hp = 1;
        else if (tp == Type.Defensive) hp = 2;
        else hp = 3;

        StartCoroutine("KillMyself");
    }

    void ChangeHitBox()
    {
        low_hit = (Random.Range(0, 4) % 2 == 0) ? false : true;

        if (low_hit)
        {
            hurt_tag = "HitBoxLow";
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else hurt_tag = "HitBoxHigh";
    }


    public void FlyLikeAnAngel(int isLeft)
    {
        fly = isLeft;
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag.Equals(hurt_tag))
        {
            //! Either health system or just one shot kill
            //- most likely just one shot kill
            Debug.Log(this + " Hurt");
            hp--;
            ChangeHitBox();
            if (hp == 0)
            {
                Destroy(this.gameObject);
                g.Fame += 1;
                g.enemiesKilled += 1;
            }
        }
    }

    IEnumerator KillMyself()
    {
        yield return new WaitForSecondsRealtime(2f);
        Destroy(this.gameObject);
    }
}
