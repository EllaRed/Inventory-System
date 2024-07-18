using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the base class for slots, cells or containers in the inventory system.
/// <para>
/// Specialized slots that can only hold specific types of items can be created by inheriting from this class.
/// (See EquipmentSlot as an Example)
/// Slots can also be given unique behaviours such as assigning them to certain buttons.(See HotKeySlot)
/// </para>
/// Emmanuella Dasilva-Domingos
/// <para>Last Updated: July 17, 2024</para>
/// </summary>
public abstract class Slot : MonoBehaviour
{
    [SerializeField] private bool isOccupied = false;
    [SerializeField] private int stackCount = 0;
    [SerializeField] private int stackLimit = 1;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] protected Sprite emptySlotIcon;

    private Item currentItem;
    public Item CurrentItem
    {
        get => currentItem;
        protected set
        {
            currentItem = value;
            IsOccupied = currentItem != null;
        }
    }

    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public int StackCount { get => stackCount; set => stackCount = value; }
    public int StackLimit { get => stackLimit; set => stackLimit = value; }
    public Sprite ItemIcon { get => itemIcon; set => itemIcon = value; }




    public Slot(int stackLimit = 1)
    {
        StackLimit = stackLimit;
    }

    public virtual bool AddItem(Item item)
    {   //if the slot is empty, add the item
        TextMeshProUGUI stackLbl = GetComponentInChildren<TextMeshProUGUI>();
        if (CurrentItem == null)
        {
            IsOccupied = true;
            CurrentItem = item;
            StackCount = 1;
            StackLimit = item.MaxStackSize;
           
            
            //if stack limit is greater than 1, show the stack count
            if (StackLimit > 1)
            {
                stackLbl.text = StackCount.ToString();
            }
            else //else hide it
            {
                stackLbl.text = "";
            }

            ItemIcon = item.Icon;
            //getcomponentsinchildren also takes in the image of parent, so set to next index
            //this means the image component meant to hold the sprite(icon) of the item 
            //held in the slot must always be the first child of the parent slot object
            GetComponentsInChildren<Image>()[1].sprite = ItemIcon;
            if (item is IPassiveItem passiveItem)
            {
                passiveItem.Equip();
            }
            return true;

        } //if the item is stackable and the same as the current item, increase the stack count
        else if (CurrentItem.GetType() == item.GetType() && StackCount < StackLimit)
        {
            StackCount += 1;
            stackLbl.text = StackCount.ToString();
            return true;
        }
        return false;
    }

    public virtual bool AddItem(Item item, Slot slot)
    {   //if the slot is empty, add the item
        TextMeshProUGUI stackLbl = GetComponentInChildren<TextMeshProUGUI>();
        if (CurrentItem == null)
        {
            IsOccupied = true;
            CurrentItem = item;
            StackCount = slot.stackCount;
            StackLimit = item.MaxStackSize;
           
            
            //if stack limit is greater than 1, show the stack count
            if (StackLimit > 1)
            {
                stackLbl.text = StackCount.ToString();
            }
            else //else hide it
            {
                stackLbl.text = "";
            }

            ItemIcon = item.Icon;
            //getcomponentsinchildren also takes in the image of parent, so set to next index
            //this means the image component meant to hold the sprite(icon) of the item 
            //held in the slot must always be the first child of the parent slot object
            GetComponentsInChildren<Image>()[1].sprite = ItemIcon;
            if (item is IPassiveItem passiveItem)
            {
                passiveItem.Equip();
            }
            item.CurrentStackSize = StackCount;
            slot.ClearSlot();
            return true;

        } //if the item is stackable and the same as the current item, increase the stack count
        else if (CurrentItem.GetType() == item.GetType() && StackCount < StackLimit)
        {
            //add up to the stack limit and return the rest
            int spaceAvailable = StackLimit - StackCount;
            if (slot.stackCount <= spaceAvailable)
            {
                StackCount += slot.stackCount;
                stackLbl.text = StackCount.ToString();
                item.CurrentStackSize = StackCount;
                slot.ClearSlot();
                return true;
            }
            
            StackCount += spaceAvailable;
            item.CurrentStackSize = StackCount;
            stackLbl.text = StackCount.ToString();
            for (int i = 0; i < spaceAvailable; i++)
            {
                slot.RemoveItem();
            }
            return true;
        }
        return false;
    }

   
    /// <summary>
    /// Removes an item from the slot. If the item is stackable and the stack count is greater than 1,
    /// the stack count is reduced by 1. If the stack count is 1, the item is removed from the slot.
    /// </summary>
    public virtual void RemoveItem()
    {
        if (StackCount > 1)
        {
            StackCount--;
            GetComponentInChildren<TextMeshProUGUI>().text = StackCount.ToString();
            CurrentItem.CurrentStackSize = StackCount;

        }
        else
        {
            if (CurrentItem is IPassiveItem passiveItem)
            {
                passiveItem.Unequip();
            }
            CurrentItem = null;
            StackCount = 0;
            IsOccupied = false;
            GetComponentsInChildren<Image>()[1].sprite = emptySlotIcon;
            GetComponentInChildren<TextMeshProUGUI>().text = "";

        }

    }

    public void ClearSlot()
    {
        CurrentItem = null;
        StackCount = 0;
        IsOccupied = false;
        GetComponentsInChildren<Image>()[1].sprite = emptySlotIcon;
        GetComponentInChildren<TextMeshProUGUI>().text = "";
    }
    public abstract void OnHover();
    public abstract void OnLeftClick();
    public abstract void OnRightClick();
}


