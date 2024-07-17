using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player controller for movement and interacting with the inventory.
/// <para> Emmanuella Dasilva-Domingos</para>
/// <para> Last Updated: July 17, 2024</para>
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private Canvas inventoryCanvas;
    private PlayerControls controls;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5f;
    void Start()
    {
        controls = new PlayerControls();
        inventory= GameObject.Find("Inventory");
        inventoryCanvas= inventory.GetComponent<Canvas>();
     
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        Vector3 movement = new Vector3(input.x, 0f, input.y) * speed;
        rb.linearVelocity = movement;
    }
    private void OnInventory(){
        if(inventoryCanvas.targetDisplay!=0)
            inventoryCanvas.targetDisplay=0;
        else
            inventoryCanvas.targetDisplay=9;
    }

    private void OnHotkeys(InputValue value){
        if(value.isPressed){
        Debug.Log("Pressed hotkey: " + value.Get<float>());
        //value 10= key 0.
        //0 cannot be read as a value by the action scalar processor, 
        //instead zero is used as the base value of the scalar, and will be the value
        //returned when the button is touched in any way. Remove the 'if value.isPressed' statement to see.
        //zero will first be returned followed by the value of the button pressed, if you set a button under
        //that action to zero on the scalar. 
        //Because of this, the value of the 0 button is set to 10 on the scalar for the hotkey action.
        //It also makes it much easier to map to the hotkey list in the inventory manager.
        inventory.GetComponent<InventoryManager>().UseHotkey((int)value.Get<float>()-1);
        }
    }

    private void PickupItem(Item item)
    {
        inventory.GetComponent<InventoryManager>().AddItem(item);
    }

    /*Another to do this is to seperate this behaviour from the player controller, if it's getting to long
    and make this function a part of the item class, but that would couple it to the player class and throws modularity out the window.
    Plus you would need additional methods for situations like clicking on and item or it being given via a dialogue context, and not all item 
    instances might require that.
    Another way is to make a new script that acts as the middle man and can handle any method the player can get an item.
    For this simple use case of all items being 3d objects we can walk into, our middle man's trigger function would look something like this:
     private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player collided with item");
            other.GetComponent<Player>().PickupItem(GetComponent<Item>());
        }
    }
    where the item script would be a seperate script attached to the same  object as our middle man
    and then other functions can be added to handle other ways of getting items without needing to modify the item class.
    I'm very open to suggestions on how to make this could be done better, please drop some!.
    */

     private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Item")) {
            Debug.Log("Player collided with: " + other.name);
            PickupItem(other.GetComponent<Item>());
            //Additional logic such as destroying the item or putting in on respawn cooldown can be done here.
        }
    }

}
