using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;
using UnityEngine.Scripting.APIUpdating;
using System;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{

    [Header("Movement Parameters")]
    public float maxSpeed = 5.0f;
    public float acceleration = 1.5f;
    public Vector3 currentVelocity { get; private set; }
    public float currentSpeed { get; private set; }

    [Header("Look Around Parameters")]
    public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
    public float pitchLimit = 85f;
    [SerializeField] float currentPitch = 0f;

    public float CurrentPitch
    {
        get => currentPitch;

        set
        {
            currentPitch = Math.Clamp(value, -pitchLimit, pitchLimit);
        }
    }

    [Header("FPS Input")]
    public Vector2 moveInput;
    public Vector2 lookInput;

    [Header("FPS Components")]
    [SerializeField] CinemachineCamera FPSCamera;
    [SerializeField] CharacterController characterController;

    // Update is called once per frame
    void Update()
    {
        MoveFPSCamera();
        LookFPSCamera();
    }

    void OnValidate()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    // Update Camera Position
    void MoveFPSCamera()
    {
        Vector3 motion = transform.forward * moveInput.y + transform.right * moveInput.x;
        motion.y = 0f;
        motion.Normalize();

        // smooth acceleration
        if (motion.sqrMagnitude >= 0.01f)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, motion * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, acceleration * Time.deltaTime);
        }

        // account for gravity
        float verticalVelocity = Physics.gravity.y * 15f * Time.deltaTime;
        Vector3 fullVelocity = new Vector3(currentVelocity.x, verticalVelocity, currentVelocity.z);

        // move character
        characterController.Move(fullVelocity * Time.deltaTime);

        //update speed
        currentSpeed = currentVelocity.magnitude;
    }

    // Update Camera orientation (what fps is looking at)
    void LookFPSCamera()
    {
        Vector2 input = new Vector2(lookInput.x * lookSensitivity.x, lookInput.y * lookSensitivity.y);

        // look up and down
        currentPitch -= input.y;
        FPSCamera.transform.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);

        //look left and right
        transform.Rotate(Vector3.up * input.x);
    }

} // end of class
