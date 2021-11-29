using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] float mouseSensitivityX = 250f;
    [SerializeField] float mouseSensitivityY = 250f;

    [Header("References")]
    [SerializeField] Transform playerBody;
    [SerializeField] Transform camera;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (GameManagerScript.isPlayerAlive && !GameManagerScript.isPlayerWin)
        {
            RotateFPSCamera();
        }
    }

    void RotateFPSCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
