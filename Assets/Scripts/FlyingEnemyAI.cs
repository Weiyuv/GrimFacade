using System.Collections;
using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    public Transform player;           // Referência ao jogador
    public float speed = 2f;           // Velocidade de movimento normal do inimigo
    public float dashSpeed = 10f;      // Velocidade durante o avanço (dash)
    public float followDistance = 5f;  // Distância mínima para começar a seguir o jogador
    public float dashDistance = 2f;    // Distância mínima para iniciar o avanço
    public float dashDuration = 0.5f;  // Duração do avanço
    public float postDashPause = 1.5f; // Tempo que o inimigo fica parado após o dash
    private Rigidbody2D rb;            // Referência ao Rigidbody2D do inimigo
    private bool isDashing = false;    // Verifica se o inimigo está fazendo o dash
    private bool facingRight = false;  // Verifica se o inimigo está virado para a direita (ajustado)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Pega o Rigidbody2D do inimigo
    }

    void Update()
    {
        if (isDashing) return;  // Se estiver no dash ou na pausa, não faz nada

        // Calcula a distância até o jogador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Se o jogador estiver dentro da distância de seguir, move-se em direção a ele
        if (distanceToPlayer < followDistance)
        {
            FollowPlayer();
        }

        // Se o jogador estiver dentro da distância de dash, inicia o avanço
        if (distanceToPlayer < dashDistance)
        {
            StartCoroutine(DashAtPlayer());
        }

        // Faz o inimigo virar na direção do jogador
        FlipTowardsPlayer();
    }

    void FollowPlayer()
    {
        // Calcula a direção em direção ao jogador
        Vector2 direction = (player.position - transform.position).normalized;

        // Move o inimigo em direção ao jogador
        rb.velocity = direction * speed;
    }

    IEnumerator DashAtPlayer()
    {
        isDashing = true; // Marca que está no dash

        // Calcula a direção do dash
        Vector2 dashDirection = (player.position - transform.position).normalized;

        // Define a velocidade do inimigo durante o dash
        rb.velocity = dashDirection * dashSpeed;

        // Aguarda pelo tempo do dash
        yield return new WaitForSeconds(dashDuration);

        // Para o movimento após o dash
        rb.velocity = Vector2.zero;

        // Aguarda o tempo de pausa após o dash
        yield return new WaitForSeconds(postDashPause);

        // Volta a permitir que o inimigo se mova
        isDashing = false;
    }

    void FlipTowardsPlayer()
    {
        // Verifica se o inimigo está à direita ou esquerda do jogador
        if (player.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        // Inverte a escala no eixo X para virar o inimigo
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
