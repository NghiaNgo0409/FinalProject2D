using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public static PlayerBullet instance;

    public int damage = 3;
    public float speed = 100.0f;
    private Rigidbody2D bulletRb;
    public ParticleSystem killEffect;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        if (PlayerController.instance.isFacingRight)
        {
            bulletRb.velocity = new Vector2(speed, 0);
        }
        else
        {
            bulletRb.velocity = new Vector2(-speed, 0);
        }

        if (Item.instance.isSpeacial == true)
        {
            damage = 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Instantiate(killEffect, collision.transform.position, Quaternion.identity);
            collision.transform.parent.gameObject.SetActive(false);
            AudioManager.Instance.CaseSoundSFX("hiting");
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.tag == "BOSS")
        {
            Boss.instance.health -= damage;
            Boss.instance.enemyAnim.SetTrigger("Hit");
            AudioManager.Instance.CaseSoundSFX("hiting");
            Destroy(gameObject);
        }

        Destroy(gameObject, 6f);
    }
}
