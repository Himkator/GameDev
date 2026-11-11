using UnityEngine;

public class CannonShooter : MonoBehaviour
{
    enum State { Idle, Aim, Fire, Cooldown }

    [SerializeField] GameObject player; 
    [SerializeField] GameObject muzzle; 
    [SerializeField] Animator anim;
    [SerializeField] CannonBall ballPrefab;
    [SerializeField] GameObject laser;
    [SerializeField] LayerMask obstacleMask; 

    [SerializeField] float viewRange = 7f;
    [SerializeField] float fov = 90f;
    [SerializeField] float sightInterval = 0.1f;
    

    [SerializeField] float aimTime = 0.6f; 
    [SerializeField] float fireTime = 0.15f; 
    [SerializeField] float cooldown = 1.5f;
    [SerializeField] float shotSpeed = 3.75f;
    [SerializeField] int damage = 1;
    [SerializeField] float warningWidth = 0.1f;

    State state;
    float timer;
    float sightTimer;
    bool canSee;
    Vector2 warningSpriteSize = Vector2.one;
    Vector2 Facing => new Vector2(transform.lossyScale.x < 0 ? -1f : 1f, 0f);
    Vector2 MuzzlePos => muzzle ? (Vector2)muzzle.transform.position : (Vector2)transform.position + Facing * 0.8f + Vector2.up * 0.5f;
    Vector2 PlayerPoint => (Vector2)player.transform.position + Vector2.up * 0.5f;

    void Awake()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (laser) {
            laser.SetActive(false);
            warningSpriteSize = laser.GetComponent<SpriteRenderer>().sprite.bounds.size;
        }
    }

    void Start() => SetState(State.Idle);

    void Update()
    {
        timer += Time.deltaTime;

        sightTimer -= Time.deltaTime;
        if (sightTimer <= 0f)
        {
            sightTimer = sightInterval;
            canSee = EnemySight2D.CanSee(MuzzlePos, Facing, PlayerPoint, viewRange, fov, obstacleMask);
        }

        switch (state)
        {
            case State.Idle:
                if (canSee) SetState(State.Aim);

                break;

            case State.Aim:
                UpdateLaser();
                if (timer >= aimTime) { Shoot(); SetState(State.Fire); }
                break;

            case State.Fire:
                if (timer >= fireTime) SetState(State.Cooldown);
                break;

            case State.Cooldown:
                if (timer >= cooldown) SetState(State.Idle);
                break;
        }
    }

    void SetState(State next)   
    {
        state = next;
        timer = 0f;
        if (laser) laser.SetActive(next == State.Aim);
        if (anim) anim.SetTrigger(next.ToString());
    }

    void Shoot()
    {
        CannonBall ball = Instantiate(ballPrefab, MuzzlePos, Quaternion.identity);
        ball.Init(Facing, shotSpeed, damage, obstacleMask);
    }

    void UpdateLaser()
    {
        if (!laser) return;
        Vector2 start = MuzzlePos;
        RaycastHit2D hit = Physics2D.Raycast(start, Facing, viewRange, obstacleMask);
        Vector2 end = hit.collider ? hit.point : start + Facing * viewRange;
        float len = hit.collider ? hit.distance : viewRange;
        laser.transform.position = start + Facing * (len * 0.5f);
        laser.transform.localScale = new Vector3(len, warningWidth, 1f);
    }

    void OnDrawGizmosSelected()
    {
        Vector2 f = new Vector2(transform.lossyScale.x < 0 ? -1f : 1f, 0f);
        Vector3 m = muzzle ? muzzle.transform.position : transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m, viewRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(m, Quaternion.Euler(0, 0, fov / 2f) * f * viewRange);
        Gizmos.DrawRay(m, Quaternion.Euler(0, 0, -fov / 2f) * f * viewRange);
    }
}