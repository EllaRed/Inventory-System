using UnityEngine;

/// <summary>
/// Example of adding item components to game objects.
/// -I was too lazy to write new item classes for this demo-.
/// </summary>
public class CreateItem : MonoBehaviour
{
    [SerializeField] private GameObject[] itemObjects;
    public Sprite[] itemIcons;

    void Start()
    {
        if (itemObjects.Length > 0)
        {

            itemObjects[0].AddComponent<HybridItem>().Icon = itemIcons[0];

            itemObjects[1].AddComponent<EquipmentItem>().SetEquipmentItemType(ItemType.TopEquip);
            itemObjects[1].GetComponent<EquipmentItem>().ItemName = "gloves";
            itemObjects[1].GetComponent<EquipmentItem>().Icon = itemIcons[1];

            //Delegates can be used to assign custom methods to the item objects, but would get very messy very fast in a complex project
            //Feel free to correct me if I'm wrong!
        }
        else
        {
            Debug.Log("No item objects found");
        }
    }


}
