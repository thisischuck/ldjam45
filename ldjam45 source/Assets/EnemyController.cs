using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //! Code Enemy Behaviours. And put randomness to them.
    //- different speeds can be a start
    public Collider2D ground;
    public GameObject player;
    public GameScriptableObject g;

    public GameObject[] hitboxes;

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

    private bool isAttacking, hit;

    private string hurt_tag;

    private int fly = 0;


    void Start()
    {
        player = GameObject.Find("Player");
        ground = GameObject.Find("Ground").GetComponent<Collider2D>();


        body = this.GetComponent<Rigidbody2D>();

        ChangeHitBox();

        if (fly != 0)
        {
            xspeed = 30 * fly;
            yspeed = 5;
        }
    }

    void ChangeHitBox()
    {
        low_hit = (Random.Range(0, 4) % 2 == 0) ? false : true;

        if (low_hit)
        {
            hurt_tag = "HitBoxLow";
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            hurt_tag = "HitBoxHigh";
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }


    public void FlyLikeAnAngel(int isLeft)
    {
        fly = isLeft;
    }

    void Update()
    {
        if (!isAttacking)
            Attack();
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

        if (hit)
        {
            xspeed = -xdir * 7;
            yspeed = -gravity * 1.5f;
        }

        body.velocity = new Vector2(xspeed, yspeed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag.Equals(hurt_tag))
        {
            //! Either health system or just one shot kill
            //- most likely just one shot kill
            if (!hit)
            {
                hp--;
                StartCoroutine("HitStagger");
            }
            ChangeHitBox();
            if (hp == 0)
            {
                Destroy(this.gameObject);
                g.Fame += 1;
                g.enemiesKilled += 1;
            }
        }
    }

    void Attack()
    {
        var distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance < 2)
        {
            StartCoroutine("AttackCooldown");
            StartCoroutine("AttackTimer");
            hitboxes[0].SetActive(true);
            hitboxes[1].SetActive(true);
        }

    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSecondsRealtime(1f);
        isAttacking = false;
    }
    IEnumerator AttackTimer()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        hitboxes[0].SetActive(false);
        hitboxes[1].SetActive(false);
    }

    IEnumerator HitStagger()
    {
        hit = true;
        yield return new WaitForSecondsRealtime(0.25f);
        hit = false;
    }
}
