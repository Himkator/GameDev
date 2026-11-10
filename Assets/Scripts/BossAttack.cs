using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Animator bossAnimator;
    public float distance = 0.7f;

    void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, 3f);
    }

    void Shoot()
    {
        int bulletCount;
        int hp = GetComponent<Boss>().hp;

        if (hp > 10)
        {
            bulletCount = 3;
        }
        else
        {
            bulletCount = 5;
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