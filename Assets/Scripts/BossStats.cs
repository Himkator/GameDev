using UnityEngine;

[CreateAssetMenu(fileName = "BossStats", menuName = "Game/Boss Stats")]
public class BossStats : ScriptableObject
{
    public float distance = 0.7f;
    public int minBulletCount = 3;
    public int maxBulletCount = 5;
    public float shootInterval = 3f;
    public int maxHp = 20;
    public int minHp = 10;
    public int damage = 1;
}
