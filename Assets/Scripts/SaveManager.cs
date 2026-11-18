
using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public SaveData Data { get; private set; }

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void Load()
    {
        Data = new SaveData();

        if (!File.Exists(SavePath))
            return;

        try
        {
            string json = File.ReadAllText(SavePath);
            SaveData loaded = JsonUtility.FromJson<SaveData>(json);

            if (loaded == null ||
                loaded.version != 1 ||
                loaded.unlockedLevel < 1 ||
                loaded.results == null)
            {
                throw new Exception("Invalid save data");
            }

            Data = loaded;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Save load failed: " + e.Message);
            Data = new SaveData();
        }
    }

    public void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(SavePath, json);

            Debug.Log("Saved to: " + SavePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Save failed: " + e.Message);
        }
    }

    public void CompleteLevel(int levelNumber, int stars, float time)
    {
        if (levelNumber < 1 || time < 0f)
            return;

        stars = Mathf.Clamp(stars, 1, 3);

        LevelResult result = Data.results.Find(
            r => r.levelNumber == levelNumber
        );

        if (result == null)
        {
            result = new LevelResult
            {
                levelNumber = levelNumber,
                bestStars = stars,
                bestTime = time
            };

            Data.results.Add(result);
        }
        else
        {
            result.bestStars = Mathf.Max(result.bestStars, stars);

            if (time < result.bestTime)
                result.bestTime = time;
        }

        Data.unlockedLevel = Mathf.Max(
            Data.unlockedLevel,
            levelNumber + 1
        );

        Save();
    }

    public bool IsLevelUnlocked(int levelNumber)
    {
        return levelNumber >= 1 &&
               levelNumber <= Data.unlockedLevel;
    }

    public int GetBestStars(int levelNumber)
    {
        LevelResult result = Data.results.Find(
            r => r.levelNumber == levelNumber
        );

        return result != null ? result.bestStars : 0;
    }

    public void SetMusicVolume(float value)
    {
        Data.musicVolume = Mathf.Clamp01(value);
        Save();
    }

    public void ResetProgress()
    {
        Data = new SaveData();
        Save();
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused && Data != null)
            Save();
    }

    private void OnApplicationQuit()
    {
        if (Data != null)
            Save();
    }
}
