using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolState
{
    NotUse,
    Using,
    AutoDestroy
}
public class PoolEntity : MonoBehaviour
{
    public PoolState State = PoolState.NotUse;
    public float waitForDestroy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (State == PoolState.AutoDestroy)
        {
            waitForDestroy -= Time.deltaTime;
            if (waitForDestroy < 0.0f)
            {
                ReturnToPool();
            }
        }
    }

    public void ReturnToPool()
    {
        State = PoolState.NotUse;
        gameObject.SetActive(false);
    }
}
