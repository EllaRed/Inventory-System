using TMPro;
using UnityEngine;
/// <summary>
/// Displays the name of the game object in the scene.
/// Was too lazy to rename each object in the scene manually.
/// <para>Emmanuella Dasilva-Domingos</para>
/// </summary>
public class DisplayName : MonoBehaviour
{
    //NB: TMP is for 3d tmp, which doesn't require a canvas. 2D tmp namespace is TextMeshProUGUI
    public TextMeshPro tmpLbl;
    void Start()
    {
        tmpLbl = GetComponentInChildren<TextMeshPro>();
        tmpLbl.text = gameObject.name;
    }

    
}
