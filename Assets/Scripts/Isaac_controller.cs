using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
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
    private Vector2 originalSize;

    private Vector2 originalSprite;
    private BoxCollider2D box;

    private bool sonidoMuerteReproducido = false;
    public static bool death;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
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
        if (death) {
            int playerLayer = LayerMask.NameToLayer("Player");
            int enemyLayer = LayerMask.NameToLayer("Enemie");

            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
            GetComponent<Collider2D>().isTrigger = true;
            transform.Find("FeetPos").GetComponent<Collider2D>().isTrigger = true;
            rb.gravityScale = 0.0f;
            float visualY = originalSize.y * 1.37f;
            //box.size = new Vector2(originalSize.x, visualY);
            box.offset = new Vector2(originalSprite.x, originalSprite.y - (originalSize.y - visualY) / 2f);

            if (!sonidoMuerteReproducido)
            {
                AudioSource sound = GetComponent<AudioSource>();
                sound.Play();

                sonidoMuerteReproducido = true;
            }
            animator.SetBool("Death", true);
            Invoke("ReloadScene", 1);
            jumpForce = 0;
            speed = 0;

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

    private void ReloadScene() {
        death = false;
        SceneManager.LoadScene("SampleScene");
    }
}
