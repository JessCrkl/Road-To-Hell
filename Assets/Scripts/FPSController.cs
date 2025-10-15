using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{

    [Header("Movement Parameters")]
    public float maxSpeed = 3.0f;

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
        
    }

} // end of class
