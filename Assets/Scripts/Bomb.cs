using UnityEngine;

public class BombHazard : Hazard
{
    public Animator animator;
    void Start()
    {
        Invoke(nameof(Explode), 2f);
    }

    void Explode()
    {
        animator.SetBool("Exploded", true);
        Destroy(gameObject, 1f);
    }

    protected override void OnHitPlayer(Health p) { p.TakeDamage(2); }
}
