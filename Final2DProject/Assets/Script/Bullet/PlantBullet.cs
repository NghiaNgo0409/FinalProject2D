using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    private Rigidbody2D bulletRb;

    public float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        if(EnemyFollow.instance.isFacingRight)
        {
            bulletRb.velocity = new Vector2(speed, 0);
        }
        else
        {
            bulletRb.velocity = new Vector2(-speed, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LivesManager.instance.TakeLives();
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
