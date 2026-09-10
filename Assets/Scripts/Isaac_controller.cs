using System;
using System.Threading;
using UnityEngine;

public class Isaac_controller : MonoBehaviour
{

    private Rigidbody2D rb;
    public int speed = 2;
    public int jumpForce = 200;
    private bool spriteRight = true;
    private float HorizontalMove;
    private float VerticalMove;
    public bool inGroud = true;
    public Animator animator;
    public Transform feetPos;
    public float checkRadius = 0.5f;
    public LayerMask whatIsUnder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
    
        float HorizontalMove = Input.GetAxis("Horizontal");
        float VerticalMove = Input.GetAxis("Vertical");
        rb.linearVelocity = new Vector2(HorizontalMove * speed, rb.linearVelocity.y);
        animator.SetFloat("Speed",Math.Abs(HorizontalMove));

        if (Input.GetKey(KeyCode.J))
        {
            animator.SetInteger("Shoot_d", -1);
            animator.SetBool("Shoot", true);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            animator.SetInteger("Shoot_d", 1);
            animator.SetBool("Shoot", true);
        }
        else
        {
            animator.SetInteger("Shoot_d", 0);
            animator.SetBool("Shoot", false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        inGroud = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsUnder);
        Debug.Log( "Estado de piso "+inGroud);
        //Cambio de sprite 
        //if ((HorizontalMove < 0.0f && spriteRight) || (HorizontalMove > 0.0f && !spriteRight)) {
        //    FlipIsaac();
        //}
        if (Input.GetKeyDown(KeyCode.Space) && inGroud) {
            Jump();
        }
    }

    void FlipIsaac() {
        spriteRight = !spriteRight;;
        Vector2 isaccScale = gameObject.transform.localScale;
        isaccScale.x *= -1;
        transform.localScale = isaccScale;
    }

    void Jump() {
        rb.AddForceY(jumpForce);
        inGroud = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("toco el piso");

        if (collision.gameObject.tag == "Ground") {
            inGroud = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("dejo el piso");
        if (collision.gameObject.tag == "Ground")
        {
            inGroud = false;
        }
    }
}
