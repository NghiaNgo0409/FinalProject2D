using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectsType
{
    Player,
    Enemy,
    Item,
    Camera
}
[System.Serializable]
public class PoolPrefab
{
    public string Name;
    public PoolObjectsType Type;
    public GameObject Prefabs;
    public int count;

    [System.NonSerialized]
    public List<GameObject> Objects = new List<GameObject>();
}
public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public PoolPrefab[] poolPrefabs;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach(PoolPrefab pool in poolPrefabs)
        {
            for (int i = 0; i < pool.count; i++)
            {
                GameObject newObject = GameObject.Instantiate(pool.Prefabs);
                newObject.transform.SetParent(transform);
                newObject.SetActive(false);
                newObject.AddComponent<PoolEntity>();
                pool.Objects.Add(newObject);
            }
        }
    }


    public GameObject SpawnObject(PoolObjectsType type, float autoDestroy = -1.0f)
    {
        foreach(PoolPrefab pool in poolPrefabs)
        {
            if (pool.Type == type)
            {
                for (int i = 0; i < pool.count; i++)
                {
                    GameObject obj = pool.Objects[i];
                    PoolEntity poolEntity = obj.GetComponent<PoolEntity>();
                    if (poolEntity.State == PoolState.NotUse)
                    {
                        if (autoDestroy < 0.0f)
                        {
                            poolEntity.State = PoolState.Using;
                        }
                        else
                        {
                            poolEntity.State = PoolState.AutoDestroy;
                            poolEntity.waitForDestroy = autoDestroy;
                        }

                        obj.SetActive(true);
                        return obj;
                    }
                }
            }
        }
        return null;
    }

    public void ReturnAllObject()
    {
        foreach(PoolPrefab pool in poolPrefabs)
        {
            if (pool.Type == PoolObjectsType.Camera)
            {
                return;
            }
            else
            {
                for (int i = 0; i < pool.count; i++)
                {
                    GameObject obj = pool.Objects[i];
                    if (obj.activeSelf == true)
                    {
                        PoolEntity poolEntity = obj.GetComponent<PoolEntity>();

                        poolEntity.ReturnToPool();
                    }
                }
            }           
        }
    }
}
