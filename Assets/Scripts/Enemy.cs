using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Hit by bullet");
            animator.SetBool("IsDead", true);
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<MovingPlatform>().isActive = false;
            Destroy(other.gameObject);
            Destroy(gameObject, 1.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
            c.gameObject.GetComponent<Health>().TakeDamage(1);
    }
}
