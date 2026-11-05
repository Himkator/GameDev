using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 offset = new Vector3(4f, 0f, 0f);
    public float speed = 0.5f;
    public bool isActive = true;

    private Vector3 start;
    private float timeOffset;
    private float lastX;

    void Start()
    {
        start = transform.position;
        lastX = start.x;
        timeOffset = Time.time;
    }

    void FixedUpdate()
    {
        if (!isActive)
            return;
        float t = Mathf.PingPong(Time.time * speed, 1f);
        Vector3 newPos = Vector3.Lerp(start, start + offset, t);
        transform.position = newPos;

        float dx = newPos.x - lastX;
        if (Mathf.Abs(dx) > 0.0001f)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x) * (dx > 0f ? 1f : -1f);
            transform.localScale = s;
        }
        lastX = newPos.x;
    }
}