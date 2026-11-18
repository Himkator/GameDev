
using UnityEngine;

public class ResetProgress : MonoBehaviour
{
    [SerializeField] private LevelSelectButton[] levelButtons;

    public void ResetGame()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager missing!");
            return;
        }

        SaveManager.Instance.ResetProgress();

        GameSession.levelIndex = 0;

        foreach (LevelSelectButton button in levelButtons)
        {
            if (button != null)
                button.Refresh();
        }

        Debug.Log("Progress reset!");
    }
}
