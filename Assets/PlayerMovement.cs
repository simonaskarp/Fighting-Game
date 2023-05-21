using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 7;
    public float jumpForce = 10;
    float dirX;
    bool jumpPressed = false;
    public bool isGrounded = false;
    public LayerMask layerMask = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        var groundHit = Physics2D.Raycast(transform.position, Vector2.down, 2f, layerMask);
        isGrounded = groundHit.collider != null;

        Move();
    }

    private void Move()
    {
        Vector2 vel = new Vector2(0, rb.velocity.y);

        if (dirX < 0)
        {
            vel.x = -moveSpeed;
        }
        else if (dirX > 0)
        {
            vel.x = moveSpeed;
        }
        else
        {
            vel.x = 0;
        }

        if (jumpPressed && isGrounded)
        {
            vel.y = jumpForce;
            jumpPressed = false;
        }

        rb.velocity = vel;
    }
}
