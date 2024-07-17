using UnityEngine;

public class EquipHead : EquipmentItem
{
    public EquipHead() : base(ItemType.HeadEquip)
    {
        ItemType = ItemType.HeadEquip;
    }

    public override void ApplyPassiveEffect()
    {
        Debug.Log("Applying head equipment's passive effect");
        // Logic for applying the passive effect
    }
    
}