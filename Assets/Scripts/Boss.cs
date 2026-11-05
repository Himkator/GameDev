using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHp = 20;
    public int hp;
    public BossHealthBar bar;
    public Animator portalAnimator;
    public Animator animator;
    public GameObject portal;

    void Start()
    {
        hp = maxHp;
        bar.SetValue(1f);
        Debug.Log("Boss HP: " + hp);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(2);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        hp = Mathf.Max(hp - dmg, 0);
        bar.SetValue((float)hp / maxHp);

        if (hp <= 0) Die();
    }

    void Die()
    {
        portal.SetActive(true);
        animator.SetBool("isDead", true);
        portalAnimator.SetBool("bossDead", true);
        bar.gameObject.SetActive(false);
        Destroy(gameObject, 0.6f);
    }
}
