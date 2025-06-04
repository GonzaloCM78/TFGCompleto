using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("Login UI")]
    public GameObject loginPanel;
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;
    public TextMeshProUGUI loginMessageText;

    [Header("Register UI")]
    public GameObject registerPanel;
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerPasswordInput;
    public TextMeshProUGUI registerMessageText;

    [Header("URLs")]
    private const string loginUrl = "http://localhost:8080/api/v1/auth/login";
    private const string registerUrl = "http://localhost:8080/api/v1/auth/register";

    [Header("Música y Sonido")]
    public AudioSource audioSource;
    public AudioClip clip;
    public AudioClip botonSonido;

    void Start()
    {
        // Reproduce la canción si está asignada
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void OnLoginButton()
    {
        ReproducirSonidoBoton();

        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;
        StartCoroutine(Login(username, password));
    }

    public void OnRegisterButton()
    {
        ReproducirSonidoBoton();

        string name = registerUsernameInput.text;
        string email = registerEmailInput.text;
        string password = registerPasswordInput.text;
        StartCoroutine(Register(name, email, password));
    }

    public void ShowRegisterPanel()
    {
        ReproducirSonidoBoton();

        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        ClearMessages();
    }

    public void ShowLoginPanel()
    {
        ReproducirSonidoBoton();

        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
        ClearMessages();
    }

    private void ClearMessages()
    {
        loginMessageText.text = "";
        registerMessageText.text = "";
    }

    private void ReproducirSonidoBoton()
    {
        if (audioSource != null && botonSonido != null)
        {
            audioSource.PlayOneShot(botonSonido);
        }
    }

    IEnumerator Login(string username, string password)
    {
        var json = JsonUtility.ToJson(new LoginRequest { name = username, password = password });
        var request = new UnityWebRequest(loginUrl, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            loginMessageText.text = "Login correcto";
            Debug.Log("Respuesta: " + request.downloadHandler.text);

            PlayerPrefs.SetString("username", username);
            SceneManager.LoadScene("Menu");
        }
        else
        {
            loginMessageText.text = "Error al iniciar sesión";
            Debug.LogError("Error de login: " + request.error);
        }
    }

    IEnumerator Register(string name, string email, string password)
    {
        var json = JsonUtility.ToJson(new RegisterRequest { name = name, email = email, password = password });
        var request = new UnityWebRequest(registerUrl, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            registerMessageText.text = "Registro exitoso";
            Debug.Log("Registro exitoso: " + request.downloadHandler.text);
            ShowLoginPanel();
        }
        else
        {
            registerMessageText.text = "Error al registrar";
            Debug.LogError("Error de registro: " + request.error);
        }
    }

    [System.Serializable]
    public class LoginRequest
    {
        public string name;
        public string password;
    }

    [System.Serializable]
    public class RegisterRequest
    {
        public string name;
        public string email;
        public string password;
    }
}
