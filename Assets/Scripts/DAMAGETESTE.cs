using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAMAGETESTE : MonoBehaviour
{
    [SerializeField]
    private float lives = 4f;  // Agora lives � um float, permitindo valores decimais.

    private float initialLives;
    [SerializeField]
    ParticleSystem smoke;
    [SerializeField]
    ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        initialLives = lives;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        StartCoroutine(Blink());

        lives--; // Diminui o valor de 'lives' a cada colis�o de part�cula.

        if (lives < initialLives / 2f) // Verifica se as vidas ca�ram para menos da metade.
        {
            CreateandPlay(smoke);
        }

        if (lives <= 0f) // Quando 'lives' chega a 0 ou menos, destr�i o objeto.
        {
            CreateandPlay(explosion);

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (!renderer)
            {
                renderer = GetComponentInChildren<SpriteRenderer>();
            }
            renderer.enabled = false;
            Destroy(gameObject, 0.8f); // Espera 0.8 segundos antes de destruir o objeto.
        }
    }

    /// <summary>
    /// Cria uma part�cula e inicia sua anima��o.
    /// </summary>
    /// <param name="particle">Refer�ncia da part�cula (prefab) a ser instanciada.</param>
    void CreateandPlay(ParticleSystem particle)
    {
        if (particle)
        {
            GameObject ob = Instantiate(particle.gameObject, transform.position, Quaternion.identity);
            ob.transform.parent = gameObject.transform;
            ob.GetComponent<ParticleSystem>().Play();
        }
    }

    IEnumerator Blink()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            renderer = GetComponentInChildren<SpriteRenderer>();
        }
        for (int i = 0; i < 5; i++)
        {
            renderer.color = new Color(1, 0, 0); // Piscando com a cor vermelha.
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color(1, 1, 1); // De volta � cor original.
            yield return new WaitForSeconds(0.1f);
        }
    }
}
