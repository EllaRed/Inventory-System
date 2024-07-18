using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles pointer events on slots.
/// <para>Emmanuella Dasilva-Domingos</para>
/// <para>Last Updated: July 17, 2024</para>
/// </summary>
public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDropHandler
{
    public Slot slot;
    public AudioSource clickSound;
    private float lastClickTime = 0f; // Field to store the timestamp of the last click
    private const float doubleClickThreshold = 0.3f; // Threshold in seconds

    public void OnPointerEnter(PointerEventData eventData)
    {
        slot?.OnHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Optionally handle pointer exit
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                slot?.OnLeftClick();
                if (clickSound != null)
                {
                    clickSound.Play();
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                slot?.OnRightClick();
                if (clickSound != null)
                {
                    clickSound.Play();
                }
            }
        }
        lastClickTime = Time.time;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<DraggableItem>().Item != null)
        {
            DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();
            Item item  = draggedItem.Item;
            Debug.Log("item stack: "+ item.CurrentStackSize);
            Slot formerSlot = draggedItem.Slot;
            if(formerSlot==slot)
                return;
            slot.AddItem(item,formerSlot);
                

            
         
        }
    }
}

