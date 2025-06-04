using UnityEngine;
using UnityEngine.UI;

public class OpcionesManager : MonoBehaviour
{
    public Slider sensibilidadSlider;
    public Slider volumenSlider;

    public CameraLook camaraJugador;
    public AudioSource[] audioSources;

    private bool valoresCargados = false;

    void Start()
    {
        float sensibilidad = PlayerPrefs.GetFloat("Sensibilidad", 80f);
        float volumen = PlayerPrefs.GetFloat("Volumen", 1f);

        sensibilidadSlider.value = sensibilidad;
        volumenSlider.value = volumen;

        AplicarSensibilidad(sensibilidad);
        AplicarVolumen(volumen);

        valoresCargados = true;
    }

    public void OnSensibilidadCambiada(float nuevaSensibilidad)
    {
        if (!valoresCargados) return; // Previene llamadas antes de inicializar
        AplicarSensibilidad(nuevaSensibilidad);
        PlayerPrefs.SetFloat("Sensibilidad", nuevaSensibilidad);
        PlayerPrefs.Save();
    }

    public void OnVolumenCambiado(float nuevoVolumen)
    {
        if (!valoresCargados) return;
        AplicarVolumen(nuevoVolumen);
        PlayerPrefs.SetFloat("Volumen", nuevoVolumen);
        PlayerPrefs.Save();
    }

    void AplicarSensibilidad(float valor)
    {
        if (camaraJugador != null)
            camaraJugador.SetSensitivity(valor);
    }

    void AplicarVolumen(float valor)
    {
        foreach (AudioSource src in audioSources)
        {
            src.volume = valor;
        }
    }
}
