using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public static EnemyAttack instance;

    [SerializeField] EnemyData enemyData;

    private Transform player;
    private Animator enemyAnim;
    private float fireRange;

    private float shootBtwTime;
    private float startShootBtwTime;
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
            enemyAnim.SetBool("Idle", true);
            GetComponent<EnemyAI>().enabled = false;
            if (transform.position.x > player.transform.position.x)
            {
                isFacingRight = false;
                transform.localScale = new Vector3(1, 1, 1);

            }
            else if (transform.position.x < player.transform.position.x)
            {
                isFacingRight = true;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            transform.position = this.transform.position;
            Fire();
        }
        else
        {
            enemyAnim.SetBool("Idle", false);
            GetComponent<EnemyAI>().enabled = true;
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
            Invoke("SpawnBullet", 0.7f);
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
        if (transform.position.x > player.position.x)
        {
            Instantiate(projectile, gun.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Instantiate(projectile, gun.transform.position, Quaternion.Euler(0, 0, 180));
        }
    }
}
