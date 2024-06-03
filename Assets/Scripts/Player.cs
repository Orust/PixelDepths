using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the Player game object.");
        }
    }

    void Update()
    {
        Move();
        Jump();
        UpdateAnimation();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(moveX * moveSpeed, rb.velocity.y);
        rb.velocity = move;

        // アニメーションの設定
        if (animator != null)
        {
            animator.SetBool("isRunning", moveX != 0);
        }

        // 左右反転の設定
        if (moveX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveX < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            if (isGrounded)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
            }
            else if (rb.velocity.y < 0)
            {
                animator.SetBool("isFalling", true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
