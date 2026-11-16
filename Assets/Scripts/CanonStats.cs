using UnityEngine;

[CreateAssetMenu(fileName = "CanonStats", menuName = "Game/Canon Stats")]
public class CanonStats : ScriptableObject
{
    public float lifetime = 5f;
    public float viewRange = 7f;
    public float fov = 90f;
    public float sightInterval = 0.1f;
    public float aimTime = 0.6f; 
    public float fireTime = 0.15f; 
    public float cooldown = 1.5f;
    public float shotSpeed = 3.75f;
    public int damage = 1;
    public float warningWidth = 0.1f;
}

