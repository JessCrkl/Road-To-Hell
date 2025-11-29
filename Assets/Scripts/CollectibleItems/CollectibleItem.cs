using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CollectableItem : MonoBehaviour
{
    [Header("Collectible Settings")]
    public string itemName;
    public int amount = 1;
    private bool playerInRange = false;

    [Header("Interaction Prompt")]
    public GameObject interactionPrompt;

    private void Update()
    {
        if (!playerInRange) return;

        bool ePressed = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
        bool leftClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (ePressed || leftClick)
        {
            NotifyCollection();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionPrompt != null) {
                interactionPrompt.SetActive(true);  
            }
        }   
             
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionPrompt != null) {
                interactionPrompt.SetActive(false);  
            }
        }
    }

    private void NotifyCollection()
    {
        if (interactionPrompt != null) {
            interactionPrompt.SetActive(false);  
        }
        GameEventsManager.Instance.ItemCollected(itemName);
        Collect();
        Destroy(gameObject);
    }
    

    protected abstract void Collect();
}
