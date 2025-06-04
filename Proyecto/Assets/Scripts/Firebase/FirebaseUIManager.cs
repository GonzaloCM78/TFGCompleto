using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirebaseUIManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelLogin;
    public GameObject panelRegistro;

    [Header("Login UI")]
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;
    public Button loginButton;
    public Button goToRegisterButton;

    [Header("Registro UI")]
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerPasswordInput;
    public Button registerButton;
    public Button goToLoginButton;

    [Header("Estado")]
    public TextMeshProUGUI statusText;

    void Start()
    {
        ShowLogin();

        loginButton.onClick.AddListener(OnLogin);
        registerButton.onClick.AddListener(OnRegister);
        goToRegisterButton.onClick.AddListener(ShowRegister);
        goToLoginButton.onClick.AddListener(ShowLogin);
    }

    public void ShowLogin()
    {
        panelLogin.SetActive(true);
        panelRegistro.SetActive(false);
        statusText.text = "";
    }

    public void ShowRegister()
    {
        panelLogin.SetActive(false);
        panelRegistro.SetActive(true);
        statusText.text = "";
    }

    public void OnLogin()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        statusText.text = "Iniciando sesión...";
        FirebaseAuthManager.Instance.IniciarSesion(email, password);
    }

    public void OnRegister()
    {
        string email = registerEmailInput.text;
        string password = registerPasswordInput.text;

        statusText.text = "Registrando usuario...";
        FirebaseAuthManager.Instance.RegistrarUsuario(email, password);
    }
}
