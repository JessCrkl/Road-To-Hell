using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FragmentDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SongFragment fragment;
    public bool isPaletteSource = false;
    private CanvasGroup group;
    private Transform originalParent;
    private RectTransform rect;
    public Image icon;
     private Canvas rootCanvas;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        if (icon == null) {
            icon = GetComponent<Image>();}
        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        if (rootCanvas != null)
        {
            transform.SetParent(rootCanvas.transform);
            transform.SetAsLastSibling();      
        } 

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
                if (slot.IsEmpty)
                {
                    // place if slot isn't already filled
                    slot.PlaceFragment(fragment);

                    if (isPaletteSource && SongLearningUIManager.Instance != null && SongLearningUIManager.Instance.songLearningPanel.activeInHierarchy)
                    {
                        SongLearningUIManager.Instance.SpawnFragmentInPalette(fragment);
                    }
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    // slot filled
                    Debug.Log("This slot is filled!");
                }
            }
        }

        // reset position if not placed
        transform.SetParent(originalParent);
        rect.localScale = Vector3.one;
    }

    public void BindData()
    {
        if (icon == null)
        {
            Debug.LogError("FragmentDraggable has no Image assigned for icon.", this);
            return;
        }

        icon.sprite = fragment.sheetSprite;
        icon.color = Color.white;
    }
}
