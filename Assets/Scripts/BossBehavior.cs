using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;
    public float meleeRange = 2f;
    public float rangeMinDistance = 5f;
    public float rangeMaxDistance = 10f;
    public float attackCooldown = 3f; // Tempo entre ataques
    public GameObject rangeProjectile;
    public Transform projectileSpawnPoint;

    private Animator animator;
    private float nextAttackTime;
    private bool isNextAttackMelee = true; // Alterna entre ataques melee e range
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Verifica se o boss pode atacar (cooldown)
        if (Time.time >= nextAttackTime && !isAttacking)
        {
            if (isNextAttackMelee && distanceToPlayer <= meleeRange)
            {
                // Realiza ataque melee
                MeleeAttack();
            }
            else if (!isNextAttackMelee && distanceToPlayer >= rangeMinDistance && distanceToPlayer <= rangeMaxDistance)
            {
                // Realiza ataque à distância
                RangeAttack();
            }
            else
            {
                // Movimenta o boss enquanto espera o próximo ataque
                AdjustPosition(distanceToPlayer);
            }
        }
        else
        {
            // Movimenta o boss enquanto espera o cooldown
            AdjustPosition(distanceToPlayer);
        }
    }

    void MeleeAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttackingMelee", true);
        animator.SetBool("isFlying", false);
        animator.SetBool("isIdle", false);

        // Alterna para próximo ataque ser à distância
        isNextAttackMelee = false;
        nextAttackTime = Time.time + attackCooldown;

        // Chama EndAttack após um tempo (ajustar o tempo conforme a animação)
        Invoke("EndAttack", 1f); // 1 segundo é apenas um exemplo; ajuste conforme necessário
    }

    void RangeAttack()
    {
        isAttacking = true;
        animator.SetTrigger("isAttackingRange");
        animator.SetBool("isFlying", false);
        animator.SetBool("isIdle", false);

        // Alterna para próximo ataque ser melee
        isNextAttackMelee = true;
        nextAttackTime = Time.time + attackCooldown;

        // Cria o projétil
        PerformRangeAttack();
    }

    void AdjustPosition(float distanceToPlayer)
    {
        Vector2 direction = Vector2.zero;

        if (isNextAttackMelee && distanceToPlayer > meleeRange)
        {
            // Aproximar-se do jogador para ataque melee
            direction = (player.position - transform.position).normalized;
        }
        else if (!isNextAttackMelee && distanceToPlayer < rangeMinDistance)
        {
            // Afastar-se do jogador para manter a distância mínima
            direction = (transform.position - player.position).normalized;
        }
        else if (!isNextAttackMelee && distanceToPlayer > rangeMaxDistance)
        {
            // Aproximar-se do jogador para manter a distância máxima
            direction = (player.position - transform.position).normalized;
        }
        else
        {
            // Boss está na posição ideal para atacar
            animator.SetBool("isIdle", true);
            return; // Não movimenta se estiver na posição ideal para atacar
        }

        animator.SetBool("isFlying", true);
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    public void EndAttack()
    {
        // Método chamado ao final de cada ataque para voltar ao voo ou idle
        isAttacking = false;
        animator.SetBool("isAttackingMelee", false);
        animator.SetBool("isFlying", true);
        animator.SetBool("isIdle", true); // Garante que o boss volte ao estado idle se não estiver atacando
    }

    public void PerformRangeAttack()
    {
        // Cria o projétil
        Instantiate(rangeProjectile, projectileSpawnPoint.position, Quaternion.identity);
    }
}
