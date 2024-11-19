using UnityEngine;

public class AzrakController : MonoBehaviour
{
    public float speed = 5f;  // Velocidade de voo
    public float attackRange = 5f;  // Dist�ncia de ataque de longo alcance
    public float attackNearRange = 2f; // Dist�ncia de ataque de curto alcance
    public float chaseRange = 10f; // Dist�ncia para ativar a persegui��o
    public float attackCooldown = 1f;  // Intervalo entre ataques
    public Transform player;  // Refer�ncia ao player
    public Transform attackPointNear;  // Ponto de ataque de curto alcance
    public Transform attackPointFar;  // Ponto de ataque de longo alcance
    public GameObject projectilePrefab;  // Projeto de ataque de longo alcance
    public Animator animator;  // Refer�ncia ao Animator

    private float lastAttackTime = 0f;

    private void Update()
    {
        // Atualizar a posi��o do Azrak e detectar a dist�ncia para o player
        MoveAzrak();
        Attack();
    }

    private void MoveAzrak()
    {
        // Calcular a dist�ncia at� o player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Verificar se o player est� dentro do alcance de persegui��o
        if (distanceToPlayer <= chaseRange)
        {
            // Mover o Azrak em dire��o ao player, fazendo ele "voar" de forma simples
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Atualizar anima��o de movimento (andar)
            animator.SetFloat("Speed", direction.magnitude);  // Supondo que tenha uma anima��o de voo ou andar

            // Rotacionar o Azrak para olhar para o jogador (invertido)
            RotateAzrak(direction);
        }
        else
        {
            // Parar o movimento caso o player esteja fora do alcance de persegui��o
            animator.SetFloat("Speed", 0);  // Parar anima��o
        }
    }

    private void RotateAzrak(Vector2 direction)
    {
        // Verificar se o jogador est� � esquerda ou � direita do Azrak
        if (direction.x > 0) // Jogador est� � esquerda (inverter l�gica)
        {
            // Inverter a escala no eixo X (virar o sprite para a esquerda)
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x < 0) // Jogador est� � direita (inverter l�gica)
        {
            // Voltar � escala normal (virar o sprite para a direita)
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;  // Verifica o cooldown do ataque

        // Checar a dist�ncia entre o Azrak e o player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackNearRange)
        {
            // Ataque de curto alcance
            animator.SetTrigger("AzrakAtacando");
            // Execute o ataque de curto alcance (ex: dano direto ao player ou anima��o de ataque)
            AttackNear();
        }
        else if (distanceToPlayer <= attackRange)
        {
            // Ataque de longo alcance
            animator.SetTrigger("AzrakRange");
            // Execute o ataque de longo alcance (ex: proj�til)
            AttackFar();
        }
    }

    private void AttackNear()
    {
        // L�gica de ataque de curto alcance
        // Aqui voc� pode dar dano direto ao player ou fazer alguma a��o f�sica
        Debug.Log("Azrak atacando de perto!");

        // Exemplo: Verificar colis�o com o player (voc� pode usar um Collider2D para detectar isso)
        // Implementar o dano direto ou anima��o de impacto
        lastAttackTime = Time.time;  // Resetar o cooldown
    }

    private void AttackFar()
    {
        // L�gica de ataque de longo alcance (ex: disparar proj�til)
        Debug.Log("Azrak atacando � dist�ncia!");

        // Instanciar um proj�til
        Instantiate(projectilePrefab, attackPointFar.position, Quaternion.identity);

        lastAttackTime = Time.time;  // Resetar o cooldown
    }
}
