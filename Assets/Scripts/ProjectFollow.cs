using UnityEngine;

public class ProjectileFollow : MonoBehaviour
{
    public float speed = 10f;  // Velocidade do projétil
    public Transform player;   // Referência ao jogador
    public float maxDistance = 20f;  // Distância máxima antes do projétil desaparecer (opcional)

    private void Update()
    {
        if (player != null)
        {
            // Calcular a direção em que o projétil deve se mover
            Vector2 direction = (player.position - transform.position).normalized;

            // Mover o projétil em direção ao jogador
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Se o projétil atingir a distância máxima, destruí-lo
            if (Vector2.Distance(transform.position, player.position) > maxDistance)
            {
                Destroy(gameObject);  // Destruir o projétil
            }
        }
    }
}
