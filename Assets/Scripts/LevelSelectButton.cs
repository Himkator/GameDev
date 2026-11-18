
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int levelIndex;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private GameObject lockIcon;

    private void Start()
    {
        if (button == null)
            button = GetComponent<Button>();

        button.onClick.AddListener(PlayLevel);

        Refresh();
    }

    public void Refresh()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager missing!");
            return;
        }

        int levelNumber = levelIndex + 1;

        bool unlocked =
            SaveManager.Instance.IsLevelUnlocked(levelNumber);

        button.interactable = unlocked;

        if (lockIcon != null)
            lockIcon.SetActive(!unlocked);

        int stars =
            SaveManager.Instance.GetBestStars(levelNumber);

        if (starsText != null)
            starsText.text = stars + " / 3 ★";
    }

    public void PlayLevel()
    {
        int levelNumber = levelIndex + 1;

        if (!SaveManager.Instance.IsLevelUnlocked(levelNumber))
            return;

        GameSession.levelIndex = levelIndex;

        Time.timeScale = 1f;

        SceneManager.LoadScene("Game");
    }
}
