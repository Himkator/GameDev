using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int hp = 3;
    public int maxHp = 3;
    public Image[] hearts;
    public AudioClip damageSound;
    public AudioClip healSound;
    private AudioSource audioSource;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        UpdateHearts();
    }

    bool invincible;
    SpriteRenderer sr;

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < 10; i++) 
        {
            sr.enabled = !sr.enabled; 
            yield return new WaitForSeconds(0.2f); 
        }
        sr.enabled = true;
        invincible = false;
    }

    public void TakeDamage(int dmg)
    {
        if (invincible) return;
        hp -= dmg;
        UpdateHearts();
        StartCoroutine(Invincibility());
        if(audioSource != null && damageSound != null){
            audioSource.PlayOneShot(damageSound);
        }
        if (hp <= 0) GameOver();
    }

    public void Heal(int amount) { 
        if (hp >= maxHp) return;
        hp += amount; 
        UpdateHearts();
        if(audioSource != null && healSound != null){
            audioSource.PlayOneShot(healSound);
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = i < hp ? Color.white : new Color(1, 1, 1, 0.2f);
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
