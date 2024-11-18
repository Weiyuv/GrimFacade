using UnityEngine;

public class Projec : MonoBehaviour
{
    // Colisão com outro objeto
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o projétil colidiu com um inimigo ou o jogador
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            // Aqui você pode adicionar algum efeito visual ou sonoro, se necessário
            Destroy(gameObject); // Destroi o projétil
        }
    }

    // Caso esteja usando triggers (caso o Collider do projétil seja um trigger)
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") || collider.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroi o projétil
        }
    }
}
