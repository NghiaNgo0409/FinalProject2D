using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Config/BossData")]
public class BossData : ScriptableObject
{
    public float jumpHeight;
    public int health;

    public Vector2 lineOfSight;
}
