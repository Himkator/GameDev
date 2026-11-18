
using UnityEngine;

public enum LevelType
{
    CollectCoins,
    Platforming,
    Boss
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemyPrefab;
    public Vector3 spawnPosition;
}

[CreateAssetMenu(
    fileName = "Level_01",
    menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Information")]
    [Min(1)] public int levelNumber = 1;
    public string levelName = "Level 01";

    [TextArea(2, 4)]
    public string levelDescription;

    public LevelType levelType = LevelType.CollectCoins;

    [Header("Level Layout")]
    public GameObject layoutPrefab;
    public Vector3 playerSpawnPosition;
    public Vector3 portalPosition;

    [Header("Star Rating")]
    [Min(0)] public float threeStarGoal = 30f;
    [Min(0)] public float twoStarGoal = 60f;

    [Header("Coins")]
    public GameObject coinPrefab;
    public Vector3[] coinPositions = new Vector3[0];
    public int coinsRequired = 6;

    [Header("Enemies")]
    public EnemySpawn[] enemies = new EnemySpawn[0];

    [Header("Boss")]
    public EnemySpawn boss;

    public int GetStars(float timeUsed)
    {
        if (timeUsed <= threeStarGoal)
            return 3;

        if (timeUsed <= twoStarGoal)
            return 2;

        return 1;
    }
}
