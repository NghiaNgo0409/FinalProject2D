using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformManager : MonoBehaviour
{
    public static FallingPlatformManager instance;
    public GameObject platformPrefab;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator SpawnPlatform(Vector2 position)
    {
        yield return new WaitForSeconds(4.0f);
        Instantiate(platformPrefab, position, Quaternion.identity);
    }
}
