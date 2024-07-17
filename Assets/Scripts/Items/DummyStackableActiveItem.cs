using UnityEngine;

public class DummyStackableActiveItem : ActiveItem
{ 
    public DummyStackableActiveItem()
    {
        MaxStackSize = 5;
    }
    private int usedAmount = 0;

    public override void Use()
    {
        usedAmount++;
        Debug.Log("Used the dummy stackable active item "+ (usedAmount) + " times");
    }

    
}