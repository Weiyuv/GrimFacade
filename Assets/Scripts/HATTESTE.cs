using UnityEngine;

public class HATTESTE : MonoBehaviour
{
    public Transform player; // O Transform do jogador
    public float speed = 2f; // Velocidade de movimento do inimigo
    public float stoppingDistance = 1f; // Distância para parar de se mover em direção ao jogador
    public float followDistance = 5f; // Distância máxima de seguimento antes do inimigo parar de seguir

    void Update()
    {
        if (player != null)
        {
            // Calcular a distância entre o inimigo e o jogador
            float distance = Vector2.Distance(transform.position, player.position);

            // Se o jogador estiver dentro do alcance de seguimento
            if (distance > stoppingDistance && distance <= followDistance)
            {
                // Calcular a direção para o jogador
                Vector2 direction = (player.position - transform.position).normalized;

                // Mover o inimigo em direção ao jogador
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            // Caso o inimigo esteja muito perto ou muito longe, ele para
            else
            {
                // O inimigo não se move
                // Você pode adicionar aqui uma animação de "espera" ou outro comportamento, se necessário.
            }
        }
    }
}
