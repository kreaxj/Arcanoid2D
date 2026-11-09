using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlockScript : BlockScript
{
    public float moveSpeed = 2f;
    private Vector2 moveDirection = Vector2.right;
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Block"))
        {
            moveDirection = -moveDirection;
        }
        else if (collision.gameObject.CompareTag("Ball"))
        {
            base.OnCollisionEnter2D(collision);
        }
    }
}