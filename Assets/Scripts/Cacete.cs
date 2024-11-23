using UnityEngine;

public class Cacete : MonoBehaviour
{
    public float moveSpeed = 3f; // Velocidade de movimento do inimigo
    public float attackRange = 1.5f; // Distância para o ataque melee
    public float attackCooldown = 1f; // Tempo entre ataques
    public float visionRange = 5f; // Distância máxima de visão (usado no lugar do collider)
    public Collider2D attackCollider; // Referência ao collider de ataque

    private Transform player;
    private bool isPlayerInVision = false; // O jogador está no alcance de visão?
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private Animator animator;

    void Start()
    {
        // Encontrar o jogador pela tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Obtém o componente Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator não encontrado no inimigo!");
        }

        // Certifique-se de que o collider de ataque esteja desativado inicialmente
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    void Update()
    {
        if (player == null) return; // Certificar que o jogador existe

        // Verificar se o jogador está dentro do alcance de visão
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= visionRange)
        {
            isPlayerInVision = true;
        }
        else
        {
            isPlayerInVision = false;
            SetAnimationState("isMoving", false); // Voltar para o estado Idle
        }

        if (isPlayerInVision)
        {
            // Se o jogador estiver dentro do alcance de visão
            if (distanceToPlayer <= attackRange)
            {
                // Parar de se mover e atacar
                if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
                {
                    StartCoroutine(Attack());
                }
                SetAnimationState("isMoving", false);
            }
            else
            {
                // Mover em direção ao jogador
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Atualizar o estado de movimento na animação
        SetAnimationState("isMoving", true);

        // Virar o sprite para a direção do jogador
        if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    System.Collections.IEnumerator Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // Ativa a animação de ataque
        SetAnimationTrigger("Attack");

        // Ativa o collider de ataque
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }

        // Aguarda o tempo da animação (sem aplicar dano)
        yield return new WaitForSeconds(0.2f);

        // Desativa o collider de ataque após a animação
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }

        // Aguarda antes de permitir outro ataque
        yield return new WaitForSeconds(attackCooldown - 0.2f); // Garante cooldown após o ataque
        isAttacking = false;
    }

    // Método para definir estados de animação
    void SetAnimationState(string parameter, bool state)
    {
        if (animator != null && animator.GetBool(parameter) != state)
        {
            animator.SetBool(parameter, state);
        }
    }

    // Método para ativar triggers de animação
    void SetAnimationTrigger(string parameter)
    {
        if (animator != null)
        {
            animator.SetTrigger(parameter);
        }
    }

    // Colisão de ataque - verifique se o jogador é atingido
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplica o efeito de dano ou outro efeito desejado
            Debug.Log("Jogador atingido pelo ataque!");
            // Exemplo: PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            // if (playerHealth != null) playerHealth.TakeDamage(10);
        }
    }
}
