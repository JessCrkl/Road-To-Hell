using UnityEngine;

public abstract class CollectableItem : MonoBehaviour
{
    [Header("Collectible Settings")]
    public string itemName;
    public int amount = 1;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotifyCollection();
        }
    }

    protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Player"))
        {
            NotifyCollection();
        }
    }

    private void NotifyCollection()
    {
        GameEventsManager.Instance.ItemCollected(itemName);
        Collect();
        Destroy(gameObject);
    }
    

    protected abstract void Collect();
}
