using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    public ParticleSystem killParticleSystem;
    public float bounceForce; //force to make player bounce up when he stomps

    public GameObject melon;

    private Rigidbody2D playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            PlayerController.instance.availableJumps++;
            Instantiate(killParticleSystem, collision.transform.position, Quaternion.identity);
            collision.transform.parent.gameObject.SetActive(false);
            playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
            if (collision.gameObject.name == "Plant")
            {
                Instantiate(melon, collision.transform.position, Quaternion.identity);
            }
            AudioManager.Instance.CaseSoundSFX("hiting");
        }
    }
}
