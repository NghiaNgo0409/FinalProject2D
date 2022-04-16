using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public static EnemyFollow instance;

    [SerializeField] EnemyData enemyData;

    private Transform player;
    private Animator enemyAnim;
    private float fireRange;

    private float shootBtwTime;
    float startShootBtwTime;
    public bool isFacingRight;

    public GameObject projectile;
    public Transform gun;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetupData(enemyData);
        shootBtwTime = startShootBtwTime;
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Vector2.Distance(transform.position, player.position) <= fireRange)
        {
            if (transform.position.x < player.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isFacingRight = true;
            }
            else if (transform.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isFacingRight = false;
            }
            Fire();
        }
    }

    void SetupData(EnemyData data)
    {
        fireRange = data.fireRange;
        startShootBtwTime = data.startTime;
    }

    void Fire()
    {
        if (shootBtwTime <= 0.0f)
        {
            enemyAnim.SetBool("Attack", true);
            Invoke("SpawnBullet", 0.4f);
            shootBtwTime = startShootBtwTime;
        }
        else
        {
            enemyAnim.SetBool("Attack", false);
            shootBtwTime -= Time.deltaTime;
        }
    }

    void SpawnBullet()
    {
        Instantiate(projectile, gun.transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
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
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, fireRange);
    //}
}
