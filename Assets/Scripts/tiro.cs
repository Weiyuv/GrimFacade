using UnityEngine;

public class tiro : MonoBehaviour
{
    public float speed = 10f;  // Velocidade do projétil
    public float lifetime = 5f; // Tempo de vida do projétil
    public int damage = 10; // Dano do projétil
    public Vector2 direction; // Direção do projétil

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroi o projétil após o tempo de vida
        // Certifica que o projétil será lançado na direção correta
        if (direction == Vector2.zero)
        {
            direction = Vector2.right; // Padrão se nenhuma direção for passada
        }
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime); // Move o projétil
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o projétil colidiu com o jogador
        if (collision.CompareTag("Player"))
        {
            // Obtém o componente PlayerHealth do player
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Aplica dano ao jogador
                playerHealth.TakeDamage(damage);
            }

            // Destroi o projétil após causar dano
            Destroy(gameObject);
        }
    }
}
