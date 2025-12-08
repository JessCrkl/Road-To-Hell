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
    [SerializeField] PlayerStats playerHealth;


    [Header("Song Combat")]
    public bool isSongAttacking = false;
    public string cerberusSongName = "Cerberus Melody";
    public float songAttackRange = 5f;

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
        } else if(FPSController != null && FPSController.SongLearningActive)
        {
            // disable player movement during song learning
            if (playerInput.currentActionMap.name != "SongLearning")
            playerInput.SwitchCurrentActionMap("SongLearning");

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            //re-enable player movement if dialogue finished and not learning song
            if (playerInput.currentActionMap.name != "Player")
            {
                playerInput.SwitchCurrentActionMap("Player");
            }
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (isSongAttacking)
        {
            ApplySongDamageToCerberus();
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

        if (!value.isPressed) return;

        Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Cerberus cerberus = hit.collider.GetComponent<Cerberus>();
            if (cerberus != null)
            {
                cerberus.TryToAppease();
            }
        }
    }

    void OnSongAttack(InputValue value)
    {
        isSongAttacking = value.isPressed;
    }

    void OnPersuade(InputValue value)
    {
        if (!value.isPressed) return;

        TryToPersuade();
    }
    
    private void TryToPersuade()
{
    Collider[] hits = Physics.OverlapSphere(transform.position, 3f);
    foreach (var hit in hits)
    {
        Cerberus cerberus = hit.GetComponent<Cerberus>();
        if (cerberus != null)
        {
            cerberus.TryToAppease();
            return; 
        }

    }
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

        private void ApplySongDamageToCerberus()
    {
        // player must know the song
        if (!PlayerStats.Instance.HasLearnedSong(cerberusSongName))
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, songAttackRange);
        foreach (var hit in hits)
        {
            Cerberus cerb = hit.GetComponent<Cerberus>();
            if (cerb != null)
            {
                float damageAmount = cerb.songDamagePerSecond * Time.deltaTime;
                cerb.TakeSongDamage(damageAmount);
                return; 
            }
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
