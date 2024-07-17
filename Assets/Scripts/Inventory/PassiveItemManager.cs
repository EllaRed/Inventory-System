using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for passive item effects.
/// <para>
/// This class is a singleton that manages the effects of passive items equipped by the player.
/// Passive items when equipped while be added to a list of equipped passive items, to keep track of them all.
/// </para>
/// Emmanuella Dasilva-Domingos
/// <para>Last Updated: July 17, 2024</para>
/// </summary>
public class PassiveItemManager : MonoBehaviour
{
    public static PassiveItemManager Instance { get; private set; }

    [SerializeField]
    public List<IPassiveItem> equippedPassiveItems = new List<IPassiveItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    public void EquipPassiveItem(IPassiveItem item)
    {
        if (!equippedPassiveItems.Contains(item))
        {
            equippedPassiveItems.Add(item);
            item.ApplyPassiveEffect();
        }
    }

    public void UnequipPassiveItem(IPassiveItem item)
    {
        item.RemovePassiveEffect();
        equippedPassiveItems.Remove(item);
    }
}
