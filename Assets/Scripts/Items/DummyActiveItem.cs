using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyActiveItem : ActiveItem
{

    public override void Use()
    {
        Debug.Log("Using the dummy active item. Items that take no arguments can be used to affect world objects."
        + "\n In a multiplayer game, the calling player or all players would be accessed via the network manager."
        + "\n so it is more server authoritative to not pass the player object as an argument.");
    }

}

