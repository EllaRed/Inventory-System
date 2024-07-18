using UnityEngine;

/// <summary>
/// Example of adding item components to game objects.
/// -I was too lazy to write new item classes for this demo-.
/// </summary>
public class CreateItem : MonoBehaviour
{
    [SerializeField] private GameObject[] itemObjects; 
 
    void Start()
    {
        if(itemObjects.Length > 0){
        
           itemObjects[0].AddComponent<HybridItem>();
           
           itemObjects[1].AddComponent<EquipmentItem>().SetEquipmentItemType(ItemType.TopEquip);
           itemObjects[1].GetComponent<EquipmentItem>().ItemName = "bodice";

           
           //Delegates can be used to assign custom methods to the item objects, but would get very messy very fast in a complex project
           //Feel free to correct me if I'm wrong!
        }else{
            Debug.Log("No item objects found");
        }
    }

   
}
