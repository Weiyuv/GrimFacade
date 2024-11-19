using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A: MonoBehaviour
{
    public Animator anima; // Referência ao Animator do personagem.
    float xmov; // Variável para guardar o movimento horizontal.
    public Rigidbody2D rdb; // Referência ao Rigidbody2D do personagem.
    bool jump; // Flag para controle de pulo.
    public ParticleSystem fire; // Sistema de partículas para o efeito de fogo.

    // Variáveis públicas para controlar as velocidades
    [Header("Velocidades")]
    public float moveSpeed = 20f; // Velocidade de movimento horizontal.
    public float jumpForce = 10f; // Força do pulo.

    // Variáveis para controlar o fogo
    [Header("Fogo")]
    public int maxFireUses = 3; // Número máximo de disparos que podem ser feitos sem recarregar.
    private int currentFireUses; // Número atual de disparos restantes (cargas).
    public float fireCooldown = 5f; // Tempo de cooldown para recarregar os disparos.
    private float fireCooldownTimer; // Temporizador para o cooldown de recarga.

    void Start()
    {
        // Inicializa o número de disparos restantes e o cooldown
        currentFireUses = maxFireUses;
        fireCooldownTimer = 0f; // Começa o cooldown em 0.
    }

    void Update()
    {
        // Captura o movimento horizontal do jogador.
        xmov = Input.GetAxis("Horizontal");

        // Verifica se o botão de pulo foi pressionado.
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        // Atualiza o cooldown do fogo
        if (fireCooldownTimer > 0)
        {
            fireCooldownTimer -= Time.deltaTime; // Diminui o tempo do cooldown
        }

        // Desativa o estado de "Fire" no Animator
        anima.SetBool("Fire", false);

        // Verifica se o jogador pode disparar
        if (Input.GetButtonDown("Fire1") && currentFireUses > 0 && fireCooldownTimer <= 0)
        {
            // Emite o efeito de fogo
            fire.Emit(1);

            // Diminui o número de disparos restantes
            currentFireUses--;

            // Define o estado "Fire" no Animator
            anima.SetBool("Fire", true);

            // Se as cargas acabarem, inicia o cooldown
            if (currentFireUses == 0)
            {
                fireCooldownTimer = fireCooldown; // Inicia o cooldown.
            }
        }

        // Quando o cooldown terminar, recarrega as cargas
        if (fireCooldownTimer <= 0 && currentFireUses == 0)
        {
            currentFireUses = maxFireUses; // Recarrega as cargas para o valor máximo
        }
    }

    void FixedUpdate()
    {
        PhisicalReverser(); // Chama a função que inverte o personagem.
        anima.SetFloat("Velocity", Mathf.Abs(xmov)); // Define a velocidade no Animator.

        // Adiciona uma força para mover o personagem com base na velocidade configurada.
        rdb.AddForce(new Vector2(xmov * moveSpeed / (rdb.velocity.magnitude + 1), 0));

        RaycastHit2D hit;

        // Faz um raycast para baixo para detectar o chão.
        hit = Physics2D.Raycast(transform.position, Vector2.down);
        if (hit)
        {
            anima.SetFloat("Height", hit.distance);
            JumpRoutine(hit); // Chama a rotina de pulo.
        }
    }

    // Rotina de pulo (parte física).
    private void JumpRoutine(RaycastHit2D hit)
    {
        // Verifica se o personagem está no chão e pode pular.
        if (hit.distance < 0.1f && jump)
        {
            rdb.velocity = new Vector2(rdb.velocity.x, 0); // Zera a velocidade vertical para evitar aceleração indesejada no pulo
            rdb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplica a força de pulo
            jump = false; // Desativa o pulo após ser executado
        }
    }

    // Função para inverter a direção do personagem (física).
    void PhisicalReverser()
    {
        if (rdb.velocity.x > 0.1f) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < -0.1f) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Detecção de colisão com objetos marcados com a tag "Damage".
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage") || collision.collider.CompareTag("Enemy"))
        {
            LevelManager.instance.LowDamage(); // Chama a função para aplicar dano.
        }
    }
}
