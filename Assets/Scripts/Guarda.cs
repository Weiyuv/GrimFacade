using System.Collections;
using UnityEngine;

public class Guarda : MonoBehaviour
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

                // Inverte o sprite do inimigo e o ponto de disparo
                if (direction.x < 0)
                {
                    // Vira para a esquerda
                    GetComponent<SpriteRenderer>().flipX = true;
                    firePoint.localRotation = Quaternion.Euler(0, 180, 0); // Gira o ponto de disparo
                }
                else
                {
                    // Vira para a direita
                    GetComponent<SpriteRenderer>().flipX = false;
                    firePoint.localRotation = Quaternion.Euler(0, 0, 0); // Gira o ponto de disparo
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

        // Inicia a animação de atirar
        animator.SetBool("IsShooting", true);

        // Aguarda o tempo da animação antes de disparar o projétil
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Cria o projétil na posição do ponto de disparo
        Vector2 direction = (player.position - firePoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Aplica a rotação dependendo da direção em que o inimigo está virado
        if (GetComponent<SpriteRenderer>().flipX)
        {
            // Inverte a direção do projétil se o inimigo estiver virado para a esquerda
            direction = new Vector2(-direction.x, direction.y);
        }

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

        // Desativa a animação de atirar após o disparo
        animator.SetBool("IsShooting", false);

        isShooting = false;
    }
}
