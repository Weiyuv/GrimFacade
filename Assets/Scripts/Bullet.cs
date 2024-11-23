using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Detecção de colisão
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroi o projétil ao colidir com qualquer objeto
        Destroy(gameObject);
    }
}
