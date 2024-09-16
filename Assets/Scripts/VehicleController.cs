using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;

    private bool isMounted = false;
    private GameObject player;
    private float horizontalInput;

    void Update()
    {
        if (isMounted)
        {
            // Captura a entrada horizontal
            horizontalInput = Input.GetAxis("Horizontal");
            
            // Movimento do veículo
            transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);
            
            // Atualiza parâmetros do Animator
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput)); // Ajusta a velocidade de movimento
            
            // Ajusta a direção com base na entrada horizontal
            if (horizontalInput > 0)
            {
                // Movimento para a direita
                // Inverte o sprite horizontalmente, mas mantém o tamanho
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (horizontalInput < 0)
            {
                // Movimento para a esquerda
                // Mantém a orientação original
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            
            animator.SetBool("IsMounted", true);
        }
        else
        {
            animator.SetFloat("Speed", 0f); // Define Speed como 0 quando não montado
            animator.SetBool("IsMounted", false);
        }
    }

    public void Mount(GameObject player)
    {
        this.player = player;
        isMounted = true;
        player.SetActive(false); // Oculta o jogador ao montar
        animator.SetBool("IsMounted", true);
    }

    public void Dismount()
    {
        isMounted = false;
        if (player != null)
        {
            player.SetActive(true); // Mostra o jogador ao desmontar
        }
        animator.SetBool("IsMounted", false);
        player.transform.position = transform.position;
    }

    public bool IsMounted
    {
        get { return isMounted; }
    }
}
