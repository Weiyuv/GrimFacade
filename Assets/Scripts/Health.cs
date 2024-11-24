using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject deathEffect;
    public Slider healthBar; // Certifique-se de atribuí-lo no Inspector!

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color damageColor = Color.red;
    public float flashDuration = 0.1f;

    private float damageFlashTimer = 0f;

    // Referência ao ponto de respawn
    public Transform respawnPoint; 
    public float respawnDelay = 2f; // Tempo de delay para o respawn

    void Start()
    {
        currentHealth = maxHealth;

        // Configura o Slider
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (damageFlashTimer > 0)
        {
            damageFlashTimer -= Time.deltaTime;
            if (damageFlashTimer <= 0 && spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log("Saúde Atual: " + currentHealth);

        // Atualiza a barra de vida
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            Debug.Log("Slider atualizado: " + healthBar.value);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            damageFlashTimer = flashDuration;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("O jogador morreu!");

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Desativa o jogador temporariamente
        gameObject.SetActive(false);

        // Inicia o respawn com delay
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        // Restaura a saúde
        currentHealth = maxHealth;

        // Atualiza a barra de vida
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Move o jogador para o ponto de respawn
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }

        // Reativa o jogador
        gameObject.SetActive(true);

        Debug.Log("Player respawnou!");
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Atualiza a barra de vida
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        Debug.Log("Saúde Curada: " + healAmount + " | Saúde Atual: " + currentHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage") || collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Damage") || collider.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }
}
