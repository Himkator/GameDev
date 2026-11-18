using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;
using UnityEngine.UI;


public class Player2D : MonoBehaviour
{
    public PlayerStats playerStats;
    public BossStats bossStats;
    public TextMeshProUGUI coinText;
    public GameObject winPanel;
    public AudioClip audioClip;
    public Animator animator;
    public Animator portalAnimator;
    public GameObject portal;
    public GameObject settingsPanel;

    private bool onGround;
    private int coins;
    private int coinsTotal;
    private AudioSource audioSource;
    private SpriteRenderer sr;
    private float speed;
    private float jumpForce;
    private bool coinsConfigured;
    private StarTimer starTimer;
    private bool levelSaved;



    private Rigidbody2D rb;

    void Start()
    {
        if (!coinsConfigured)
        {
            coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
        }

        if (coinText != null)
        {
            coinText.text = "Coins: " + coins + " / " + coinsTotal;
        }
        audioSource = GetComponent<AudioSource>();
        speed = playerStats.speed;
        jumpForce = playerStats.jumpForce;
        starTimer = GetComponent<StarTimer>();
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
                portalAnimator.SetTrigger("OpenPortal");
                portal.SetActive(true);
            }
        }

        if (other.CompareTag("Fire"))
        {
            gameObject.GetComponent<Health>().TakeDamage(playerStats.fireDamage);
        }

        if (other.CompareTag("Exit"))
        {
            Debug.Log(coins + " / " + coinsTotal);
            if (coins >= coinsTotal && !levelSaved)
            {
                levelSaved = true;

                LevelData level = LevelLoader.CurrentLevel;

                int earnedStars = starTimer.FinishLevel();

                SaveManager.Instance.CompleteLevel(
                    level.levelNumber,
                    earnedStars,
                    starTimer.ElapsedTime
                );  

                winPanel.SetActive(true);
            }
            else
                Debug.Log("Collect all coins first! " + coins + " / " + coinsTotal);
        }

        if (other.CompareTag("BossBullet"))
        {
            gameObject.GetComponent<Health>().TakeDamage(bossStats.damage);
        }
    }

    public void clickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    public void nextLevel()
    {
        int nextIndex = GameSession.levelIndex + 1;

        if (nextIndex >= GameSession.levelCount)
        {
            Debug.Log("All levels completed!");
            Time.timeScale = 1f;
            SceneManager.LoadScene("WinScene");
            return;
        }

        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager is missing!");
            return;
        }

        if (!SaveManager.Instance.IsLevelUnlocked(nextIndex + 1))
        {
            Debug.LogWarning("Next level is locked!");
            return;
        }

        GameSession.levelIndex = nextIndex;

        Time.timeScale = 1f;

        Debug.Log("Loading Level " + (nextIndex + 1));

        SceneManager.LoadScene("Game");
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
        if (transform.position.y < playerStats.fallPoint)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ConfigureCoins(int required)
    {
        coinsConfigured = true;

        coinsTotal = Mathf.Max(0, required);
        coins = 0;

        if (coinText != null)
        {
            coinText.text = "Coins: 0 / " + coinsTotal;
        }
    }
}
