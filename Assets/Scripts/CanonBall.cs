using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public CanonStats canonStats;
    private float lifetime;
    [SerializeField] GameObject impactEffect;

    int damage;
    LayerMask obstacleMask;

    public void Init(Vector2 direction, float speed, int dmg, LayerMask obstacles)
    {
        damage = dmg;
        obstacleMask = obstacles;
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.linearVelocity = direction.normalized * speed;
        lifetime = canonStats.lifetime;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<Health>() is Health target)
        {
            target.TakeDamage(damage);
            Remove();
        }
        else if (((1 << other.gameObject.layer) & obstacleMask) != 0)
        {
            Remove();
        }
    }

    void Remove()
    {
        if (impactEffect) Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}