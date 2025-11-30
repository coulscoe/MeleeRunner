using Unity.Cinemachine;
using UnityEngine;

public class FPCameraHandler : MonoBehaviour
{

    [Header("Camera Settings")]
    public float pitchLimit;
    public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);

    [Header("Components")]
    [SerializeField] private CinemachineCamera fpCamera;
    [SerializeField] private PlayerController playerController;

    [Header("Info")]
    public float currentPitch;

    public void LookUpdate(Vector2 lookInput)
    {
        Vector2 input = new Vector2(lookInput.x * lookSensitivity.x * Time.deltaTime, lookInput.y * lookSensitivity.y * Time.deltaTime);

        // Update current pitch
        currentPitch = Mathf.Clamp(currentPitch - input.y, -pitchLimit, pitchLimit);
        fpCamera.transform.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);

        playerController.transform.Rotate(Vector3.up * input.x);
    }
}