public class EquipmentSlot : Slot
{
    public EquipmentSlot(int stackLimit = 1) : base(stackLimit)
    {
    }

    public override bool AddItem(Item item)
    {
        // Check if the item is a "EquipmentItem" 
        if (!(item is EquipmentItem))
        {

            throw new InvalidOperationException("Only Equipment items can be added to a EquipSlot.");
        }
        else
        {

            if (CurrentItem == null)
            {
                CurrentItem = item;
                StackCount = 1;
                ItemIcon = item.Icon;
                GetComponent<Image>().sprite = ItemIcon;
                GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName;
                return true;

            }
            return false;
        }
    }

    public override void RemoveItem()
    {
        if (StackCount > 1)
        {
            StackCount--;
            GetComponentInChildren<TextMeshProUGUI>().text = StackCount.ToString();

        }
        else
        {
            if (CurrentItem is IPassiveItem passiveItem)
            {
                passiveItem.Unequip();
            }
            CurrentItem = null;
            StackCount = 0;
            IsOccupied = false;
            GetComponent<Image>().sprite = emptySlotIcon;
            GetComponentInChildren<TextMeshProUGUI>().text = "";

        }

    }
    public override void OnHover()
    {
        // Display equipment item info
    }

    public override void OnLeftClick()
    {
        if (CurrentItem is IActiveItem activeItem)
        {
            activeItem.Use();
            if (!activeItem.IsReusable)
            {
                RemoveItem();
            }
        }
    }

    public override void OnRightClick()
    {
        RemoveItem();
    }
}

public class HotKeySlot : Slot
{
    public HotKeySlot(int stackLimit) : base(stackLimit) { }
    public int AssignedNumber { get; set; } // The hotkey number 

    public override void OnHover()
    {
        // Display hotkey item info
    }

    private void UseItem()
    {
        Debug.Log("Using hotkey " + AssignedNumber);
        if (CurrentItem is IActiveItem activeItem)
        {
            activeItem.Use();
            if (!activeItem.IsReusable)
            {
                RemoveItem();
            }
        }
    }

    public override void OnLeftClick()
    {
        UseItem();
    }

    public override void OnRightClick()
    {
        RemoveItem();
    }
}

public class ItemSlot : Slot
{
    public ItemSlot(int stackLimit) : base(stackLimit) { }

    public override void OnHover()
    {
        // Display item info
    }

    public override void OnLeftClick()
    {
        if (CurrentItem is IActiveItem activeItem)
        {
            activeItem.Use();
            if (!activeItem.IsReusable)
            {
                RemoveItem();
            }

        }
    }

    public override void OnRightClick()
    {
        RemoveItem();
    }
}
