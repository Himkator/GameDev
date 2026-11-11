using UnityEngine;

public class SlimePatroller : MonoBehaviour
{
    enum State { Patrol, Chase, Windup, Strike }

    [SerializeField] GameObject player;
    [SerializeField] GameObject eye;
    [SerializeField] Animator anim;
    [SerializeField] Transform[] waypoints; 
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LayerMask playerMask;

    [SerializeField] float viewRange = 4.5f;
    [SerializeField] float fov = 90f;
    [SerializeField] float sightInterval = 0.1f;

    [SerializeField] float patrolSpeed = 1.4f; 
    [SerializeField] float chaseSpeed = 2.0f; 
    [SerializeField] float leashExtra = 1.5f; 
    [SerializeField] float loseTime = 2f;

    [SerializeField] float attackRange = 0.9f;
    [SerializeField] float attackHeight = 0.8f;
    [SerializeField] float windup = 0.5f; 
    [SerializeField] float recovery = 0.4f;
    [SerializeField] float cooldown = 1.0f;
    [SerializeField] float hitReach = 0.7f;
    [SerializeField] float hitRadius = 0.6f;
    [SerializeField] int damage = 1;
    [SerializeField] int health = 1;

    State state;
    float timer; 
    float unseenTime; 
    float sightTimer;
    float nextAttackTime;
    bool canSee;
    int wp;
    int facing = 1; 
    float minX, maxX;

    Vector2 EyePos => eye ? (Vector2)eye.transform.position : (Vector2)transform.position + Vector2.up * 0.4f;
    Vector2 PlayerPoint => (Vector2)player.transform.position + Vector2.up * 0.5f;

    void Awake()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player"); 
        facing = transform.localScale.x < 0 ? -1 : 1;

        minX = maxX = transform.position.x;
        foreach (var w in waypoints) { minX = Mathf.Min(minX, w.position.x); maxX = Mathf.Max(maxX, w.position.x); }
        minX -= leashExtra; maxX += leashExtra;
    }

    void Start() => SetState(State.Patrol);

    void Update()
    {
        if (health <= 0) return;
        timer += Time.deltaTime;

        sightTimer -= Time.deltaTime;
        if (sightTimer <= 0f)
        {
            sightTimer = sightInterval;
            canSee = EnemySight2D.CanSee(EyePos, Vector2.right * facing, PlayerPoint, viewRange, fov, obstacleMask);
        }

        switch (state)
        {
            case State.Patrol:
                if (waypoints.Length > 0)
                {
                    float tx = waypoints[wp].position.x;
                    Face(tx - transform.position.x);
                    MoveX(tx, patrolSpeed);
                    if (Mathf.Abs(transform.position.x - tx) < 0.05f) wp = (wp + 1) % waypoints.Length;
                }
                if (canSee) SetState(State.Chase);
                break;

            case State.Chase:
                Face(player.transform.position.x - transform.position.x);
                MoveX(Mathf.Clamp(player.transform.position.x, minX, maxX), chaseSpeed);
                unseenTime = canSee ? 0f : unseenTime + Time.deltaTime;

                bool close = Mathf.Abs(player.transform.position.x - transform.position.x) < attackRange
                          && Mathf.Abs(player.transform.position.y - transform.position.y) < attackHeight;
                if (close && Time.time >= nextAttackTime) SetState(State.Windup);
                else if (unseenTime > loseTime) SetState(State.Patrol);
                break;

            case State.Windup:
                if (timer >= windup) { Strike(); SetState(State.Strike); }
                break;

            case State.Strike:
                if (timer >= recovery) SetState(State.Chase);
                break;
        }
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        if (next == State.Chase) unseenTime = 0f;
        if (anim) anim.SetTrigger(next.ToString());
    }

    void Strike()
    {
        nextAttackTime = Time.time + cooldown;
        Vector2 center = (Vector2)transform.position + new Vector2(facing * hitReach, 0.4f);
        Collider2D hit = Physics2D.OverlapCircle(center, hitRadius, playerMask);
        Debug.Log("Strike, hit: " + (hit ? hit.name : "nothing"));
        if (hit && hit.GetComponentInParent<Health>() is Health target) target.TakeDamage(damage);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0) {
            anim.SetTrigger("Die");
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 1f);
        }

    }




    void MoveX(float targetX, float speed)
    {
        Vector3 p = transform.position;
        p.x = Mathf.MoveTowards(p.x, targetX, speed * Time.deltaTime);
        transform.position = p;
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
        Vector3 e = eye ? eye.transform.position : transform.position + Vector3.up * 0.4f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(e, viewRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(e, Quaternion.Euler(0, 0, fov / 2f) * Vector2.right * f * viewRange);
        Gizmos.DrawRay(e, Quaternion.Euler(0, 0, -fov / 2f) * Vector2.right * f * viewRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(f * hitReach, 0.4f), hitRadius);
    }
}