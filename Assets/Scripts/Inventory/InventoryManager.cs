using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the allocation of items to slots in the inventory system.
/// <para> Emmanuella Dasilva-Domingos</para>
/// <para> Last Updated: July 17, 2024</para>
/// </summary>
public class InventoryManager : MonoBehaviour
{
	public GameObject equipContainer;
	public GameObject hotkeysContainer; 
    public GameObject itemContainer;

	public List<GameObject> equipSlotList = new List<GameObject>();
	public List<GameObject> hotkeysSlotList = new List<GameObject>();
    public List<GameObject> itemSlotList = new List<GameObject>();

	private void Start()
	{
		// Populate list containers for specialized slots
		foreach (Transform child in equipContainer.transform)
		{
			equipSlotList.Add(child.gameObject);
		}

		foreach (Transform child in hotkeysContainer.transform)
		{
			hotkeysSlotList.Add(child.gameObject);
		}
	}

    /// <summary>
    /// Initializes special slots.
    /// </summary>
    public void InitializeSlots()
    {
        
        foreach (GameObject slot in equipSlotList)
        {
            slot.AddComponent<EquipmentSlot>();
            SlotUI slotUI= slot.AddComponent<SlotUI>();
            slotUI.slot = slot.GetComponent<EquipmentSlot>();

        }

        foreach (GameObject slot in hotkeysSlotList)
        {
            HotKeySlot hotKey = slot.AddComponent<HotKeySlot>();
            hotKey.AssignedNumber = hotkeysSlotList.IndexOf(slot)+1;
            SlotUI slotUI = slot.AddComponent<SlotUI>();
            slotUI.slot = slot.GetComponent<HotKeySlot>();
        }
    }

    /// <summary>
    /// Adds an item to the appropriate slot in the inventory.
    /// </summary>
    public bool AddItem(Item item)
    {
        // Check for stackable items in hotkey slots
        if (item.IsStackable && TryStackItem(item, hotkeysSlotList))
        {
            return true;
        }

        // Check for stackable items in item slots
        if (item.IsStackable && TryStackItem(item, itemSlotList))
        {
            return true;
        }

        // Check for available item slots
        if (TryAddToEmptySlot(item, itemSlotList))
        {
            return true;
        }

        // Check for charm slots
        switch (item.ItemType)
        {
            case ItemType.HeadEquip:
                if (TryAddToEmptySlot(item, new List<GameObject> { equipSlotList[0] }))
                {
                    return true;
                }
                break;
            case ItemType.TopEquip:
                if (TryAddToEmptySlot(item, new List<GameObject> { equipSlotList[1], equipSlotList[2] }))
                {
                    return true;
                }
                break;
            case ItemType.BottomEquip:
                if (TryAddToEmptySlot(item, new List<GameObject> { equipSlotList[3], equipSlotList[4] }))
                {
                    return true;
                }
                break;
        }

        // If no slot is available, return false
        return false;
    }

    private bool TryStackItem(Item item, List<GameObject> slots)
    {
        foreach (GameObject slotObject in slots)
        {
            Slot slot = slotObject.GetComponent<Slot>();
            if (slot.CurrentItem != null && slot.CurrentItem.ItemName == item.ItemName && slot.StackCount < slot.StackLimit)
            {
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }

    private bool TryAddToEmptySlot(Item item, List<GameObject> slots)
    {
        foreach (GameObject slotObject in slots)
        {
            Slot slot = slotObject.GetComponent<Slot>();
            if (!slot.IsOccupied)
            {
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }

     public bool RemoveItem(Item item)
    {
        // Check hotkey slots for item removal
        if (TryRemoveFromSlot(item, hotkeysSlotList))
        {
            return true;
        }

        // Check item slots for item removal
        if (TryRemoveFromSlot(item, itemSlotList))
        {
            return true;
        }

        // Check charm slots for item removal
        switch (item.ItemType)
        {
            case ItemType.HeadEquip:
                if (TryRemoveFromSlot(item, new List<GameObject> { equipSlotList[0] }))
                {
                    return true;
                }
                break;
            case ItemType.TopEquip:
                if (TryRemoveFromSlot(item, new List<GameObject> { equipSlotList[1], equipSlotList[2] }))
                {
                    return true;
                }
                break;
            case ItemType.BottomEquip:
                if (TryRemoveFromSlot(item, new List<GameObject> { equipSlotList[3], equipSlotList[4] }))
                {
                    return true;
                }
                break;
        }

        // If the item was not found in any slot, return false
        return false;
    }

    private bool TryRemoveFromSlot(Item item, List<GameObject> slots)
    {
        foreach (GameObject slotObject in slots)
        {
            Slot slot = slotObject.GetComponent<Slot>();
            if (slot.CurrentItem != null && slot.CurrentItem.ItemName == item.ItemName)
            {
                slot.RemoveItem();
                return true;
            }
        }
        return false;
    }

    public void UseHotkey(int hotkeyNumber)
    {
        //I would like UseItem to remain private because I'm obssessed with encapsulation rn
        hotkeysSlotList[hotkeyNumber].GetComponent<HotKeySlot>().OnLeftClick();
        
    }
}
