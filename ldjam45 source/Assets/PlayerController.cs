using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D ground;
    public GameScriptableObject gm;

    public bool isOnGround = false;
    public int horizontalSpeed = 5;
    public int jumpSpeed = 20;
    public int hp = 3;

    public int invulTime = 10; //maybe frames

    private Rigidbody2D body;

    private int gravity = -1;
    private float yspeed = 0;
    private float xspeed = 0;
    private float xdir = 0;

    private bool isFlipped = false;
    private bool isAttacking = false;
    private bool isInvul = false;

    public GameObject[] hitboxes;
    public GameObject[] a;

    void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (gm.isKnockedDown)
        {
            CheckMash();
            return;
        }

        if (isInvul)
            GetComponent<SpriteRenderer>().color = Color.grey;
        else
            GetComponent<SpriteRenderer>().color = Color.blue;


        if (body.IsTouching(ground))
        {
            yspeed = 0;
            isOnGround = true;
        }
        else isOnGround = false;

        if (xdir < 0)
            isFlipped = true;
        else if (xdir > 0)
            isFlipped = false;


        if (!isAttacking)
            Attack();
    }

    void FixedUpdate()
    {
        //! This is kinda slow. Needs to be a bit faster. Or i need to tweak the values

        if (gm.isKnockedDown)
        {
            body.velocity = Vector2.zero;
            return;
        }

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
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine("AttackCooldown");
            if (!isFlipped)
                hitboxes[0].SetActive(true);
            else hitboxes[2].SetActive(true);

            return;
        }
        else
        {
            hitboxes[0].SetActive(false);
            hitboxes[2].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine("AttackCooldown");
            if (!isFlipped)
                hitboxes[1].SetActive(true);
            else hitboxes[3].SetActive(true);

            return;
        }
        else
        {
            hitboxes[1].SetActive(false);
            hitboxes[3].SetActive(false);
        }

        return;
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSecondsRealtime(0.2f);
        isAttacking = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag.Equals("HitBoxEnemy"))
        {
            //! Either health system or just one shot kill
            //- most likely just one shot kill
            if (!isInvul)
            {
                hp--;
                StartCoroutine("Invulnerability");
            }
            if (hp == 0)
            {
                Debug.Log("KnockedDown");
                gm.isKnockedDown = true;
                gm.count = gm.MainCount;
                //KnockDownMode
            }
        }
    }

    IEnumerator Invulnerability()
    {
        isInvul = true;
        yield return new WaitForSecondsRealtime(invulTime);
        isInvul = false;
    }

    int keyPressLoss = 3;

    bool cStarted;
    IEnumerator Countdown()
    {
        cStarted = true;
        yield return new WaitForSecondsRealtime(1);
        gm.count--;
        gm.keyPresses -= keyPressLoss;
        if (gm.keyPresses < 0) gm.keyPresses = 0;
        cStarted = false;
    }

    int getUpThreshold = 40;
    void CheckMash()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)) gm.keyPresses++;

        if (gm.count <= 0)
        {
            if (gm.keyPresses >= getUpThreshold)
            {
                //you win and can keep playing
                Debug.Log("You Win");
                hp = 3;
                gm.MainCount--;
                keyPressLoss += 1;
                gm.keyPresses = 0;
                gm.restarted = true;
                gm.isKnockedDown = false;
                return;
            }
            else
            {
                Debug.Log("You Lose");
                SceneManager.LoadScene(0);
            }
        }
        else if (!cStarted) StartCoroutine("Countdown");
    }
}
