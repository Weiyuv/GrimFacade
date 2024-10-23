using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlx : MonoBehaviour
{
    public Animator anima; // Referência ao Animator do personagem.
    public Rigidbody2D rdb; // Referência ao Rigidbody2D do personagem.
    bool doublejump; // Flag para controle de pulo duplo.
    public ParticleSystem fire; // Sistema de partículas para o efeito de fogo.

    // Parâmetros de velocidade.
    public float moveSpeed = 5.0f; // Velocidade de movimento horizontal.
    public float jumpForce = 5.0f; // Força aplicada para o pulo.

    private float groundCheckDistance = 0.1f; // Distância do raycast para verificar o chão.

    void Start()
    {
        // Inicializações.
    }

    void Update()
    {
        // Captura o movimento horizontal do jogador.
        float xmov = Input.GetAxis("Horizontal");

        // Define a velocidade horizontal constante.
        rdb.velocity = new Vector2(xmov * moveSpeed, rdb.velocity.y);

        // Faz um raycast para baixo para detectar o chão.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);
        bool isGrounded = hit.collider != null;

        // Verifica se o botão de pulo foi pressionado.
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded) // Se está no chão.
            {
                Jump();
            }
            else if (doublejump) // Se está no ar e pode fazer um pulo duplo.
            {
                DoubleJump();
            }
        }

        // Reseta o pulo duplo quando o personagem está no chão.
        if (isGrounded)
        {
            doublejump = true; // Permite pulo duplo se estiver no chão.
            anima.SetBool("isJumping", false); // Reseta a animação de pulo.
        }
        else
        {
            anima.SetBool("isJumping", true); // Ativa a animação de pulo se não estiver no chão.
        }

        // Desativa o estado de "Fire" no Animator.
        anima.SetBool("Fire", false);

        // Efeito de fogo.
        if (Input.GetButtonDown("Fire1"))
        {
            fire.Emit(1);
            anima.SetBool("Fire", true);
        }

        PhisicalReverser(); // Inverte o personagem.
        anima.SetFloat("Velocity", Mathf.Abs(xmov)); // Define a velocidade no Animator.
    }

    private void Jump()
    {
        // Aplica força de pulo.
        rdb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        doublejump = true; // Permite pulo duplo.
    }

    private void DoubleJump()
    {
        // Aplica força de pulo duplo.
        rdb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        doublejump = false; // Desabilita o pulo duplo após uso.
    }

    void PhisicalReverser()
    {
        if (rdb.velocity.x > 0.1f) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < -0.1f) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Detecção de colisão com objetos.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Aqui você pode adicionar lógica para colisões, se necessário.
    }
}
