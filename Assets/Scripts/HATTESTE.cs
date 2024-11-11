using UnityEngine;

public class HATETESTE : MonoBehaviour
{
    public Transform player; // O Transform do jogador
    public float speed = 2f; // Velocidade do inimigo
    public float stoppingDistance = 1f; // Distância para parar de se mover em direção ao jogador
    public float minFollowDistance = 3f; // Distância mínima para começar a seguir o jogador
    public float maxFollowDistance = 10f; // Distância máxima para seguir o jogador

    void Update()
    {
        if (player != null)
        {
            // Calcular a distância entre o inimigo e o jogador
            float distance = Vector2.Distance(transform.position, player.position);

            // Se o jogador estiver fora do range de seguir (fora do intervalo de distância definido)
            if (distance < minFollowDistance || distance > maxFollowDistance)
            {
                // Se estiver fora do range, o inimigo não se move e fica parado
                return;
            }

            // Se o jogador estiver dentro do range (entre minFollowDistance e maxFollowDistance)
            if (distance > stoppingDistance)
            {
                // Se o jogador estiver mais distante que a distância de parada, o inimigo se move em direção a ele
                Vector2 direction = (player.position - transform.position).normalized; // Direção para o jogador
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime); // Mover o inimigo
            }
        }
    }
}
