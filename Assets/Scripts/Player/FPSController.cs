using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;
using System;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    #region Variable Definitions
    [Header("Movement Parameters")]
    [SerializeField] float maxSpeed => sprintInput ? sprintSpeed : walkSpeed;
    [SerializeField] float acceleration = 80f;
    [SerializeField] float deceleration = 100f;
    [Tooltip("This is how fast the character can walk.")]
    [SerializeField] float walkSpeed = 4.5f;
    [Tooltip("This is how fast the character can run.")]
    [SerializeField] float sprintSpeed = 15f;

    [Space(15)]
    [Tooltip("This is how high the character can jump.")]
    [SerializeField] float jumpHeight = 2f;
    private int timesJumped = 0;
    [SerializeField] bool canDoubleJump = true;

    public bool sprinting
    {
        get
        {
            return sprintInput && currentSpeed > 0.1f;
        }
    }

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

    [Header("Camera Parameters")]
    [SerializeField] float cameraNormalFOV = 60f;
    [SerializeField] float cameraSprintFOV = 100f;
    [SerializeField] float cameraFOVSmoothing = 0.5f;

    float targetCameraFOV
    {
        get
        {
            return sprinting ? cameraSprintFOV : cameraNormalFOV;
        }
    }

    [Header("Physics Parameters")]
    [SerializeField] float gravityScale = 3f;

    public float verticalVelocity = 0f;
    public Vector3 currentVelocity { get; private set; }
    public float currentSpeed { get; private set; }

    public bool wasGrounded = false;
    public bool isGrounded => characterController.isGrounded;

    [Header("Dialogue Parameters")]
    [SerializeField] DialogueManager dialogueManager;

    [Header("State Flags")]
    public bool SongLearningActive = false;

    [Header("FPS Input")]
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool sprintInput;

    [Header("FPS Components")]
    [SerializeField] CinemachineCamera FPSCamera;
    [SerializeField] CharacterController characterController;

    [Header("Events")]
    public UnityEvent Landed;
    #endregion

    #region Unity Methods
    
    void Update()
    {
        //Debug.Log($"dialogueManager: {dialogueManager != null}, DialogueActive: {(dialogueManager != null && dialogueManager.DialogueActive)}, moveInput: {moveInput}");
        // // don't move if dialogue is active
        if (dialogueManager != null && dialogueManager.DialogueActive)
        {
            return;
        }

        if (SongLearningActive)
        {
            return;
        }
        
        MoveFPSCamera();
        LookFPSCamera();
        CameraUpdate();

        if (!wasGrounded && isGrounded)
        {
            timesJumped = 0;
            Landed?.Invoke();
        }
        
        wasGrounded = isGrounded;
    }

    void OnValidate()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }
    #endregion

    #region Controller Methods

    public void Jump()
    {
        if (isGrounded == false)
        {
            if(canDoubleJump && timesJumped < 2 && verticalVelocity > 0.01f)
            {
                return;
            }

            if (!canDoubleJump || timesJumped >= 2)
            {
                return;
            }

        } 

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * gravityScale);
        timesJumped++;
    }

    // Update Camera Position
    void MoveFPSCamera()
    {
        //Debug.Log(1f / Time.deltaTime);
        Vector3 motion = transform.forward * moveInput.y + transform.right * moveInput.x;
        motion.y = 0f;
        motion.Normalize();

        float accel = acceleration * Time.deltaTime;
        float decel = deceleration * Time.deltaTime;

        // smooth acceleration
        if (motion.sqrMagnitude >= 0.01f)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, motion * maxSpeed, accel);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, decel);
        }
        
        // account for gravity
        if (isGrounded && verticalVelocity <= 0.01f)
        {
            currentVelocity *= 0.95f;
        } else
        {
            verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
        }

        Vector3 fullVelocity = new Vector3(currentVelocity.x, verticalVelocity, currentVelocity.z);
        //Debug.Log(fullVelocity);

        // move character
        CollisionFlags flags = characterController.Move(fullVelocity * Time.deltaTime);

        // account for hitting things above character
        if ((flags & CollisionFlags.Above) != 0 && verticalVelocity > 0.01f)
        {
            verticalVelocity = 0f;
        }

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

    void CameraUpdate()
    {
        float targetFOV = cameraNormalFOV;

        if (sprinting)
        {
            float speedRatio = currentSpeed / sprintSpeed;
            targetFOV = Mathf.Lerp(cameraNormalFOV, cameraSprintFOV, speedRatio);
        }

        FPSCamera.Lens.FieldOfView = Mathf.Lerp(FPSCamera.Lens.FieldOfView, targetFOV, cameraFOVSmoothing * Time.deltaTime);
    }
    #endregion
} // end of class
