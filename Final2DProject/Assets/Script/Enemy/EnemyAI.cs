using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    private Animator enemyAnim;
    public List<Transform> pointList = new List<Transform>();
    private float speed;
    public int nextID;
    public float startWaitTime;
    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        speed = enemyData.speed;
        waitTime = startWaitTime;
        enemyAnim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        MovePatrol();
    }

    public void MovePatrol()
    {
        //Initialize goalpoint
        Transform goalPoint = pointList[nextID];
        //Check the direction of point to flip the enemies
        if(transform.position.x < goalPoint.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (transform.position.x > goalPoint.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //Use MoveTowards to move enemies
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);
        //Update nextID by checking the distance
        if (Vector2.Distance(transform.position, goalPoint.position) < 0.1f)
        {
            if (waitTime <= 0.0f)
            {
                enemyAnim.SetBool("Idle", false);
                if (nextID == pointList.Count - 1)
                {
                    nextID = 0;
                }
                else
                {
                    nextID++;
                }
                waitTime = startWaitTime;
            }
            else
            {
                enemyAnim.SetBool("Idle", true);
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !PlayerController.instance.isTitan)
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
