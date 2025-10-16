using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FPSController))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] FPSController FPSController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Input Handling
    void OnMove(InputValue value)
    {
        FPSController.moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        FPSController.lookInput = value.Get<Vector2>();
    }

    // Unity Methods
    void OnValidate()
    {
        if (FPSController == null)
        {
            FPSController = GetComponent<FPSController>();
        }
    }

} // eoc
