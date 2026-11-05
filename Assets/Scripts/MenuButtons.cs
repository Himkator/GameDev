using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public GameObject settingsPanel;
    
    public AudioSource bgSound;

    public void Play(){
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

    public void Settings(){
        settingsPanel.SetActive(true);
    }

    public void Back(){
        settingsPanel.SetActive(false);
    }

    public void SetVolume(float volume){
        bgSound.volume = volume;
    }

    public void Quit(){
        Application.Quit();
        Debug.Log("Quitting the game");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
