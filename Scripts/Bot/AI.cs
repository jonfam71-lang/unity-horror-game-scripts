using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Vision")]
    public float viewDistance = 10f;
    public float attackDistance = 2f;

    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;

    public int damage = 20;

    private bool isAttacking;
    private bool isDead = false;

    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > viewDistance)
            Idle();
        else if (distance > attackDistance)
            Chase();
        else
            Attack();

        animator.SetFloat("Speed", isAttacking ? 0f : agent.velocity.magnitude);
    }

    void Idle()
    {
        if (isDead || !agent.isOnNavMesh) return;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        isAttacking = false;
    }

    void Chase()
    {
        if (isDead || !agent.isOnNavMesh) return;

        agent.isStopped = false;
        agent.angularSpeed = 120f;
        agent.SetDestination(player.position);
        isAttacking = false;
    }

    void Attack()
    {
        if (isDead || !agent.isOnNavMesh) return;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.angularSpeed = 0f;

        if (isAttacking) return;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            isAttacking = true;
            animator.SetTrigger("Attack");
        }

        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(lookDir),
            Time.deltaTime * 5f
        );
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void DealDamage()
    {
        float radius = 1.5f;

        Collider[] hits = Physics.OverlapSphere(
            transform.position + transform.forward,
            radius
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Vector3 dir = hit.transform.position - transform.position;
                hit.GetComponent<PlayerHealth>()?.TakeDamage(damage, dir);
            }
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        agent.enabled = false;
    }
}
