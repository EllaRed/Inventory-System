using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform slotTransform;
    private Image image;
    private GameObject copy;
    private Item item;
    private Slot slot;

    public Item Item { get => item; set => item = value; }
    public Slot Slot { get => slot; set => slot = value; }

    private void Start(){

        image = GetComponent<Image>();
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        try
        {
            slot = GetComponentInParent<Slot>();
            item = slot.CurrentItem;
            
            slotTransform = transform.parent;
            copy = new GameObject("Copy");
            copy.AddComponent<Image>();
            copy.GetComponent<RectTransform>().sizeDelta = image.rectTransform.sizeDelta;
            copy.GetComponent<Image>().sprite = image.sprite;
            copy.transform.SetParent(transform.root);
            copy.transform.SetAsLastSibling();
            copy.GetComponent<Image>().raycastTarget = false;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(slotTransform.gameObject.GetComponent<Slot>().CurrentItem == null)
            return;
        
        copy.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(copy);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}

