using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackRange = 3f;
    public float shootRange = 10f;
    public float attackCooldown = 1f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float modeSwitchCooldown = 5f;
    public float rangedDistance = 5f;

    private float lastAttackTime = 0f;
    private float lastModeSwitchTime = 0f;
    private Transform player;
    private Animator animator;

    private bool isMovingToPlayer = false;
    private bool isMeleeMode = true;

    // Collider para o ataque corpo a corpo
    public Collider2D meleeAttackCollider;

    private float meleeAttackDuration = 0.5f;  // Duração do ataque corpo a corpo (em segundos)
    private float meleeAttackTimer = 0f;  // Temporizador para controlar a duração do ataque

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        // Certifique-se de que o Collider esteja desativado inicialmente
        if (meleeAttackCollider != null)
        {
            meleeAttackCollider.enabled = false;
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Alterna entre modos (melee / ranged) após o cooldown de troca de modo
        if (Time.time - lastModeSwitchTime >= modeSwitchCooldown)
        {
            isMeleeMode = !isMeleeMode;
            lastModeSwitchTime = Time.time;

            if (!isMeleeMode)
            {
                MoveAwayFromPlayer();
            }
        }

        // Se estiver no modo corpo a corpo (melee)
        if (isMeleeMode)
        {
            if (distanceToPlayer <= attackRange && !isMovingToPlayer)
            {
                MeleeAttack();
            }
            else
            {
                FlyTowardsPlayer();
            }
        }
        // Se estiver no modo à distância (ranged)
        else
        {
            if (distanceToPlayer <= shootRange && !isMovingToPlayer)
            {
                RangedAttack();
            }
            else
            {
                FlyTowardsPlayer();
            }
        }

        if (isMovingToPlayer && distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer();
        }

        // Gerenciar a duração do ataque corpo a corpo
        if (meleeAttackTimer > 0)
        {
            meleeAttackTimer -= Time.deltaTime;
            if (meleeAttackTimer <= 0 && meleeAttackCollider != null)
            {
                meleeAttackCollider.enabled = false;  // Desativa o collider após a duração do ataque
            }
        }
    }

    private void MeleeAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            isMovingToPlayer = true;
            animator.SetBool("IsMoving", true);
            lastAttackTime = Time.time;

            // Ativa o Collider durante a animação do ataque corpo a corpo
            if (meleeAttackCollider != null)
            {
                meleeAttackCollider.enabled = true;
            }

            // Inicia o temporizador para o ataque corpo a corpo
            meleeAttackTimer = meleeAttackDuration;

            // Inicia a animação do ataque corpo a corpo
            animator.SetTrigger("AttackMelee");
        }
    }

    private void RangedAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            animator.SetTrigger("AttackRanged");
            lastAttackTime = Time.time;

            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Vector2 direction = (player.position - shootPoint.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f;

            Debug.Log("Ranged Attack!");
        }
    }

    private void FlyTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        RotateTowardsPlayer(direction);
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", direction.magnitude);
        animator.SetBool("IsFlying", true);
        Debug.Log("Flying towards player");
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        RotateTowardsPlayer(direction);
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", 1f);

        // Quando o boss chega ao jogador e está dentro do alcance de ataque
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            // Realiza o ataque corpo a corpo
            animator.SetTrigger("AttackMelee");
            lastAttackTime = Time.time;

            // Lógica para dano corpo a corpo pode ser implementada aqui
            Debug.Log("Melee Attack!");

            // Após o ataque, o boss para de se mover
            isMovingToPlayer = false;
            animator.SetBool("IsMoving", false);
        }
    }

    private void RotateTowardsPlayer(Vector2 direction)
    {
        if (direction.x > 0)
        {
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void MoveAwayFromPlayer()
    {
        Vector2 directionAway = (transform.position - player.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)directionAway * rangedDistance, moveSpeed * Time.deltaTime);
        animator.SetBool("IsFlying", true);
        Debug.Log("Moving away from player!");
    }
}
