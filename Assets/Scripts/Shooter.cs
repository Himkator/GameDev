using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown = 0.3f;
    public AudioClip shootSound;
    float nextShot;

    private SpriteRenderer sr;
    private AudioSource audioSource;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // void Update()
    // {
    //     if (Input.GetKey(KeyCode.X) && Time.time >= nextShot)
    //     {
    //         nextShot = Time.time + cooldown;
    //         Quaternion rot = sr.flipX ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
    //         Instantiate(bulletPrefab, transform.position, rot);
    //         if(audioSource != null && shootSound != null){
    //             audioSource.PlayOneShot(shootSound);
    //         }
    //     }
    // }

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
