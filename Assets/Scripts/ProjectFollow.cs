using UnityEngine;

public class ProjectileFollow : MonoBehaviour
{
    public float speed = 10f;  // Velocidade do proj�til
    public Transform player;   // Refer�ncia ao jogador
    public float maxDistance = 20f;  // Dist�ncia m�xima antes do proj�til desaparecer (opcional)

    private void Update()
    {
        if (player != null)
        {
            // Calcular a dire��o em que o proj�til deve se mover
            Vector2 direction = (player.position - transform.position).normalized;

            // Mover o proj�til em dire��o ao jogador
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Se o proj�til atingir a dist�ncia m�xima, destru�-lo
            if (Vector2.Distance(transform.position, player.position) > maxDistance)
            {
                Destroy(gameObject);  // Destruir o proj�til
            }
        }
    }
}
