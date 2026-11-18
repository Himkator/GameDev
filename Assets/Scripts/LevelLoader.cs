
using UnityEngine;

public static class GameSession
{
    public static int levelIndex = 0;
    public static int levelCount = 0;
}

public class LevelLoader : MonoBehaviour
{
    [Header("Available Levels")]
    [SerializeField] private LevelData[] levels;

    [Header("Level Selection")]
    [SerializeField] private bool useMenuSelection = true;
    [SerializeField] private int testLevelIndex = 0;

    [Header("Scene References")]
    [SerializeField] private Player2D player;
    [SerializeField] private StarTimer starTimer;
    [SerializeField] private Transform contentRoot;

    [Header("Boss UI")]
    [SerializeField] private GameObject bossHealthBarObject;
    [SerializeField] private BossHealthBar bossHealthBar;

    [Header("Stars UI")]
    [SerializeField] private RectTransform starsGroup;

    [SerializeField] private Vector2 normalStarsPosition =
        new Vector2(-4f, 201.3723f);

    [SerializeField] private Vector2 bossStarsPosition =
        new Vector2(-50f, 120f);

    public static LevelData CurrentLevel { get; private set; }

    private void Awake()
    {
        if (levels == null || levels.Length == 0)
        {
            Debug.LogError("No LevelData assets assigned!");
            return;
        }

        starTimer = player.GetComponent<StarTimer>();

        GameSession.levelCount = levels.Length;

        int index = useMenuSelection
            ? GameSession.levelIndex
            : testLevelIndex;

        if (index < 0 || index >= levels.Length)
        {
            Debug.LogWarning("Invalid level index. Using Level 1.");
            index = 0;
        }

        GameSession.levelIndex = index;

        CurrentLevel = levels[index];

        if (CurrentLevel == null)
        {
            Debug.LogError("Selected LevelData is null!");
            return;
        }

        LoadLevel(CurrentLevel);
    }

    private void LoadLevel(LevelData data)
    {
        Debug.Log("Loading: " + data.levelName);

        bool isBossLevel = data.levelType == LevelType.Boss;

        if (bossHealthBarObject != null)
        {
            bossHealthBarObject.SetActive(isBossLevel);
        }


        ConfigureLevelUI(data);

        SpawnLayout(data);
        SpawnCoins(data);
        SpawnEnemies(data);
        SpawnBoss(data);

        ConfigurePlayer(data);
        ConfigureTimer(data);
    }

    private void SpawnLayout(LevelData data)
    {
        if (data.layoutPrefab == null)
        {
            Debug.LogWarning("No layout prefab assigned.");
            return;
        }

        Instantiate(
            data.layoutPrefab,
            data.layoutPrefab.transform.position,
            data.layoutPrefab.transform.rotation,
            contentRoot
        );
    }

    private void SpawnCoins(LevelData data)
    {
        if (data.coinPrefab == null)
        {
            if (data.coinPositions.Length > 0)
                Debug.LogError("Coin prefab is missing!");

            return;
        }

        foreach (Vector3 position in data.coinPositions)
        {
            Instantiate(
                data.coinPrefab,
                position,
                Quaternion.identity,
                contentRoot
            );
        }

        if (data.levelType == LevelType.CollectCoins &&
            data.coinsRequired != data.coinPositions.Length)
        {
            Debug.LogWarning(
                "Coins required does not match spawned coins!"
            );
        }
    }

    private void SpawnEnemies(LevelData data)
    {
        foreach (EnemySpawn enemy in data.enemies)
        {
            if (enemy == null || enemy.enemyPrefab == null)
                continue;

            Instantiate(
                enemy.enemyPrefab,
                enemy.spawnPosition,
                Quaternion.identity,
                contentRoot
            );
        }
    }

    
    private void SpawnBoss(LevelData data)
    {
        if (data.levelType != LevelType.Boss)
            return;

        if (data.boss == null ||
            data.boss.enemyPrefab == null)
        {
            Debug.LogError("Boss prefab is missing!");
            return;
        }

        GameObject bossObject = Instantiate(
            data.boss.enemyPrefab,
            data.boss.spawnPosition,
            Quaternion.identity,
            contentRoot
        );

        Boss bossScript = bossObject.GetComponent<Boss>();

        if (bossScript == null)
        {
            Debug.LogError("Boss script is missing!");
            return;
        }

        bossScript.bar = bossHealthBar;

        bossScript.portal = player.portal;
        bossScript.portalAnimator = player.portalAnimator;

        bossScript.playerStats = player.playerStats;
    }


    private void ConfigurePlayer(LevelData data)
    {
        if (player == null)
        {
            Debug.LogError("Player reference missing!");
            return;
        }

        player.transform.position =
            data.playerSpawnPosition;

        player.ConfigureCoins(data.coinsRequired);

        if (player.portal != null)
        {
            player.portal.transform.position =
                data.portalPosition;
        }
    }

    private void ConfigureTimer(LevelData data)
    {
        if (starTimer == null)
        {
            Debug.LogError("StarTimer reference missing!");
            return;
        }

        starTimer.Begin(data);
    }

    private void ConfigureLevelUI(LevelData data)
    {
        bool isBossLevel = data.levelType == LevelType.Boss;

        if (bossHealthBarObject != null)
        {
            bossHealthBarObject.SetActive(isBossLevel);
        }

        if (starsGroup != null)
        {
            starsGroup.anchoredPosition = isBossLevel
                ? bossStarsPosition
                : normalStarsPosition;
        }
    }
}
