using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    Rigidbody2D rb;
    public Attack attack;
    public Animator animator;
    BoxCollider2D coll;
    public Transform enemy;
    string currentState;
    public float moveSpeed = 7;
    public float jumpForce = 10;
    float dirX;
    bool jumpPressed = false;
    public bool isGrounded = false;
    bool crouchPressed = false;
    public bool isCrouched = false;
    bool crouchColl = false;
    public LayerMask layerMask = 0;

    const string PLAYER_IDLE = "Player_idle";
    const string PLAYER_RUNNING = "Player_running";
    const string PLAYER_JUMP = "Player_jump";
    const string PLAYER_FALL = "Player_fall";
    const string PLAYER_CROUCH = "Player_crouch";
    const string PLAYER_CROUCH_RUN = "Player_crouch_run";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //dirX = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(left)) dirX = -1;
        else if (Input.GetKey(right)) dirX = 1;
        else dirX = 0;

        if (Input.GetKeyDown(up) && !attack.isAttacking && !attack.isBlocking)
        {
            jumpPressed = true;
        }

        crouchPressed = Input.GetKey(down) && !attack.isAttacking;
    }

    private void FixedUpdate()
    {
        if (transform.position.x > enemy.position.x) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = Vector3.one;

        var groundHit = Physics2D.Raycast(transform.position, Vector2.down, 2f, layerMask);
        isGrounded = groundHit.collider != null;

        isCrouched = crouchPressed && isGrounded;

        if (isCrouched && !crouchColl)
        {
            coll.size = new Vector2(coll.size.x, coll.size.y / 1.58f);
            transform.position = new Vector3(transform.position.x, -3.285719f);
            crouchColl = true;
        }
        else if (!isCrouched && crouchColl)
        {
            coll.size = new Vector2(coll.size.x, coll.size.y * 1.58f);
            transform.position = new Vector3(transform.position.x, -2.581772f);
            crouchColl = false;
        }

        Move();
    }

    private void Move()
    {
        if (attack.isBlocking)
        {
            rb.velocity = Vector2.zero;
            return;
        }
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

        if (isGrounded && !attack.isAttacking)
        {
            if (isCrouched)
            {
                if (dirX != 0)
                {
                    ChangeAnimationState(PLAYER_CROUCH_RUN);
                }
                else
                {
                    ChangeAnimationState(PLAYER_CROUCH);
                }
            }
            else
            {
                if (dirX != 0)
                {
                    ChangeAnimationState(PLAYER_RUNNING);
                }
                else
                {
                    ChangeAnimationState(PLAYER_IDLE);
                }
            }
        }

        if (jumpPressed && isGrounded)
        {
            vel.y = jumpForce;
            jumpPressed = false;
        }

        if (rb.velocity.y > 0)
        {
            ChangeAnimationState(PLAYER_JUMP);
        }
        else if (rb.velocity.y < 0)
        {
            ChangeAnimationState(PLAYER_FALL);
        }

        rb.velocity = vel;
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}
