using UnityEngine;

public class Shooter : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject bulletPrefab;
    public AudioClip shootSound;
    float nextShot;

    private SpriteRenderer sr;
    private AudioSource audioSource;
    private float cooldown;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        cooldown = playerStats.cooldown;
    }

    public void Shoot()
    {
        if (Time.time < nextShot) return;
        nextShot = Time.time + cooldown;
        Quaternion rot = sr.flipX ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
        Instantiate(bulletPrefab, transform.position, rot);
        if(audioSource != null && shootSound != null){
            audioSource.PlayOneShot(shootSound);
        }
    }
}
