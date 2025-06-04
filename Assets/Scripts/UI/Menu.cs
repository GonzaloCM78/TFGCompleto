using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject opcionesPanel;

    private bool isGamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (opcionesPanel.activeSelf)
            {
                CerrarOpciones(); // Si está en opciones, vuelve al menú de pausa
            }
            else
            {
                isGamePaused = !isGamePaused;
                TogglePausa();
            }
        }
    }

    public void TogglePausa()
    {
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            opcionesPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Reanudar()
    {
        isGamePaused = false;
        TogglePausa();
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AbrirOpciones()
    {
        pausePanel.SetActive(false);
        opcionesPanel.SetActive(true);
    }

    public void CerrarOpciones()
    {
        opcionesPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // Asegúrate de que se llama así
    }
}
