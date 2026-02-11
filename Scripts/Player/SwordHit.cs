using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public Collider hitCollider;
    public int damage = 25;

    void Awake()
    {
        if (hitCollider == null)
            hitCollider = GetComponent<Collider>();

        hitCollider.enabled = false;
    }

    // גûחûגאועסÿ טח ְְָּֽײָָ
    public void EnableHit()
    {
        hitCollider.enabled = true;
        Debug.Log("HIT ON");
    }

    // גûחûגאועסÿ טח ְְָּֽײָָ
    public void DisableHit()
    {
        hitCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        
        if (other.CompareTag("Enemy"))
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);

            other.GetComponent<EnemyHealth>()
                ?.TakeDamage(damage, hitPoint);
        }
    }
}