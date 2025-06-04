using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EnviarSesion : MonoBehaviour
{
    public static IEnumerator Enviar(string username, int ronda, int puntos, string mapa, string duracion)
    {
        string url = "http://localhost:8080/api/v1/session/save";

        var json = JsonUtility.ToJson(new GameSessionDTO
        {
            username = username,
            ronda = ronda,
            puntos = puntos,
            mapa = mapa,
            duracion = duracion
        });

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Sesión enviada correctamente");
        else
            Debug.LogError("Error al enviar sesión: " + request.error);
    }

    [System.Serializable]
    public class GameSessionDTO
    {
        public string username;
        public int ronda;
        public int puntos;
        public string mapa;
        public string duracion;
    }
}
