using UnityEngine;

public class AzrakController : MonoBehaviour
{
    public float speed = 5f;  // Velocidade de voo
    public float attackRange = 5f;  // Distância de ataque de longo alcance
    public float attackNearRange = 2f; // Distância de ataque de curto alcance
    public float chaseRange = 10f; // Distância para ativar a perseguição
    public float attackCooldown = 1f;  // Intervalo entre ataques
    public Transform player;  // Referência ao player
    public Transform attackPointNear;  // Ponto de ataque de curto alcance
    public Transform attackPointFar;  // Ponto de ataque de longo alcance
    public GameObject projectilePrefab;  // Projeto de ataque de longo alcance
    public Animator animator;  // Referência ao Animator

    private float lastAttackTime = 0f;

    private void Update()
    {
        // Atualizar a posição do Azrak e detectar a distância para o player
        MoveAzrak();
        Attack();
    }

    private void MoveAzrak()
    {
        // Calcular a distância até o player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Verificar se o player está dentro do alcance de perseguição
        if (distanceToPlayer <= chaseRange)
        {
            // Mover o Azrak em direção ao player, fazendo ele "voar" de forma simples
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Atualizar animação de movimento (andar)
            animator.SetFloat("Speed", direction.magnitude);  // Supondo que tenha uma animação de voo ou andar

            // Rotacionar o Azrak para olhar para o jogador (invertido)
            RotateAzrak(direction);
        }
        else
        {
            // Parar o movimento caso o player esteja fora do alcance de perseguição
            animator.SetFloat("Speed", 0);  // Parar animação
        }
    }

    private void RotateAzrak(Vector2 direction)
    {
        // Verificar se o jogador está à esquerda ou à direita do Azrak
        if (direction.x > 0) // Jogador está à esquerda (inverter lógica)
        {
            // Inverter a escala no eixo X (virar o sprite para a esquerda)
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x < 0) // Jogador está à direita (inverter lógica)
        {
            // Voltar à escala normal (virar o sprite para a direita)
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;  // Verifica o cooldown do ataque

        // Checar a distância entre o Azrak e o player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackNearRange)
        {
            // Ataque de curto alcance
            animator.SetTrigger("AzrakAtacando");
            // Execute o ataque de curto alcance (ex: dano direto ao player ou animação de ataque)
            AttackNear();
        }
        else if (distanceToPlayer <= attackRange)
        {
            // Ataque de longo alcance
            animator.SetTrigger("AzrakRange");
            // Execute o ataque de longo alcance (ex: projétil)
            AttackFar();
        }
    }

    private void AttackNear()
    {
        // Lógica de ataque de curto alcance
        // Aqui você pode dar dano direto ao player ou fazer alguma ação física
        Debug.Log("Azrak atacando de perto!");

        // Exemplo: Verificar colisão com o player (você pode usar um Collider2D para detectar isso)
        // Implementar o dano direto ou animação de impacto
        lastAttackTime = Time.time;  // Resetar o cooldown
    }

    private void AttackFar()
    {
        // Lógica de ataque de longo alcance (ex: disparar projétil)
        Debug.Log("Azrak atacando à distância!");

        // Instanciar um projétil
        Instantiate(projectilePrefab, attackPointFar.position, Quaternion.identity);

        lastAttackTime = Time.time;  // Resetar o cooldown
    }
}
