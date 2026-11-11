using UnityEngine;
public class BullCharger : MonoBehaviour
{
    enum State { Idle, Windup, Dash, Stunned, Return }

    [SerializeField] GameObject player;
    [SerializeField] GameObject eye; 
    [SerializeField] Animator anim;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LayerMask playerMask;

    [SerializeField] float viewRange = 5f;
    [SerializeField] float fov = 90f;
    [SerializeField] float sightInterval = 0.1f;

    [SerializeField] float windup = 0.7f; 
    [SerializeField] float dashSpeed = 7.5f;
    [SerializeField] float maxDashDistance = 7f;
    [SerializeField] float wallCheck = 0.9f; 
    [SerializeField] float stun = 1f;
    [SerializeField] float hitRadius = 0.8f;
    [SerializeField] int damage = 1;

    [SerializeField] float returnSpeed = 1.6f;

    State state;
    float timer;
    float sightTimer;
    bool canSee;
    bool hasHit;
    int facing = 1;
    int dashDir = 1;
    float homeX;
    float dashStartX;

    Vector2 EyePos => eye ? (Vector2)eye.transform.position : (Vector2)transform.position + Vector2.up * 0.5f;
    Vector2 PlayerPoint => (Vector2)player.transform.position + Vector2.up * 0.5f;

    void Awake()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        facing = transform.localScale.x < 0 ? -1 : 1;
        homeX = transform.position.x;
    }

    void Start() => SetState(State.Idle);

    void Update()
    {
        timer += Time.deltaTime;

        sightTimer -= Time.deltaTime;
        if (sightTimer <= 0f)
        {
            sightTimer = sightInterval;
            canSee = EnemySight2D.CanSee(EyePos, Vector2.right * facing, PlayerPoint, viewRange, fov, obstacleMask);
        }

        switch (state)
        {
            case State.Idle:
                if (canSee) SetState(State.Windup);
                break;

            case State.Windup: 
                if (timer >= windup) SetState(State.Dash);
                break;

            case State.Dash:
                transform.position += Vector3.right * dashDir * dashSpeed * Time.deltaTime;

                if (!hasHit && Physics2D.OverlapCircle((Vector2)transform.position + Vector2.up * 0.5f, hitRadius, playerMask) is Collider2D c
                    && c.GetComponentInParent<Health>() is Health target)
                {
                    target.TakeDamage(damage);
                    hasHit = true; 
                }

                bool wallAhead = Physics2D.Raycast(EyePos, Vector2.right * dashDir, wallCheck, obstacleMask).collider != null;
                bool tooFar = Mathf.Abs(transform.position.x - dashStartX) >= maxDashDistance;
                if (wallAhead || tooFar) SetState(State.Stunned);
                break;

            case State.Stunned: 
                if (timer >= stun) SetState(State.Return);
                break;

            case State.Return:
                Face(homeX - transform.position.x);
                Vector3 p = transform.position;
                p.x = Mathf.MoveTowards(p.x, homeX, returnSpeed * Time.deltaTime);
                transform.position = p;
                if (Mathf.Abs(p.x - homeX) < 0.05f) SetState(State.Idle);
                break;
        }
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;

        if (next == State.Windup)  
        {
            Face(player.transform.position.x - transform.position.x);
            dashDir = facing;
        }
        if (next == State.Dash) { dashStartX = transform.position.x; hasHit = false; }

        if (anim)
        {
            anim.SetTrigger(next.ToString());
        }
    }

    void Face(float dx)
    {
        if (Mathf.Abs(dx) < 0.01f) return;
        facing = dx > 0 ? 1 : -1;
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * facing;
        transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        int f = transform.localScale.x < 0 ? -1 : 1;
        Vector3 e = eye ? eye.transform.position : transform.position + Vector3.up * 0.5f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(e, viewRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(e, Quaternion.Euler(0, 0, fov / 2f) * Vector2.right * f * viewRange);
        Gizmos.DrawRay(e, Quaternion.Euler(0, 0, -fov / 2f) * Vector2.right * f * viewRange);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(e, Vector2.right * f * wallCheck);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, hitRadius);
    }
}