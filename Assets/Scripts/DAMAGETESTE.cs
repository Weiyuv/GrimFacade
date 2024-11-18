using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAMAGETESTE: MonoBehaviour
{
    [Header("Configura��es de Vida")]
    [SerializeField]
    private float maxLives = 40f;  // A quantidade m�xima de vidas
    private float currentLives;     // Vidas atuais

    [Header("Efeitos de Part�culas")]
    [SerializeField]
    private ParticleSystem smoke;   // Part�cula de fuma�a para indicar dano
    [SerializeField]
    private ParticleSystem explosion; // Part�cula de explos�o para quando morrer

    [Header("Configura��es de Destrui��o")]
    [SerializeField]
    private float destructionDelay = 0.8f;  // Delay antes da destrui��o do objeto

    private SpriteRenderer spriteRenderer;

    // Start � chamado antes do primeiro frame
    void Start()
    {
        currentLives = maxLives;  // Inicializa as vidas atuais com o valor m�ximo configurado
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obt�m o SpriteRenderer (para desabilitar a visibilidade)

        // Exibe o valor inicial de vidas no console
        Debug.Log("Vidas iniciais: " + currentLives);
    }

    // Chamado quando o objeto � atingido por part�culas (colis�o com part�culas)
    private void OnParticleCollision(GameObject other)
    {
        // Reduz as vidas ao ser atingido
        TakeDamage(1f);  // Reduz 1 vida por colis�o com part�culas
    }

    // Fun��o que aplica dano e gerencia as vidas
    private void TakeDamage(float damageAmount)
    {
        currentLives -= damageAmount;  // Subtrai a quantidade de dano das vidas atuais
        Debug.Log("Vidas restantes: " + currentLives);

        // Verifica se as vidas est�o abaixo de metade e, se sim, cria a part�cula de fuma�a
        if (currentLives < maxLives / 2f)
        {
            if (smoke != null)
            {
                CreateAndPlayParticle(smoke);
            }
        }

        // Quando as vidas chegam a 0 ou menos, destr�i o objeto
        if (currentLives <= 0f)
        {
            HandleDeath();
        }
    }

    // Fun��o que lida com a morte do objeto (destrui��o e efeitos)
    private void HandleDeath()
    {
        if (explosion != null)
        {
            CreateAndPlayParticle(explosion);  // Cria a part�cula de explos�o
        }

        // Desabilita o sprite para indicar que o objeto "morreu"
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // Destroi o objeto ap�s um pequeno delay
        Destroy(gameObject, destructionDelay);
    }

    // Fun��o para instanciar e tocar a part�cula
    private void CreateAndPlayParticle(ParticleSystem particle)
    {
        // Instancia a part�cula na posi��o do objeto
        GameObject particleObj = Instantiate(particle.gameObject, transform.position, Quaternion.identity);
        particleObj.transform.parent = transform;  // Torna a part�cula filha do objeto para n�o se mover sozinha
        particleObj.GetComponent<ParticleSystem>().Play();  // Toca a part�cula
    }
}
