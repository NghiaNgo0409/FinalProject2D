using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private CinemachineVirtualCamera vcam;
    // Start is called before the first frame update
    void Start()
    {

        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                vcam.LookAt = player.transform;
                vcam.Follow = player.transform;
            }
        }
    }
}
