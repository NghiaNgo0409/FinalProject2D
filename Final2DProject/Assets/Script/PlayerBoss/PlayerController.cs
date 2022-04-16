using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public enum PowerUpTypes
{
    Basic, Titan, Gun
}
public class PlayerController : MonoBehaviour
{ 
    public static PlayerController instance;

    [Header("Properties")]
    public ParticleSystem dust;
    private Rigidbody2D playerRb;
    private Animator playerAnim;
    [SerializeField] PlayerData playerData;

    [Header("Unity Variables")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask groundCheckLayer;
    [SerializeField] private Transform wallCheckPos;
    [SerializeField] private LayerMask wallCheckLayer;
    private Vector2 desiredVector;

    [Header("Basic variables and Serialize")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 100.0f;
    [SerializeField] private float slideSpeed = 0.5f;
    [SerializeField] private float horizontalInput;
    [SerializeField] private bool isGrounded;
    [SerializeField] private int totalJumps;

    private float groundCheckRadius = 0.8f;
    private float wallCheckRadius = 0.1f;
    public bool isSliding;
    public float availableJumps;
    public bool multipleJumps;
    public bool isFacingRight = true;
    private bool coyoteJump;

    [Header("Knockback")]
    public float knockBack;
    public float knockBackCounter;
    public float startKnockBackCounter;
    public bool isKnockBackFromRight;

    [Header("PowerUp")]
    public PowerUpTypes type = PowerUpTypes.Basic;
    public bool isTitan;
    public ParticleSystem killEffect;
    public bool isShooting;

    private void Awake()
    {
        availableJumps = totalJumps;
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        SetupData(playerData);
        
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        MoveInput();
        WallCheck();
    }

    private void FixedUpdate()
    {
        Move(horizontalInput);
        CheckGround();
    }

    void SetupData(PlayerData data)
    {
        speed = data.speed;
        jumpForce = data.jumpForce;
        totalJumps = data.totalJumps;
        slideSpeed = data.slideSpeed;
    }

    void CheckGround()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        Collider2D collider = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundCheckLayer);
        if (collider)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJumps = false;
            }
            if (collider.tag == "Platform")
            {
                transform.parent = collider.transform;
            }
        }
        else
        {
            transform.parent = null;
            if (wasGrounded) //at the moment fall from the ground , turn on coyote
            {
                StartCoroutine("CoyoteJump");
            }
        }
        playerAnim.SetBool("isJump", !isGrounded);
    }

    void WallCheck()
    {
        if (Physics2D.OverlapCircle(wallCheckPos.position, wallCheckRadius, wallCheckLayer) && Mathf.Abs(horizontalInput) > 0 && !isGrounded && playerRb.velocity.y < 0)
        {
            if (!isSliding)
            {
                availableJumps = totalJumps;
                multipleJumps = false;
            }
            Vector2 slide = playerRb.velocity;
            slide.y = -slideSpeed;
            playerRb.velocity = slide;
            isSliding = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                multipleJumps = true;
                availableJumps--;
                playerRb.velocity = Vector2.up * jumpForce;
                playerAnim.Play("WallJump");
            }
        }
        else
        {
            isSliding = false;
        }
    }

    IEnumerator CoyoteJump()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.3f);
        coyoteJump = false;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                multipleJumps = true;
                availableJumps--;
                playerRb.velocity = Vector2.up * jumpForce;
                CreateDust();
                playerAnim.SetBool("isJump", true);
                AudioManager.Instance.CaseSoundSFX("flying");
            }
            else
            {
                if (coyoteJump)
                {
                    playerRb.velocity = Vector2.up * jumpForce;
                    CreateDust();
                    AudioManager.Instance.CaseSoundSFX("flying");
                    playerAnim.SetBool("isJump", true);
                }
                if (multipleJumps && availableJumps > 0)
                {
                    availableJumps--;
                    playerRb.velocity = Vector2.up * jumpForce;
                    AudioManager.Instance.CaseSoundSFX("flying");
                    playerAnim.Play("DoubleJump");
                }
            }
        }    
    }    

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        CreateDust();
    }

    void MoveInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); //CrossPlatformInputManager to be button
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Move(float dir)
    {
        #region Jump
        playerAnim.SetFloat("yVelocity", playerRb.velocity.y);
        #endregion

        #region Movement
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        
        if(knockBackCounter <= 0.0f)
        {
            desiredVector = new Vector2(xVal, playerRb.velocity.y);
            playerRb.velocity = desiredVector;
        }
        else
        {
            if (isKnockBackFromRight)
            {
                playerRb.velocity = new Vector2(-knockBack, playerRb.velocity.y);
            }
            else if (!isKnockBackFromRight)
            {
                playerRb.velocity = new Vector2(knockBack, playerRb.velocity.y);
            }
            knockBackCounter -= Time.deltaTime;
        }
        playerAnim.SetFloat("xVelocity", Mathf.Abs(desiredVector.x));
        #endregion
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            GameManager.instance.GameOver();
        }

        if (collision.gameObject.tag == "Enemy" && isTitan)
        {
            Instantiate(killEffect, collision.transform.position, Quaternion.identity);
            collision.transform.parent.gameObject.SetActive(false);
        }
    }

    void CreateDust()
    {
        if(isGrounded)
        dust.Play();
    }
}
