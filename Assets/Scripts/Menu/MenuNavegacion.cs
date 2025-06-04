using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuNavegacion : MonoBehaviour
{
    [Header("Paneles de UI")]
    public GameObject panelMenuPrincipal;
    public GameObject panelSeleccionMapa;
    public GameObject panelOpciones;
    public GameObject fadePanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip musicaFondo;

    private void Start()
    {
        // Reproducir música de fondo al iniciar el menú
        if (musicaFondo != null && audioSource != null)
        {
            audioSource.clip = musicaFondo;
            audioSource.loop = true;
            audioSource.Play();

            // Uncomment si quieres que la música persista entre escenas
            // DontDestroyOnLoad(audioSource.gameObject);
        }
    }

    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    // ----------------------------
    // NAVEGACIÓN ENTRE PANELES
    // ----------------------------

    public void IrASeleccionMapa()
    {
        PlayClickSound();
        panelMenuPrincipal.SetActive(false);
        panelSeleccionMapa.SetActive(true);
    }

    public void VolverAlMenuPrincipalDesdeSeleccionMapa()
    {
        PlayClickSound();
        panelSeleccionMapa.SetActive(false);
        panelMenuPrincipal.SetActive(true);
    }

    public void AbrirOpciones()
    {
        PlayClickSound();
        panelMenuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        PlayClickSound();
        panelOpciones.SetActive(false);
        panelMenuPrincipal.SetActive(true);
    }

    // ----------------------------
    // CAMBIO DE ESCENAS
    // ----------------------------

    public void IniciarJuego()
    {
        PlayClickSound();
        StartCoroutine(FadeAndLoadScene("SampleScene"));
    }

    public void VolverAlMenu()
    {
        PlayClickSound();
        StartCoroutine(FadeAndLoadScene("Menu"));
    }

    public void Reiniciar()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadePanel != null)
            fadePanel.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(sceneName);
    }

    // ----------------------------
    // SALIR DEL JUEGO
    // ----------------------------

    public void Salir()
    {
        PlayClickSound();
        Application.Quit();
    }
}
