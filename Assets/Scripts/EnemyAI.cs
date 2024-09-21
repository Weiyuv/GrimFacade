using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab do projétil
    public Transform player; // Referência ao jogador
    public Transform firePoint; // Ponto de onde o projétil será disparado
    public float moveSpeed = 3f; // Velocidade de movimento do inimigo
    public float shootingRange = 10f; // Alcance para o inimigo atirar
    public float fireRate = 2f; // Tempo entre disparos
    public float projectileSpeed = 10f; // Velocidade do projétil
    public float stoppingDistance = 2f; // Distância mínima que o inimigo mantém do jogador

    private float fireCooldown;
    private Animator animator; // Referência ao Animator
    private bool isShooting = false; // Controle de estado de tiro

    void Start()
    {
        fireCooldown = fireRate;
        animator = GetComponent<Animator>(); // Obtém o Animator do inimigo
    }

    void Update()
    {
        MoveTowardsPlayer();

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0 && IsPlayerInShootingRange() && !isShooting)
        {
            StartCoroutine(ShootAtPlayer());
            fireCooldown = fireRate;
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Move o inimigo em direção ao jogador se ele estiver longe o suficiente
            if (distanceToPlayer > stoppingDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                // Inverte o sprite do inimigo sem mudar o tamanho
                if (direction.x < 0)
                {
                    // Vira para a esquerda
                    GetComponent<SpriteRenderer>().flipX = true; 
                }
                else
                {
                    // Vira para a direita
                    GetComponent<SpriteRenderer>().flipX = false; 
                }
            }
        }
    }

    bool IsPlayerInShootingRange()
    {
        return Vector2.Distance(transform.position, player.position) <= shootingRange;
    }

    IEnumerator ShootAtPlayer()
    {
        isShooting = true;
        animator.SetBool("IsShooting", true); // Ativa a animação de tiro

        // Calcula a direção do jogador
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Cria o projétil na posição do ponto de disparo
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Aplica velocidade ao projétil na direção do jogador
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("O projétil precisa de um Rigidbody2D!");
        }

        // Espera a animação terminar
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isShooting = false; // Reseta o estado de tiro
        animator.SetBool("IsShooting", false); // Desativa a animação de tiro
    }
}
