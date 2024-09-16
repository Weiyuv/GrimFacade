using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public VehicleController currentVehicle; // A referência ao veículo

    void Update()
    {
        // Verifica se o jogador pressionou a tecla "E" e se há um veículo disponível
        if (Input.GetKeyDown(KeyCode.E) && currentVehicle != null)
        {
            // Alterna entre montar e desmontar o veículo
            if (currentVehicle.IsMounted)
            {
                currentVehicle.Dismount();
            }
            else
            {
                currentVehicle.Mount(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto com o qual o jogador colidiu tem a tag "Vehicle"
        if (other.CompareTag("Vehicle"))
        {
            // Obtém o componente VehicleController do veículo e armazena na variável currentVehicle
            currentVehicle = other.GetComponent<VehicleController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Se o jogador sair do trigger do veículo, limpa a referência ao veículo
        if (other.CompareTag("Vehicle"))
        {
            if (currentVehicle != null && currentVehicle.gameObject == other.gameObject)
            {
                currentVehicle = null;
            }
        }
    }
}
