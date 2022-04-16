using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public GameObject[] virtualCamera;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        EnableCamera(0);
    }
    public void EnableCamera(int id)
    {
        for (int i = 0; i < virtualCamera.Length; i++)
        {
            if (id == i)
            {
                virtualCamera[i].gameObject.SetActive(true);
            }
            else
            {
                virtualCamera[i].gameObject.SetActive(false);
            }
        }
    }
}
