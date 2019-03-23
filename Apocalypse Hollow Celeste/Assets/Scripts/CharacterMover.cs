using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private bool facingRight = true;

    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool doubleJump = false;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private bool wallSlide;
    [SerializeField] private float slideSpeed = -5f;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private bool wallCheck;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private LayerMask slideLayerMask;
    [SerializeField] private float resetSlideTime = 0.5f;
    [SerializeField] private float timeOnWall = 0.5f;

    // Portals
    public bool canTeleport = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "IgnorePortal")
        {
            canTeleport = false;
        }
        else if (collision.gameObject.tag == "PortalTrigger")
        {
            canTeleport = true;
        }
        else if (collision.gameObject.tag == "Cannon")
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce * 4, ForceMode2D.Impulse);
        }

        if (collision.gameObject.tag == "Portal" && canTeleport == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

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
                    timeOnWall -= Time.deltaTime;
                    grounded = true;
                    doubleJump = false;
                    if (timeOnWall <= 0.0f)
                    {
                        HandleWallSliding();
                    }
                }
                else
                {
                    timeOnWall = resetSlideTime;
                }
            }
        }

        // Check for unclimbable wall
        if (!grounded)
        {
            wallCheck = Physics2D.OverlapCircle(wallCheckPoint.position, 0.1f, slideLayerMask);
            if (facingRight && Input.GetAxis("Horizontal") > 0.1f || !facingRight && Input.GetAxis("Horizontal") < 0.1f)
            {
                if (wallCheck)
                {
                    grounded = true;
                    doubleJump = true;
                    HandleSliding();
                }
            }
        }

        // Character jump and checks if touching the ground
        bool jump = Input.GetButtonDown("Jump");
        if (jump && (grounded || !doubleJump || wallCheck))
        {
            if (!doubleJump && !grounded)
            {
                doubleJump = true;
                Vector2 v = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(v.x, 0);
            }
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
        if (wallCheck == false || grounded)
        {
            wallSlide = false;
        }
    }

    // Wall sliding method
    private void HandleWallSliding()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, slideSpeed);

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

    // Sliding from an unclimbable wall
    private void HandleSliding()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, Physics2D.gravity.y);

        wallSlide = true;

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
