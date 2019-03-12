using System.Collections;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public bool facingRight = true;

    public float jumpForce = 250.0f;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public bool grounded = false;
    public LayerMask whatIsGround;

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

        // Character jump and checks if touching the ground
        bool jump = Input.GetButtonDown("Jump");
        if (jump && grounded)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce);
        }

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

    // Method to flip the character
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }
}
