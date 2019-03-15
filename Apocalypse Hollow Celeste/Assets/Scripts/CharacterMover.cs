using System.Collections;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public bool facingRight = true;

    public float jumpForce = 10.0f;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public bool grounded = false;
    public bool doubleJump = false;
    public LayerMask whatIsGround;

    public bool wallSlide;
    public Transform wallCheckPoint;
    public bool wallCheck;
    public LayerMask wallLayerMask;

    private void Update()
    {
        // Check for wall sliding
        if (!grounded)
        {
            wallCheck = Physics2D.OverlapCircle(wallCheckPoint.position, 0.1f, wallLayerMask);
            if (facingRight && Input.GetAxis("Horizontal") > 0.1f || !facingRight && Input.GetAxis("Horizontal") < 0.1f)
            {
                if (wallCheck)
                {
                    grounded = true;
                    doubleJump = false;
                    HandleWallSliding();
                }
            }
        }
       // else
        //{
        // Character jump and checks if touching the ground
        bool jump = Input.GetButtonDown("Jump");
        if (jump && (grounded || !doubleJump))
        {
            if (!doubleJump && !grounded)
            {
                Debug.Log("Double Jump!");
                doubleJump = true;
                Vector2 v = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(v.x, 0);
            }
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
        //}
        if (wallCheck == false || grounded)
        {
            wallSlide = false;
        }
    }

    // Wall sliding method
    private void HandleWallSliding()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, -4f);

        wallSlide = true;

        if (Input.GetButtonDown("Jump"))
        {
            if (facingRight)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector3(-1, 4) * jumpForce);
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector3(1, 4) * jumpForce);
            }
        }

    }

    private void FixedUpdate()
    {
        // Character move right or left
        float move = Input.GetAxis("Horizontal");
        if (move < 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(move * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (move > 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(move * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }

        // If touching ground
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        // Double jump
        if (grounded)
        {
            doubleJump = false;
        }

        //// Character jump and checks if touching the ground
        //bool jump = Input.GetButtonDown("Jump");
        //if (jump && (grounded || !doubleJump))
        //{
        //    GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        //    if (!doubleJump && !grounded)
        //    {
        //        Debug.Log("Double Jump!");
        //        doubleJump = true;
        //    }
        //}

        // Flip Character by calling Flip() function, does this when right or left key is pressed
        if (move < 0 && facingRight)
        {
            Flip();
        }
        if (move > 0 && !facingRight)
        {
            Flip();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "MovingPlatform")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }

    // Method to flip the character
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }
}
