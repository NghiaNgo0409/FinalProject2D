using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAI : MonoBehaviour
{
    public List<Transform> waypoints;
    public int nextID;
    public int speed;

    public float startWaitTime;
    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        MovePatrol();
    }

    void MovePatrol()
    {
        Transform goalPoint = waypoints[nextID];
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, goalPoint.position) < 0.3f)
        {
            if (waitTime <= 0.0f)
            {
                if (nextID == waypoints.Count - 1)
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
                waitTime -= Time.deltaTime;
            }
        }
    }
}
