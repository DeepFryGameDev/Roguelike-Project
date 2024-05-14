using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTestEvent : BaseInteractable
{
    void Start()
    {
        
    }

    void Update()
    {

    }

    /// <summary>
    /// This method should be used on every interactable in the world
    /// Can be customized to perform any event in BaseScriptedEvent by referencing bse
    /// </summary>
    public override void OnInteract()
    {
        base.OnInteract();
        bse.TransitionToScene(2);
    }
}
