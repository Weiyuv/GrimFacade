using UnityEngine;

public class HATTESTE : MonoBehaviour
{
    public Transform player; // O Transform do jogador
    public float speed = 2f; // Velocidade de movimento do inimigo
    public float stoppingDistance = 1f; // Dist�ncia para parar de se mover em dire��o ao jogador
    public float followDistance = 5f; // Dist�ncia m�xima de seguimento antes do inimigo parar de seguir

    void Update()
    {
        if (player != null)
        {
            // Calcular a dist�ncia entre o inimigo e o jogador
            float distance = Vector2.Distance(transform.position, player.position);

            // Se o jogador estiver dentro do alcance de seguimento
            if (distance > stoppingDistance && distance <= followDistance)
            {
                // Calcular a dire��o para o jogador
                Vector2 direction = (player.position - transform.position).normalized;

                // Mover o inimigo em dire��o ao jogador
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            // Caso o inimigo esteja muito perto ou muito longe, ele para
            else
            {
                // O inimigo n�o se move
                // Voc� pode adicionar aqui uma anima��o de "espera" ou outro comportamento, se necess�rio.
            }
        }
    }
}
