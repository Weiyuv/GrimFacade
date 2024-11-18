using UnityEngine;

public class Projec : MonoBehaviour
{
    // Colis�o com outro objeto
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o proj�til colidiu com um inimigo ou o jogador
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            // Aqui voc� pode adicionar algum efeito visual ou sonoro, se necess�rio
            Destroy(gameObject); // Destroi o proj�til
        }
    }

    // Caso esteja usando triggers (caso o Collider do proj�til seja um trigger)
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") || collider.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroi o proj�til
        }
    }
}
