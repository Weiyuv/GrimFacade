using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAMAGETESTE : MonoBehaviour
{
    [Header("Configurações de Vida")]
    [SerializeField]
    private float maxLives = 40f;  // Quantidade total de vidas configurável no Unity
    private float currentLives;     // Vidas atuais do objeto

    [Header("Efeitos de Partículas")]
    [SerializeField]
    private ParticleSystem smoke;   // Partícula de fumaça para indicar dano
    [SerializeField]
    private ParticleSystem explosion; // Partícula de explosão para quando morrer

    [Header("Configurações de Destruição")]
    [SerializeField]
    private float destructionDelay = 0.8f;  // Tempo de delay antes de destruir o objeto

    private SpriteRenderer spriteRenderer;

    // Start é chamado antes do primeiro frame
    void Start()
    {
        currentLives = maxLives;  // Inicializa as vidas atuais com o valor máximo configurado
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o SpriteRenderer (para desabilitar a visibilidade)

        // Exibe o valor inicial de vidas no console para depuração
        Debug.Log("Vidas iniciais: " + currentLives);
    }

    // Chamado quando o objeto é atingido por partículas (colisão com partículas)
    private void OnParticleCollision(GameObject other)
    {
        // Reduz as vidas ao ser atingido
        TakeDamage(1f);  // Cada colisão reduz 1 vida
    }

    // Função que aplica dano e gerencia as vidas
    private void TakeDamage(float damageAmount)
    {
        currentLives -= damageAmount;  // Subtrai a quantidade de dano das vidas atuais
        Debug.Log("Vidas restantes: " + currentLives);  // Exibe no console a quantidade de vidas restantes

        // Se as vidas estão abaixo da metade, cria a partícula de fumaça
        if (currentLives < maxLives / 2f)
        {
            if (smoke != null)
            {
                CreateAndPlayParticle(smoke);
            }
        }

        // Quando as vidas chegam a 0 ou menos, chama a função de destruição
        if (currentLives <= 0f)
        {
            HandleDeath();
        }
    }

    // Função que lida com a morte do objeto (destruição e efeitos)
    private void HandleDeath()
    {
        if (explosion != null)
        {
            CreateAndPlayParticle(explosion);  // Cria a partícula de explosão
        }

        // Desabilita o sprite para indicar que o objeto "morreu"
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // Destroi o objeto após um pequeno delay
        Destroy(gameObject, destructionDelay);
    }

    // Função para instanciar e tocar a partícula
    private void CreateAndPlayParticle(ParticleSystem particle)
    {
        // Instancia a partícula na posição do objeto
        GameObject particleObj = Instantiate(particle.gameObject, transform.position, Quaternion.identity);
        particleObj.transform.parent = transform;  // Torna a partícula filha do objeto para não se mover sozinha
        particleObj.GetComponent<ParticleSystem>().Play();  // Toca a partícula
    }
}
