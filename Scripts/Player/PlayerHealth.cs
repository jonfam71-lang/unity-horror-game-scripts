using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool isBlocking;

    [Header("UI")]
    public Image damageFlash;
    public float flashSpeed = 5f;
    public Image hpFill;

    [Header("Knockback")]
    public float knockbackForce = 5f;

    private CharacterController controller;
    private Vector3 knockbackVelocity;
    private PlayerBlock block;
    public DeathScreen deathScreen;

    private bool isFlashing;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();
        block = GetComponent<PlayerBlock>();
    }

    void Update()
    {
        if (isDead) return; // ⬅️ ВАЖНО

        if (isFlashing)
        {
            Color c = damageFlash.color;
            c.a = Mathf.Lerp(c.a, 0, Time.deltaTime * flashSpeed);
            damageFlash.color = c;

            if (c.a < 0.01f)
                isFlashing = false;
        }

        if (knockbackVelocity.magnitude > 0.1f && controller.enabled)
        {
            controller.Move(knockbackVelocity * Time.deltaTime);
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, Time.deltaTime * 10f);
        }
    }

    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        if (isDead) return;

        if (block != null && block.isBlocking)
        {
            Debug.Log("Урон заблокирован");
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        ApplyKnockback(hitDirection);
        FlashScreen();

        hpFill.fillAmount = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
            Die();
    }

    void ApplyKnockback(Vector3 dir)
    {
        dir.y = 0;
        knockbackVelocity = dir.normalized * knockbackForce;
    }

    void FlashScreen()
    {
        Debug.Log("FLASH SCREEN");

        Color c = damageFlash.color;
        c.a = 0.6f;
        damageFlash.color = c;
        isFlashing = true;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("PLAYER DEAD");

        if (deathScreen != null)
            deathScreen.ShowDeathScreen();

        controller.enabled = false;
    }

    public void HealFull()
    {
        if(isDead) return;
        currentHealth = maxHealth;
        hpFill.fillAmount = 1f;

        Debug.Log("HP восстановлено полностью");
    }
}