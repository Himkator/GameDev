using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public BossStats bossStats;
    public GameObject bulletPrefab;
    public Animator bossAnimator;
    private float distance;


    void Start()
    {
        distance = bossStats.distance;
        InvokeRepeating(nameof(Shoot), 1f, bossStats.shootInterval);
    }

    void Shoot()
    {
        int bulletCount;
        int hp = GetComponent<Boss>().CurrentHp;

        if (hp > bossStats.minHp)
        {
            bulletCount = bossStats.minBulletCount;
        }
        else
        {
            bulletCount = bossStats.maxBulletCount;
        }

        for (int i = 0; i < bulletCount; i++)
        {
            bossAnimator.SetTrigger("Fire");
            float y = (i - (bulletCount - 1) / 2f) * distance;

            Vector3 position = transform.position
                + Vector3.left * 1.5f
                + Vector3.up * y;

            Instantiate(bulletPrefab, position, Quaternion.Euler(0, 0, 180));
        }
    }
}