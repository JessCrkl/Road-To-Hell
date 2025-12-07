using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FragmentDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SongFragment fragment;
    private CanvasGroup group;
    private Transform originalParent;
    private RectTransform rect;
    public Image icon;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        icon = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); 
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = true;

        if (eventData.pointerEnter != null)
        {
            StaffSlot slot = eventData.pointerEnter.GetComponentInParent<StaffSlot>();

            if (slot != null)
            {
                if (slot.CorrectPlacement(fragment))
                {
                    // correct
                    slot.PlaceFragment(fragment);
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    // TBD: wrong so shake animation 
                    Debug.Log("Wrong answer! Puzzle reseting...");
                }
            }
        }

        // reset position if not placed
        transform.SetParent(originalParent);
    }

    public void BindData()
    {
        icon.sprite = fragment.sheetSprite;
    }
}
