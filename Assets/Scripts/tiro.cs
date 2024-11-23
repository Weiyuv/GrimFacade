using UnityEngine;

public class tiro : MonoBehaviour
{
    public float speed = 10f;  // Velocidade do projétil
    public float lifetime = 5f; // Tempo de vida do projétil
    public int damage = 10; // Dano do projétil

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroi o projétil após o tempo de vida
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // Move o projétil
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
