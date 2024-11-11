using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlx : MonoBehaviour
{
    public Animator anima; // Refer�ncia ao Animator do personagem.
    float xmov; // Vari�vel para guardar o movimento horizontal.
    public Rigidbody2D rdb; // Refer�ncia ao Rigidbody2D do personagem.
    bool jump, doublejump, jumpagain; // Flags para controle de pulo e pulo duplo.
    float jumptime, jumptimeside; // Controla a dura��o dos pulos.
    public ParticleSystem fire; // Sistema de part�culas para o efeito de fogo.

    // Vari�veis p�blicas para controlar as velocidades
    [Header("Velocidades")]
    public float moveSpeed = 20f; // Velocidade de movimento horizontal.
    public float jumpForce = 10f; // For�a do pulo.
    public float doubleJumpForce = 8f; // For�a do pulo duplo.
    public float sideJumpForce = 5f; // For�a do pulo lateral.

    void Start()
    {
        // M�todo para inicializa��es. 
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

        // Define o estado de pulo com base na entrada do usu�rio.
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

        // Ativa o efeito de fogo e define o estado "Fire" no Animator quando o bot�o de fogo � pressionado.
        if (Input.GetButtonDown("Fire1"))
        {
            fire.Emit(1);
            anima.SetBool("Fire", true);
        }
    }

    void FixedUpdate()
    {
        PhisicalReverser(); // Chama a fun��o que inverte o personagem.
        anima.SetFloat("Velocity", Mathf.Abs(xmov)); // Define a velocidade no Animator.

        // Adiciona uma for�a para mover o personagem com base na velocidade configurada.
        if (jumptimeside < 0.1f)
        {
            rdb.AddForce(new Vector2(xmov * moveSpeed / (rdb.velocity.magnitude + 1), 0));
        }

        RaycastHit2D hit;

        // Faz um raycast para baixo para detectar o ch�o.
        hit = Physics2D.Raycast(transform.position, Vector2.down);
        if (hit)
        {
            anima.SetFloat("Height", hit.distance);
            if (jumptimeside < 0.1)
                JumpRoutine(hit); // Chama a rotina de pulo.
        }

        RaycastHit2D hitright;

        // Faz um raycast para a direita para detectar paredes.
        hitright = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, transform.right, 1);
        if (hitright)
        {
            if (hitright.distance < 0.3f && hit.distance > 0.5f)
            {
                JumpRoutineSide(hitright); // Chama a rotina de pulo lateral.
            }
            Debug.DrawLine(hitright.point, transform.position + Vector3.up * 0.5f);
        }
    }

    // Rotina de pulo (parte f�sica).
    private void JumpRoutine(RaycastHit2D hit)
    {
        // Verifica a dist�ncia do ch�o e aplica uma for�a de pulo se necess�rio.
        if (hit.distance < 0.1f)
        {
            jumptime = jumpForce; // Ajuste com a vari�vel de velocidade do pulo.
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

    // Rotina de pulo lateral.
    private void JumpRoutineSide(RaycastHit2D hitside)
    {
        if (hitside.distance < 0.3f)
        {
            jumptimeside = sideJumpForce; // Ajuste com a vari�vel de velocidade do pulo lateral.
        }

        if (doublejump)
        {
            jumptimeside = Mathf.Lerp(jumptimeside, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce((hitside.normal + Vector2.up) * jumptimeside, ForceMode2D.Impulse);
        }
    }

    // Fun��o para inverter a dire��o do personagem (visual).
    void Reverser()
    {
        if (rdb.velocity.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
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