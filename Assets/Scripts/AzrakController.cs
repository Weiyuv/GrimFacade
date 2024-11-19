using UnityEngine;

public class AzrakController : MonoBehaviour
{
    public enum BossState { Idle, Flying, MeleeAttack, RangeAttack }
    public BossState currentState;

    public float flySpeed = 3f;
    public float meleeRange = 2f;
    public float rangeAttackCooldown = 3f;
    private float nextRangeAttackTime = 0f;

    private Transform player;
    private Animator animator;

    void Start()
    {
        // Referência ao jogador e ao Animator
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();

        // Inicializa o Boss no estado Idle
        currentState = BossState.Idle;
    }

    void Update()
    {
        // Comportamento do Boss
        switch (currentState)
        {
            case BossState.Idle:
                IdleBehavior();
                break;

            case BossState.Flying:
                FlyBehavior();
                break;

            case BossState.MeleeAttack:
                MeleeAttackBehavior();
                break;

            case BossState.RangeAttack:
                RangeAttackBehavior();
                break;
        }

        // Trocar de estado dependendo da situação
        UpdateState();
    }

    void IdleBehavior()
    {
        // Aqui o Boss fica parado
        animator.SetBool("IsIdle", true);
    }

    void FlyBehavior()
    {
        // Movimenta o Boss para o jogador
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * flySpeed * Time.deltaTime);

        animator.SetBool("IsFlying", true);
    }

    void MeleeAttackBehavior()
    {
        // Realiza um ataque corpo a corpo (melee)
        animator.SetTrigger("MeleeAttack");

        // Quando o ataque for feito, o Boss volta ao estado Idle
        currentState = BossState.Idle;
    }

    void RangeAttackBehavior()
    {
        // Lança um projétil ou realiza um ataque à distância
        if (Time.time >= nextRangeAttackTime)
        {
            animator.SetTrigger("RangeAttack");
            nextRangeAttackTime = Time.time + rangeAttackCooldown;
        }
    }

    void UpdateState()
    {
        // Aqui você pode decidir as condições para mudar de estado
        if (currentState == BossState.Idle && Vector3.Distance(transform.position, player.position) < 5f)
        {
            currentState = BossState.Flying;  // Mudar para voo se o jogador estiver perto
        }

        if (currentState == BossState.Flying && Vector3.Distance(transform.position, player.position) < meleeRange)
        {
            currentState = BossState.MeleeAttack;  // Se o jogador estiver dentro do alcance do ataque corpo a corpo
        }

        if (currentState == BossState.Flying && Vector3.Distance(transform.position, player.position) > meleeRange)
        {
            currentState = BossState.RangeAttack;  // Se o jogador estiver fora do alcance do melee, ataca à distância
        }
    }
}
