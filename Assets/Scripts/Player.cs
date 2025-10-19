using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FPSController))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] FPSController FPSController;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] DialogueEventHandler dialogueEventHandler;
    [SerializeField] PlayerInput playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerInput.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Move Input: {FPSController.moveInput}, DialogueActive: {dialogueManager.DialogueActive}");
        
        if(dialogueManager != null && dialogueManager.DialogueActive) // disable player movement during dialogue
        {
            if (playerInput.enabled)
            {
                playerInput.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

        } else if(dialogueManager != null && !dialogueManager.DialogueActive && !playerInput.enabled) {
            //re-enable player movement if dialogue finished
            playerInput.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    #region Input Handling
    void OnMove(InputValue value)
    {
        FPSController.moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        FPSController.lookInput = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        FPSController.sprintInput = value.isPressed;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            FPSController.Jump();
        }
    }

    void OnContinueDialogue(InputValue value)
    {
        if (value.isPressed && dialogueManager != null && dialogueManager.DialogueActive)
        {
            dialogueManager.ContinueStory();
        }
    }

     void OnStartDialogue(InputValue value)
    {
        if(value.isPressed && dialogueEventHandler != null)
        {
            dialogueEventHandler.TryStartDialogue();
        }
    }

    #endregion

    #region Unity Methods
    void OnValidate()
    {
        if (FPSController == null)
        {
            FPSController = GetComponent<FPSController>();
        }

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
    }
    #endregion

} // eoc
