using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject target;  // Referência ao jogador (alvo)
    [SerializeField]
    private float smoothSpeed = 0.125f;  // Velocidade da suavização
    [SerializeField]
    private Vector3 offset;  // Distância entre a câmera e o jogador

    void LateUpdate()
    {
        // Verifica se há um alvo (jogador)
        if (target != null)
        {
            // Calcula a posição desejada da câmera
            Vector3 desiredPosition = target.transform.position + offset;

            // Suaviza o movimento da câmera para seguir o jogador
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Garantir que a posição Z da câmera seja fixa para que ela "olhe" a cena 2D
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10f);
        }
    }

    /// <summary>
    /// Seta o jogador na câmera
    /// </summary>
    /// <param name="tgt">Jogador a ser seguido</param>
    public void SetPlayer(GameObject tgt)
    {
        target = tgt;
    }
}
