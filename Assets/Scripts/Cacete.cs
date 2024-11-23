using UnityEngine;

public class Cacete : MonoBehaviour
{
    public float moveSpeed = 3f; // Velocidade de movimento do inimigo
    public float attackRange = 1.5f; // Distância para o ataque melee
    public float attackCooldown = 1f; // Tempo entre ataques
    public float visionWidth = 5f; // Largura da área de visão (retangular)
    public float visionHeight = 2f; // Altura da área de visão (retangular)
    public int damage = 10; // Dano do ataque

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

        // Adiciona o BoxCollider2D para o campo de visão (área retangular)
        BoxCollider2D visionCollider = gameObject.AddComponent<BoxCollider2D>();
        visionCollider.isTrigger = true;  // Isso garante que o collider é apenas um gatilho e não causa colisões físicas
        visionCollider.size = new Vector2(visionWidth, visionHeight); // Define o tamanho do campo de visão
        visionCollider.offset = new Vector2(0, 0); // Alinha o collider com o centro do inimigo
    }

    void Update()
    {
        if (player == null || !isPlayerInVision) return; // Certificar que o jogador está no alcance

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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

        // Aguarda o tempo da animação antes de aplicar dano (opcional)
        yield return new WaitForSeconds(0.2f);

        // Checar se o jogador está dentro do alcance e aplicar dano
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
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

    // Detecta o jogador entrando no campo de visão (aqui apenas detecta a entrada na área de visão)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInVision = true;
        }
    }

    // Detecta o jogador saindo do campo de visão
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInVision = false;
            SetAnimationState("isMoving", false); // Volta para Idle
        }
    }

    // Não será mais necessário detectar dano no collider, pois será feito pelo ataque
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Não faça nada aqui. O dano não será mais aplicado com base no collider.
    }
}
