using UnityEngine;

public class Hat : MonoBehaviour
{
    public Transform player; // O Transform do jogador
    public float speed = 2f; // Velocidade do inimigo
    public float stoppingDistance = 1f; // Distância para parar de se mover em direção ao jogador

    void Update()
    {
        if (player != null)
        {
            // Calcular a distância entre o inimigo e o jogador
            float distance = Vector2.Distance(transform.position, player.position);

            // Se o jogador estiver fora da distância de parada, o inimigo se move em direção a ele
            if (distance > stoppingDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized; // Direção para o jogador
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime); // Mover o inimigo
            }
        }
    }
}
