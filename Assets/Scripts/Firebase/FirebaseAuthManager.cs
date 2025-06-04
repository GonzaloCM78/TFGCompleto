using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions; // ¡IMPORTANTE!
using System.Threading.Tasks;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuthManager Instance;

    private FirebaseAuth auth;
    private FirebaseUser user;

    void Awake()
    {
        Instance = this;
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Auth inicializado correctamente.");
            }
            else
            {
                Debug.LogError($"No se pudieron resolver dependencias de Firebase: {dependencyStatus}");
            }
        });
    }

    public void RegistrarUsuario(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Registro cancelado.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Error en el registro: " + task.Exception?.Flatten().InnerException?.Message);
                return;
            }

            user = task.Result.User;
            Debug.Log("Registro exitoso: " + user.Email);
        });
    }

    public void IniciarSesion(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Login cancelado.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Error en el login: " + task.Exception?.Flatten().InnerException?.Message);
                return;
            }

            user = task.Result.User;
            Debug.Log("Login exitoso: " + user.Email);
        });
    }
}
