using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    public GameObject hitParticlesPrefab;

    private Animator animator;
    private NavMeshAgent agent;
    private Renderer rend;
    private Material mat;
    public float dissolveSpeed = 1f; // скорость растворения

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
            mat = rend.material; // экземпляр материала для этого врага
    }

    public void TakeDamage(int damage, Vector3 hitPoint)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (hitParticlesPrefab != null)
            Instantiate(hitParticlesPrefab, hitPoint, Quaternion.identity);

        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die");      // проигрываем анимацию
        if (agent != null) agent.enabled = false;
        GetComponent<Collider>().enabled = false;

        // запускаем dissolve через корутину
        if (mat != null)
            StartCoroutine(DelayedDissolve());
    }

    private IEnumerator DelayedDissolve()
    {
        // 🔹 2 секунды ждем анимацию смерти
        yield return new WaitForSeconds(3f);

        // 🔹 После задержки запускаем dissolve
        float dissolve = 0f;
        while (dissolve < 1f)
        {
            dissolve += Time.deltaTime * dissolveSpeed;
            mat.SetFloat("_DissolveAmount", dissolve);
            yield return null;
        }

        Destroy(gameObject);
    }
}