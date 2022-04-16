using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Config/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int health;
    public float speed;
    public float jumpForce;
    public float slideSpeed;
    public int totalJumps;
}
