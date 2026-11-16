using UnityEngine;

[CreateAssetMenu(fileName = "BullStats", menuName = "Game/Bull Stats")]
public class BullStats : ScriptableObject
{
    public float viewRange = 5f;
    public float fov = 90f;
    public float sightInterval = 0.1f;

    public float windup = 0.7f; 
    public float dashSpeed = 7.5f;
    public float maxDashDistance = 7f;
    public float wallCheck = 0.9f; 
    public float stun = 1f;
    public float hitRadius = 0.8f;
    public int damage = 1;

    public float returnSpeed = 1.6f;
}
