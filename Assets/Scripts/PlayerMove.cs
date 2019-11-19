using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;

    public bool inputLeft = false;
    public bool inputRight = false;
    public bool inputJump = false;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        UIButtonManager ui = GameObject.FindGameObjectWithTag ("Managers").GetComponent<UIButtonManager>();
        ui.Init();
    }

    void Update()
    {
        //Jump
        if (inputJump && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            inputJump = false;
            anim.SetBool("isJumping", true);
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
    }

    void FixedUpdate()
    {
        Move();
        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 0, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }
    }

    void Move()
    {
        if ((!inputLeft && !inputRight))
            anim.SetBool("isWalking", false);

        else if (inputLeft)
        {
            anim.SetBool("isWalking", true);
            transform.localScale = new Vector3(-1, 1, 1);
            transform.Translate((maxSpeed * Time.deltaTime) * -1, 0, 0);
        }
        else if (inputRight)
        {
            anim.SetBool("isWalking", true);
            transform.localScale = new Vector3(1, 1, 1);
            transform.Translate(maxSpeed * Time.deltaTime, 0.0f, 0.0f);
        }

        //Max Speed
        if (rigid.velocity.x > maxSpeed) //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }
}
