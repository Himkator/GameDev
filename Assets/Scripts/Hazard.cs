using UnityEngine;

public class Hazard : MonoBehaviour
{
    public GameObject warningPrefab;
    void Start()
    {
        Destroy(gameObject, 5f);  
    }  

    void OnCollisionEnter2D(Collision2D c)
    {
        if (!c.gameObject.CompareTag("Player")) return;
        OnHitPlayer(c.gameObject.GetComponent<Health>());
        Destroy(gameObject);
    }

    protected virtual void OnHitPlayer(Health player)
    {
        player.TakeDamage(1);
    }
}
