using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for passive item effects.
/// <para>
/// This class is a singleton that manages the effects of passive items equipped by the player.
/// Passive items when equipped while be added to a list of equipped passive items
/// and their effects will be applied in the Update method.
/// This makes it easy to manage the effects of passive items without having to check for them in every script
/// or cluttering up a player controller with passive item logic.
/// </para>
/// Emmanuella Dasilva-Domingos
/// <para>Last Updated: July 17, 2024</para>
/// </summary>
public class PassiveItemManager : MonoBehaviour
{
    public static PassiveItemManager Instance { get; private set; }

    private List<IPassiveItem> equippedPassiveItems = new List<IPassiveItem>();

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

    void Update()
    {
        foreach (var item in equippedPassiveItems)
        {
            item.ApplyPassiveEffect();
        }
    }

    public void EquipPassiveItem(IPassiveItem item)
    {
        if (!equippedPassiveItems.Contains(item))
        {
            equippedPassiveItems.Add(item);
        }
    }

    public void UnequipPassiveItem(IPassiveItem item)
    {
        equippedPassiveItems.Remove(item);
    }
}
