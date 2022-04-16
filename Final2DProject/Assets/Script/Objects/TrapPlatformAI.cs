using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatformAI : PlatformAI
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController.instance.knockBackCounter = PlayerController.instance.startKnockBackCounter;
            LivesManager.instance.TakeLives();
            if (transform.position.x > collision.transform.position.x)
            {
                PlayerController.instance.isKnockBackFromRight = true;
            }
            else if (transform.position.x > collision.transform.position.x)
            {
                PlayerController.instance.isKnockBackFromRight = false;
            }
        }
    }
}
