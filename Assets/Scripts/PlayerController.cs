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
    [SerializeField] private GameObject inventoryUI;
    private PlayerControls controls;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5f;
    void Start()
    {
        controls = new PlayerControls();
        inventory= GameObject.Find("Inventory");
        inventoryUI = inventory.transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        Vector3 movement = new Vector3(input.x, 0f, input.y) * speed;
        rb.linearVelocity = movement;
    }
    private void OnInventory(){
        inventoryUI.SetActive(!inventoryUI.activeSelf);
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
}
