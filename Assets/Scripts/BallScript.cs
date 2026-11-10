using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Vector2 ballInitialForce;
    Rigidbody2D rb;
    GameObject playerObj;
    float deltaX;
    public AudioClip hitSound;
    public AudioClip loseSound;

    public GameDataScript gameData;

    private float _targetSpeed = -1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        deltaX = transform.position.x;
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (rb.isKinematic)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    LaunchBall();
                }
                else
                {
                    var pos = transform.position;
                    pos.x = playerObj.transform.position.x + deltaX;
                    transform.position = pos;
                }
            }

            if (!rb.isKinematic && Input.GetKeyDown(KeyCode.J))
            {
                var v = rb.velocity;
                if (Random.Range(0, 2) == 0)
                    v.Set(v.x - 0.1f, v.y + 0.1f);
                else
                    v.Set(v.x + 0.1f, v.y - 0.1f);
                rb.velocity = v;
            }
        }
    }

    public void LaunchBall()
    {
        rb.isKinematic = false;
        rb.AddForce(ballInitialForce);
    }

    void FixedUpdate()
    {
        if (_targetSpeed <= 0f) return;

        if (rb.velocity.magnitude > _targetSpeed)
            rb.velocity = rb.velocity.normalized * _targetSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameData.sound)
            playerObj.GetComponent<PlayerScript>().PlaySound(loseSound, 5f);

        Destroy(gameObject);
        playerObj.GetComponent<PlayerScript>().BallDestroyed();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameData.sound)
            playerObj.GetComponent<PlayerScript>().PlaySound(hitSound, 5f);
    }

    public void ApplySpeed(float multiplier)
    {
        if (rb == null || rb.isKinematic) return;

        if (multiplier > 1f)
        {
            rb.velocity *= multiplier;

            if (_targetSpeed > 0f) _targetSpeed *= multiplier;
        }
        else
        {
            float baseSpeed = _targetSpeed > 0f ? _targetSpeed : rb.velocity.magnitude;
            if (baseSpeed <= 0f) return;
            _targetSpeed = baseSpeed * multiplier;
        }
    }
}