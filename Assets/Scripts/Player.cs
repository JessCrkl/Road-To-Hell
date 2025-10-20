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
        
        if(dialogueManager != null && dialogueManager.DialogueActive)
        {
            // disable player movement during dialogue
            if (playerInput.currentActionMap.name != "Dialogue")
            {
                playerInput.SwitchCurrentActionMap("Dialogue");
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else {
            //re-enable player movement if dialogue finished
            if (playerInput.currentActionMap.name != "Player")
            {
                playerInput.SwitchCurrentActionMap("Player");
            }
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

    void OnInteract(InputValue value)
    {
        // TBD - pick up lost verses/gas canisters or open doors or smtg
        if (!value.isPressed) return;
        
        // TO DO: Create Interactable interface
        // Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        // if (Physics.Raycast(ray, out RaycastHit hit, 3f)) 
        // {
        //     Interactable interactable = hit.collider.GetComponent<Interactable>();
        //     if (interactable != null)
        //     {
        //         interactable.Interact();
        //     }
        // }
    }

    void OnContinueDialogue(InputValue value)
    {
        if (!value.isPressed) return;

        
        if (dialogueManager != null && dialogueManager.DialogueActive)
        {
            Debug.Log("ContinueDialogue pressed");
            dialogueManager.ContinueStory();
        }
    }

     void OnStartDialogue(InputValue value)
    {
        if (!value.isPressed) return;

        if(dialogueEventHandler != null && !dialogueManager.DialogueActive)
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
