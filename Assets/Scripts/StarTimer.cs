
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StarTimer : MonoBehaviour
{
    public float threeStarGoal = 30f;
    public float twoStarGoal = 60f;

    private LevelData levelData;

    private float t;
    private bool isRunning;

    public TextMeshProUGUI timerText;
    public Image[] stars;

    public float ElapsedTime => t;

    public void Begin(LevelData data)
    {
        if (data == null)
        {
            Debug.LogError("LevelData is missing!");
            return;
        }

        levelData = data;

        threeStarGoal = data.threeStarGoal;
        twoStarGoal = data.twoStarGoal;

        t = 0f;
        isRunning = true;

        foreach (Image star in stars)
        {
            if (star != null)
                star.color = Color.white;
        }

        UpdateUI();
    }

    void Update()
    {
        if (!isRunning)
            return;

        t += Time.deltaTime;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText == null)
            return;

        if (t < threeStarGoal)
        {
            timerText.text = "Time: " +
                Mathf.Round(t) + " / " + threeStarGoal;
        }
        else if (t < twoStarGoal)
        {
            if (stars != null && stars.Length >= 3 && stars[2] != null)
                stars[2].color = Color.gray;

            timerText.text = "Time: " +
                Mathf.Round(t) + " / " + twoStarGoal;
        }
        else
        {
            if (stars != null && stars.Length >= 3)
            {
                if (stars[2] != null)
                    stars[2].color = Color.gray;

                if (stars[1] != null)
                    stars[1].color = Color.gray;
            }

            timerText.text = "Time: " + Mathf.Round(t);
        }
    }

    public int FinishLevel()
    {
        isRunning = false;

        if (levelData == null)
            return 0;

        int earnedStars = levelData.GetStars(t);

        Debug.Log("Level finished! Stars: " + earnedStars);

        return earnedStars;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
