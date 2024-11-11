using UnityEngine;

public class HATETESTE : MonoBehaviour
{
    public Transform player; // O Transform do jogador
    public float speed = 2f; // Velocidade do inimigo
    public float stoppingDistance = 1f; // Dist�ncia para parar de se mover em dire��o ao jogador
    public float minFollowDistance = 3f; // Dist�ncia m�nima para come�ar a seguir o jogador
    public float maxFollowDistance = 10f; // Dist�ncia m�xima para seguir o jogador

    void Update()
    {
        if (player != null)
        {
            // Calcular a dist�ncia entre o inimigo e o jogador
            float distance = Vector2.Distance(transform.position, player.position);

            // Se o jogador estiver fora do range de seguir (fora do intervalo de dist�ncia definido)
            if (distance < minFollowDistance || distance > maxFollowDistance)
            {
                // Se estiver fora do range, o inimigo n�o se move e fica parado
                return;
            }

            // Se o jogador estiver dentro do range (entre minFollowDistance e maxFollowDistance)
            if (distance > stoppingDistance)
            {
                // Se o jogador estiver mais distante que a dist�ncia de parada, o inimigo se move em dire��o a ele
                Vector2 direction = (player.position - transform.position).normalized; // Dire��o para o jogador
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime); // Mover o inimigo
            }
        }
    }
}
