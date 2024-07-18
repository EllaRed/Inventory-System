using System.Collections;
using UnityEngine;


public class DummyPassiveItem : PassiveItem
{
    //Gameobject.Find is expensive and impractical in a multiplayer game
    //If using unity netcode for games, the player object would be accessed via the network manager
    //In a single player non-networked game, the player object can be stored and accessed via a singleton
    //, for example, a game manager. 
    MeshRenderer targetRenderer ;
    private void Start()
    {
        targetRenderer = GameObject.Find("Player").GetComponent<MeshRenderer>();
    }
    public override void ApplyPassiveEffect()
    {
        Debug.Log("Applying dummy passive effect");
        if(targetRenderer == null)
        {
            Debug.LogError("Player object not found");
            return;
        }
        StartCoroutine(ChangeColor());     
    }
    IEnumerator ChangeColor()
    {
        while (true)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            targetRenderer.material.color = randomColor;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void RemovePassiveEffect()
    {
        Debug.Log("Removing dummy passive effect");
        if(targetRenderer == null)
        {
            Debug.LogError("Player object not found");
            return;
        }
        StopCoroutine(ChangeColor());
        targetRenderer.material.color = Color.white;
    
    }

}
