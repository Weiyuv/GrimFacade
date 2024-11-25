using UnityEngine;
using UnityEngine.SceneManagement;  // Necessário para gerenciar cenas

public class Escape : MonoBehaviour
{
    // O nome ou índice da cena do menu principal
    public string mainMenuSceneName = "Main menu"; 

    void Update()
    {
        // Verifica se a tecla 'Esc' foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Carrega a cena do menu principal
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
