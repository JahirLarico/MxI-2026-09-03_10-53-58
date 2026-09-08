using System;
using System.Threading;
using UnityEngine;

public class Isaac_controller : MonoBehaviour
{

    private Rigidbody2D rb;
    public int speed = 5;

    private bool spriteRight = true;
    private float HorizontalMove;
    private float VerticalMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        float HorizontalMove = Input.GetAxis("Horizontal");
        float VerticalMove = Input.GetAxis("Vertical");
        rb.linearVelocity = new Vector2(HorizontalMove * speed, rb.linearVelocity.y);
    }
    // Update is called once per frame
    void Update()
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        if ((HorizontalMove < 0.0f && spriteRight) || (HorizontalMove > 0.0f && !spriteRight)) {
            FlipIsaac();
        }
    }

    void FlipIsaac() {
        spriteRight = !spriteRight;;
        Vector2 isaccScale = gameObject.transform.localScale;
        isaccScale.x *= -1;
        transform.localScale = isaccScale;
    }
}
