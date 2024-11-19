using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A: MonoBehaviour
{
    public Animator anima; // Refer�ncia ao Animator do personagem.
    float xmov; // Vari�vel para guardar o movimento horizontal.
    public Rigidbody2D rdb; // Refer�ncia ao Rigidbody2D do personagem.
    bool jump; // Flag para controle de pulo.
    public ParticleSystem fire; // Sistema de part�culas para o efeito de fogo.

    // Vari�veis p�blicas para controlar as velocidades
    [Header("Velocidades")]
    public float moveSpeed = 20f; // Velocidade de movimento horizontal.
    public float jumpForce = 10f; // For�a do pulo.

    // Vari�veis para controlar o fogo
    [Header("Fogo")]
    public int maxFireUses = 3; // N�mero m�ximo de disparos que podem ser feitos sem recarregar.
    private int currentFireUses; // N�mero atual de disparos restantes (cargas).
    public float fireCooldown = 5f; // Tempo de cooldown para recarregar os disparos.
    private float fireCooldownTimer; // Temporizador para o cooldown de recarga.

    void Start()
    {
        // Inicializa o n�mero de disparos restantes e o cooldown
        currentFireUses = maxFireUses;
        fireCooldownTimer = 0f; // Come�a o cooldown em 0.
    }

    void Update()
    {
        // Captura o movimento horizontal do jogador.
        xmov = Input.GetAxis("Horizontal");

        // Verifica se o bot�o de pulo foi pressionado.
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

            // Diminui o n�mero de disparos restantes
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
            currentFireUses = maxFireUses; // Recarrega as cargas para o valor m�ximo
        }
    }

    void FixedUpdate()
    {
        PhisicalReverser(); // Chama a fun��o que inverte o personagem.
        anima.SetFloat("Velocity", Mathf.Abs(xmov)); // Define a velocidade no Animator.

        // Adiciona uma for�a para mover o personagem com base na velocidade configurada.
        rdb.AddForce(new Vector2(xmov * moveSpeed / (rdb.velocity.magnitude + 1), 0));

        RaycastHit2D hit;

        // Faz um raycast para baixo para detectar o ch�o.
        hit = Physics2D.Raycast(transform.position, Vector2.down);
        if (hit)
        {
            anima.SetFloat("Height", hit.distance);
            JumpRoutine(hit); // Chama a rotina de pulo.
        }
    }

    // Rotina de pulo (parte f�sica).
    private void JumpRoutine(RaycastHit2D hit)
    {
        // Verifica se o personagem est� no ch�o e pode pular.
        if (hit.distance < 0.1f && jump)
        {
            rdb.velocity = new Vector2(rdb.velocity.x, 0); // Zera a velocidade vertical para evitar acelera��o indesejada no pulo
            rdb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplica a for�a de pulo
            jump = false; // Desativa o pulo ap�s ser executado
        }
    }

    // Fun��o para inverter a dire��o do personagem (f�sica).
    void PhisicalReverser()
    {
        if (rdb.velocity.x > 0.1f) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < -0.1f) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Detec��o de colis�o com objetos marcados com a tag "Damage".
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage") || collision.collider.CompareTag("Enemy"))
        {
            LevelManager.instance.LowDamage(); // Chama a fun��o para aplicar dano.
        }
    }
}
