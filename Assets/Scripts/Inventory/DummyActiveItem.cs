using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyActiveItem : Item, IActiveItem
{
    public bool IsReusable { get; set; } = false;
    public float CooldownTime { get; set; }

    public DummyActiveItem()
    {
        ItemType = ItemType.Active;
    }

    public void Use()
    {
        Debug.Log("Using the dummy active item");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player collided with dummy active item");
            //other.GetComponent<Player>().PickupItem(this);
        }
    }
}

