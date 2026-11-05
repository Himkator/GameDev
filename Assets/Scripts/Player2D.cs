using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;
using UnityEngine.UI;


public class Player2D : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 12f;
    public TextMeshProUGUI coinText;
    public GameObject winPanel;
    public AudioClip audioClip;
    public int health = 3;
    public Animator animator;
    public Animator portalAnimator;
    public GameObject portal;
    public GameObject settingsPanel;
    public bool AllCoinCollected = false;

    private bool onGround;
    private int coins;
    private int coinsTotal;
    private AudioSource audioSource;
    private SpriteRenderer sr;


    private Rigidbody2D rb;

    void Start()
    {
        if (coinText != null)
        {
            coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
            coinText.text = "Coins: " + coins + " / " + coinsTotal;
        }
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            if(audioSource != null && audioClip != null){
                audioSource.PlayOneShot(audioClip);
            }
            if (coinText != null)
            {
                coinText.text = "Coins: " + coins + " / " + coinsTotal;
            }
            Debug.Log("Coins: " + coins + " / " + coinsTotal);

            if (coins >= coinsTotal){
                AllCoinCollected = true;
                portal.SetActive(true);
                portalAnimator.SetBool("AllCoinCollected", true);
            }
        }

        if (other.CompareTag("Fire"))
        {
            gameObject.GetComponent<Health>().TakeDamage(1);
        }

        if (other.CompareTag("Exit"))
        {
            if (coins >= coinsTotal)
            {
                Debug.Log("You win!");
                winPanel.SetActive(true);
            }
            else
                Debug.Log("Collect all coins first! " + coins + " / " + coinsTotal);
        }

        if (other.CompareTag("BossBullet"))
        {
            gameObject.GetComponent<Health>().TakeDamage(1);
        }
    }

    public void clickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void nextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels available.");
        }
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void clickSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void closeSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.y < 180)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
