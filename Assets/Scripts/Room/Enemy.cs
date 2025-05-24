using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float wakeUpDistance = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 1.5f;  // медленно, для хоррора
    public int damage = 10;
    public float attackCooldown = 1.5f;

    private Transform player;
    private bool isAwake = false;
    private float lastAttackTime;

    private Vector3 patrolTarget;
    private float patrolRadius = 5f;
    private float patrolWaitTime = 2f;
    private float patrolWaitCounter = 0f;

    private Room currentRoom;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;

        currentRoom = GetComponentInParent<Room>();

        SetNewPatrolTarget();
        Sleep();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isAwake && distanceToPlayer <= wakeUpDistance && IsPlayerInSameRoom())
        {
            WakeUp();
        }
        else if (isAwake && (distanceToPlayer > wakeUpDistance || !IsPlayerInSameRoom()))
        {
            Sleep();
        }

        if (isAwake)
        {
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                FollowPlayer();
            }
        }
        else
        {
            Patrol();
        }
    }

    bool IsPlayerInSameRoom()
    {
        if (currentRoom == null || player == null) return false;
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null) return false;

        return playerMovement.currentRoom == currentRoom;
    }

    void WakeUp()
    {
        if (isAwake) return;
        isAwake = true;
        Debug.Log($"{gameObject.name} проснулся!");
    }

    void Sleep()
    {
        if (!isAwake) return;
        isAwake = false;
        Debug.Log($"{gameObject.name} засыпает.");
        patrolWaitCounter = 0;
        SetNewPatrolTarget();
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log($"{gameObject.name} атакует игрока!");
            // здесь можно вызвать урон у игрока
            lastAttackTime = Time.time;
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        RotateTowards(player.position);
    }

    void Patrol()
    {
        if (patrolWaitCounter > 0)
        {
            patrolWaitCounter -= Time.deltaTime;
            return;
        }

        Vector3 direction = (patrolTarget - transform.position);
        direction.y = 0;

        if (direction.magnitude < 0.3f)
        {
            patrolWaitCounter = patrolWaitTime;
            SetNewPatrolTarget();
        }
        else
        {
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
            RotateTowards(patrolTarget);
        }
    }

    void SetNewPatrolTarget()
    {
        if (currentRoom == null)
        {
            patrolTarget = transform.position;
            return;
        }
        Vector3 center = currentRoom.transform.position;
        patrolTarget = center + new Vector3(
            Random.Range(-patrolRadius, patrolRadius),
            0,
            Random.Range(-patrolRadius, patrolRadius)
        );
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5f * Time.deltaTime);
        }
    }
}