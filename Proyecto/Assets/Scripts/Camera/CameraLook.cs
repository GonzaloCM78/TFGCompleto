using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensivity = 80f;
    public Transform playerBody;

    public float xRotation = 0;

    void Start()
    {
        //  Carga la sensibilidad desde PlayerPrefs
        mouseSensivity = PlayerPrefs.GetFloat("Sensibilidad", mouseSensivity);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    public void SetSensitivity(float value)
    {
        mouseSensivity = value;
    }
}
