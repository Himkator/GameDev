using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Sprite[] stages;
    Image img;

    void Awake() => img = GetComponent<Image>();

    public void SetValue(float percent)
    {
        int index = Mathf.RoundToInt((1f - percent) * (stages.Length - 1));
        img.sprite = stages[index];
    }
}