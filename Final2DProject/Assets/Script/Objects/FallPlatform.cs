using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    Rigidbody2D platformRb;
    // Start is called before the first frame update
    void Start()
    {
        platformRb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FallingPlatformManager.instance.StartCoroutine("SpawnPlatform", new Vector2(transform.position.x, transform.position.y));
            Invoke("DropPlatform", 0.5f);
            Destroy(gameObject, 2f);
        }
    }

    void DropPlatform()
    {
        platformRb.isKinematic = false;
    }
}
