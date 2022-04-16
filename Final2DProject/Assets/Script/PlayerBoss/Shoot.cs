using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawn;

    private float shootBtwTime;
    public float startShootBtwTime;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J) && PlayerController.instance.isShooting)
        {
            if (shootBtwTime <= 0.0f)
            {
                if (PlayerController.instance.isFacingRight)
                {
                    Instantiate(bullet, bulletSpawn.position, Quaternion.Euler(0,0,90));
                }
                else
                {
                    Instantiate(bullet, bulletSpawn.position, Quaternion.Euler(0, 0, -90));
                }
                shootBtwTime = startShootBtwTime;
            }
            else
            {
                shootBtwTime -= Time.deltaTime;
            }
        }
    }
}
