using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles pointer events on slots.
/// <para>Emmanuella Dasilva-Domingos</para>
/// <para>Last Updated: July 17, 2024</para>
/// </summary>
public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
}

