using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSound : MonoBehaviour
{
    public AudioSource source;
    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
            Destroy(gameObject);
    }
}
