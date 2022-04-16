using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public static Boss instance; 

    [SerializeField] BossData bossData;

    [Header("For Jump Attacking")]
    [SerializeField] float jumpHeight;
    [SerializeField] Transform player;
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 boxSize;
    [SerializeField] LayerMask groundLayer;
    public bool isGrounded;
    private bool isFacingRight = false;
    [Header("Seeing Player")]
    [SerializeField] Vector2 lineOfSight;
    [SerializeField] LayerMask playerLayer;
    public bool canSeePlayer;

    private Rigidbody2D enemyRb;
    public Animator enemyAnim;
    public ParticleSystem killEffect;

    [Header("Info")]
    public int health;

    [Header("UI")]
    public Slider healthBar;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetupData(bossData);
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        healthBar.value = health; 
        if (health <= 0)
        {
            Instantiate(killEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            GameManager.instance.Win();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = false;
        if (Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer))
        {
            isGrounded = true;
        }
        canSeePlayer = false;
        if (Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer))
        {
            canSeePlayer = true;
        }
        AnimationController();
        if (!canSeePlayer && isGrounded)
        {
            enemyRb.velocity = Vector2.zero;
        }
    }

    void SetupData(BossData data)
    {
        health = data.health;
        lineOfSight = data.lineOfSight;
        jumpHeight = data.jumpHeight;
    }

    void AnimationController()
    {
        enemyAnim.SetBool("canSeePlayer", canSeePlayer);
        enemyAnim.SetBool("isGrounded", isGrounded);
    }
    void Flip()
    {
        float playerPos = player.position.x - transform.position.x;
        if (playerPos < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (playerPos > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    void JumpAttacking()
    {
        float distancePlayer = player.position.x - transform.position.x;

        if(isGrounded)
        {
            enemyRb.AddForce(new Vector2(distancePlayer, jumpHeight), ForceMode2D.Impulse);
            enemyAnim.SetFloat("yVelocity", enemyRb.velocity.y);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, lineOfSight);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(groundCheck.position, boxSize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.instance.knockBackCounter = PlayerController.instance.startKnockBackCounter;
            LivesManager.instance.TakeLives();
            if (transform.position.x > collision.transform.position.x)
            {
                PlayerController.instance.isKnockBackFromRight = true;
            }
            else if (transform.position.x < collision.transform.position.x)
            {
                PlayerController.instance.isKnockBackFromRight = false;
            }
        }
    }
}
