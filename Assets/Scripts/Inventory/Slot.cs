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
        StackLimit= stackLimit;    
    }

    public virtual void AddItem(Item item)
    {   //if the slot is empty, add the item
        if (CurrentItem == null)
        {
            IsOccupied = true;
            CurrentItem = item;
            StackCount = 1;
            TextMeshProUGUI stackLbl= GetComponentInChildren<TextMeshProUGUI>();
            //if stack limit is greater than 1, show the stack count
            if (StackLimit> 1)
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

        } //if the item is stackable and the same as the current item, increase the stack count
        else if (CurrentItem.GetType() == item.GetType() && StackCount < StackLimit)
        {
            StackCount = StackCount + 1;
        }
    }

    public virtual void RemoveItem()
    {
        if (StackCount > 1)
        {
            StackCount--;
        }
        else
        {
            CurrentItem = null;
            StackCount = 0;
            IsOccupied = false;
        }
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

    public override void AddItem(Item item)
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

            }
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
        Debug.Log("Using hotkey "+ AssignedNumber);
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
