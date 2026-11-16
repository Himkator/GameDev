using UnityEngine;

[CreateAssetMenu(fileName = "SlimeStats", menuName = "Game/Slime Stats")]
public class SlimeStats : ScriptableObject
{
    public float viewRange = 4.5f;
    public float fov = 90f;
    public float sightInterval = 0.1f;

    public float patrolSpeed = 1.4f; 
    public float chaseSpeed = 2.0f; 
    public float leashExtra = 1.5f; 
    public float loseTime = 2f;

    public float attackRange = 0.9f;
    public float attackHeight = 0.8f;
    public float windup = 0.5f; 
    public float recovery = 0.4f;
    public float cooldown = 1.0f;
    public float hitReach = 0.7f;
    public float hitRadius = 0.6f;
    public int damage = 1;
    public int health = 1;
}
