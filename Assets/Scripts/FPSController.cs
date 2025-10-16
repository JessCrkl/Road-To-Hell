using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;
using UnityEngine.Scripting.APIUpdating;
using System;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{

    [Header("Movement Parameters")]
    public float maxSpeed = 3.0f;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveFPSCamera();
        LookFPSCamera();
    }

    // void onValidate()
    // {
    //     if (characterController == null)
    //     {
    //         characterController = GetComponent<CharacterController>();
    //     }
    // }

    // Update Camera Position
    void MoveFPSCamera()
    {
        Vector3 motion = transform.forward * moveInput.y + transform.right * moveInput.x;
        motion.y = 0f;
        motion.Normalize();

        characterController.Move(motion * maxSpeed * Time.deltaTime);
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
