using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject testButton;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CameraManager.Instance.EnableCamera(1);
            dialogue.SetActive(true);
            testButton.SetActive(true);
        }
    }
}
