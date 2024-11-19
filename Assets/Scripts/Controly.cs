using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controly : MonoBehaviour
{
    public Animator anima; // Refer�ncia ao Animator do personagem.
    public Rigidbody2D rdb; // Refer�ncia ao Rigidbody2D do personagem.
    public ParticleSystem fire; // Sistema de part�culas para o efeito de fogo.

    [Header("Velocidades")]
    public float moveSpeed = 20f; // Velocidade de movimento horizontal.
    public float jumpForce = 10f; // For�a do pulo.
    public float doubleJumpForce = 8f; // For�a do pulo duplo.
    public float sideJumpForce = 5f; // For�a do pulo lateral.

    [Header("Sa�de e Dano")]
    public float maxHealth = 100f; // A sa�de m�xima do personagem.
    private float currentHealth; // A sa�de atual do personagem.
    public float damageAmount = 20f; // A quantidade de dano sofrido.
    public float invincibilityTime = 1f; // Tempo de invencibilidade ap�s sofrer dano.

    private bool isInvincible = false; // Flag de invencibilidade para prevenir m�ltiplos danos seguidos.

    // Flags de controle de movimento.
    float xmov;
    bool jump, doublejump, jumpagain;
    float jumptime, jumptimeside;

    void Start()
    {
        currentHealth = maxHealth; // Inicializa a sa�de.
        jumpagain = true;
    }

    void Update()
    {
        // Captura o movimento horizontal do jogador.
        xmov = Input.GetAxis("Horizontal");

        // Verifica se o bot�o de pulo foi pressionado e controla o pulo duplo.
        if (Input.GetButtonDown("Jump"))
        {
            doublejump = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            jumpagain = true;
        }

        if (Input.GetButton("Jump") && jumpagain)
        {
            jump = true;
        }
        else
        {
            jump = false;
            doublejump = false;
            jumptime = 0;
            jumptimeside = 0;
        }

        // Desativa o estado de "Fire" no Animator.
        anima.SetBool("Fire", false);

        if (Input.GetButtonDown("Fire1"))
        {
            fire.Emit(1);
            anima.SetBool("Fire", true);
        }
    }

    void FixedUpdate()
    {
        PhisicalReverser(); // Inverte o personagem.
        anima.SetFloat("Velocity", Mathf.Abs(xmov)); // Atualiza a velocidade no Animator.

        // Movimenta��o horizontal.
        if (jumptimeside < 0.1f)
        {
            rdb.AddForce(new Vector2(xmov * moveSpeed / (rdb.velocity.magnitude + 1), 0));
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        if (hit)
        {
            anima.SetFloat("Height", hit.distance);
            if (jumptimeside < 0.1)
                JumpRoutine(hit);
        }

        RaycastHit2D hitright = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, transform.right, 1);
        if (hitright)
        {
            if (hitright.distance < 0.3f && hit.distance > 0.5f)
            {
                JumpRoutineSide(hitright);
            }
            Debug.DrawLine(hitright.point, transform.position + Vector3.up * 0.5f);
        }
    }

    private void JumpRoutine(RaycastHit2D hit)
    {
        if (hit.distance < 0.1f)
        {
            jumptime = jumpForce;
        }

        if (jump)
        {
            jumptime = Mathf.Lerp(jumptime, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce(Vector2.up * jumptime, ForceMode2D.Impulse);
            if (rdb.velocity.y < 0)
            {
                jumpagain = false;
            }
        }
    }

    private void JumpRoutineSide(RaycastHit2D hitside)
    {
        if (hitside.distance < 0.3f)
        {
            jumptimeside = sideJumpForce;
        }

        if (doublejump)
        {
            jumptimeside = Mathf.Lerp(jumptimeside, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce((hitside.normal + Vector2.up) * jumptimeside, ForceMode2D.Impulse);
        }
    }

    void Reverser()
    {
        if (rdb.velocity.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void PhisicalReverser()
    {
        if (rdb.velocity.x > 0.1f) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < -0.1f) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Fun��o para detectar colis�es e aplicar dano.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible) return; // Se o personagem est� invenc�vel, n�o recebe dano.

        if (collision.collider.CompareTag("Damage") || collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(damageAmount); // Chama a fun��o para aplicar dano.
        }
    }

    // Fun��o para reduzir a sa�de do personagem.
    private void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Garante que a sa�de n�o seja menor que 0.

        // Exibir algum feedback visual ou de �udio para o dano, se necess�rio.
        anima.SetTrigger("Hurt"); // Trigger para anima��o de dano (crie uma anima��o de dano no Animator).

        if (currentHealth <= 0)
        {
            Die(); // Chama a fun��o de morte, se a sa�de chegar a zero.
        }

        // Ativar invencibilidade tempor�ria.
        StartCoroutine(InvincibilityCoroutine());
    }

    // Fun��o para lidar com a morte do personagem.
    private void Die()
    {
        anima.SetTrigger("Die"); // Anima��o de morte (crie uma anima��o no Animator).
        // Pode adicionar mais l�gica aqui, como reiniciar o n�vel ou mostrar tela de Game Over.
        Destroy(gameObject); // Remove o personagem da cena, ou voc� pode desativar o objeto dependendo do seu jogo.
    }

    // Corrotina que gerencia o tempo de invencibilidade ap�s o dano.
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
}
