using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float speed = 7f;
    public float jumpForce = 12f;
    public int hp = 3;
    public int maxHp = 3;
    public int fireDamage = 1;
    public int gunDamage = 1;
    public float cooldown = 0.3f;
    public float fallPoint = 180f;
}
