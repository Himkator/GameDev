using System.Collections;
using UnityEngine;
using TMPro;

public class LevelIntro : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelDescription;
    public float showTime = 1f;

    IEnumerator Start()
    {
        levelText.gameObject.SetActive(true);
        levelDescription.gameObject.SetActive(true);

        yield return new WaitForSeconds(showTime);

        levelText.gameObject.SetActive(false);
        levelDescription.gameObject.SetActive(false);
    }
}