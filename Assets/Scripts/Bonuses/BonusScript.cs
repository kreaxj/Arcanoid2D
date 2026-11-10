using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScript : MonoBehaviour
{
    [SerializeField] private float _fallSpeed = 5f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x, -_fallSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Bottom Wall")
        {
            Destroy(gameObject);
        }
    }
}
