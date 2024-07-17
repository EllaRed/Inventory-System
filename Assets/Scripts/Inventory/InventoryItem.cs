using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Active,
    Passive,
    Hybrid,
    HeadEquip,
    TopEquip,
    BottomEquip
}
/// <summary>
/// Base class for all items in the system.
/// Defines active, passive, and hybrid items.
/// Can be expanded to include more specific item types such as equipment.
/// <para> Emmanuella Dasilva-Domingos</para>
/// <para> Last Updated: July 17, 2024</para>
/// </summary>
public abstract class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;
    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }

    [SerializeField]
    private Sprite icon;
    public Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    [SerializeField]
    private string description;
    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    [SerializeField]
    private int maxStackSize = 1;
    public int MaxStackSize
    {
        get { return maxStackSize; }
        set { maxStackSize = value; }
    }

    public bool IsStackable => MaxStackSize > 1;

    public ItemType ItemType { get; set; }
}

public interface IActiveItem
{
    /// <summary>
    /// Method where active item functionality is implemented
    /// </summary>
    void Use();
    bool IsReusable { get; }
    float CooldownTime { get; }
}

/// <summary>
/// Effects that change over time must be implemented using a coroutine and started in the ApplyPassiveEffect method.
/// </summary>
public interface IPassiveItem
{
    void ApplyPassiveEffect();
    void RemovePassiveEffect();
    void Equip();
    void Unequip();
}

public interface IHybridItem : IActiveItem, IPassiveItem { }

/// <summary>
/// Active Items by default are not reusable and have a cooldown time of 0.
/// </summary>
public class ActiveItem : Item, IActiveItem
{
    public bool IsReusable { get; set; }
    public float CooldownTime { get; set; }

    public ActiveItem()
    {
        ItemType = ItemType.Active;
    }

    public virtual void Use()
    {
        Debug.Log("Used an active item");
    }

}


public class PassiveItem : Item, IPassiveItem
{
    public PassiveItem()
    {
        ItemType = ItemType.Passive;
    }
    public virtual void ApplyPassiveEffect()
    {
        // Logic for applying the passive effect
        Debug.Log("Applying passive item's passive effect");
    }

    public virtual void RemovePassiveEffect()
    {
        // Logic for removing the passive effect
        Debug.Log("Removing passive item's passive effect");
    }

    public void Equip()
    {
        PassiveItemManager.Instance.EquipPassiveItem(this);
    }

    public void Unequip()
    {
        PassiveItemManager.Instance.UnequipPassiveItem(this);
    }
}

public class HybridItem : Item, IHybridItem
{
    // Active Item Properties
    public bool IsReusable { get; set; }
    public float CooldownTime { get; set; }

    public HybridItem()
    {
        ItemType = ItemType.Hybrid;
    }

    // Active Item Method
    public virtual void Use()
    {
        Debug.Log("Using the hybrid item actively");
    }

    // Passive Item Method
    public virtual void ApplyPassiveEffect()
    {
        Debug.Log("Applying hybrid item's passive effect");
        // Logic for applying the passive effect
    }

    public void RemovePassiveEffect()
    {
        Debug.Log("Removing hybrid item's passive effect");
        // Logic for removing the passive effect
    }

    public void Equip()
    {
        PassiveItemManager.Instance.EquipPassiveItem(this);
    }

    public void Unequip()
    {
        PassiveItemManager.Instance.UnequipPassiveItem(this);
    }
}

public class EquipmentItem : Item, IPassiveItem
{
    public EquipmentItem(ItemType itemType)
    {
        ItemType = itemType;
    }
    public void SetEquipmentItemType(ItemType itemType)
    {
        ItemType = itemType;
    }
   
    public virtual void ApplyPassiveEffect()
    {
        
    }

    public virtual void RemovePassiveEffect(){

    }

     public void Equip()
    {
        PassiveItemManager.Instance.EquipPassiveItem(this);
    }

    public void Unequip()
    {
        PassiveItemManager.Instance.UnequipPassiveItem(this);
    }

}