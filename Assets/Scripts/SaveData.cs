
using System;
using System.Collections.Generic;

[Serializable]
public class LevelResult
{
    public int levelNumber;
    public int bestStars;
    public float bestTime;
}

[Serializable]
public class SaveData
{
    public int version = 1;
    public int unlockedLevel = 1;
    public float musicVolume = 1f;

    public List<LevelResult> results = new();
}
