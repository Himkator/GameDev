using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    public float goal = 60f;
    float t;
    bool won = false;
    bool AllCoinCollected = false;
    public Animator animator;
    public GameObject portal;
    public TextMeshProUGUI timerText;

    void Update()
    {
        t += Time.deltaTime;
        AllCoinCollected = gameObject.GetComponent<Player2D>().AllCoinCollected;
        timerText.text = "Time: " + Mathf.Round(t).ToString() + " / " + goal.ToString();
        if (!won && t >= goal && AllCoinCollected)
        {
            won = true;
            portal.SetActive(true);
            animator.SetBool("isEnd", true);
            Debug.Log("YOU WIN!");
        }
    }
}
