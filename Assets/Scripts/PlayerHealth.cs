using UnityEngine;
using System.Collections; // Necessário para o IEnumerator

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Saúde máxima do jogador
    private int currentHealth;

    public GameObject deathEffect; // Efeito ao morrer (opcional)

    private SpriteRenderer spriteRenderer; // Para modificar a cor do sprite
    private Color originalColor; // Cor original do personagem
    public Color damageColor = Color.red; // Cor que o personagem ficará ao tomar dano
    public float flashDuration = 0.1f; // Duração do flash (em segundos)
    private bool isFlashing = false; // Para evitar múltiplos flashes ao mesmo tempo

    void Start()
    {
        // Inicializa a saúde com o valor máximo
        currentHealth = maxHealth;

        // Obtém o componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Armazena a cor original do personagem
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduz a saúde
        Debug.Log("Player tomou dano: " + damage + " | Saúde atual: " + currentHealth);

        // Se o personagem já estiver piscando, não deixe ele piscar novamente até o final do efeito
        if (!isFlashing)
        {
            StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("O jogador morreu!");

        // Se houver um efeito de morte, instancie-o
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Desativar o jogador ou reiniciar o jogo
        gameObject.SetActive(false); // Alternativa: adicione lógica para recomeçar o nível.
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Não ultrapassa a saúde máxima
        }
        Debug.Log("Player curado: " + healAmount + " | Saúde atual: " + currentHealth);
    }

    // Coroutine para piscar o personagem de vermelho
    private IEnumerator FlashRed()
    {
        isFlashing = true;

        // Altera a cor para vermelho
        spriteRenderer.color = damageColor;

        // Espera a duração do flash
        yield return new WaitForSeconds(flashDuration);

        // Restaura a cor original
        spriteRenderer.color = originalColor;

        isFlashing = false;
    }
}
