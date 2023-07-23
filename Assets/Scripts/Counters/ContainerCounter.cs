using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField]
    private KitchenObjectScriptableObject kitchenObjectScriptableObject;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject()) return;

        KitchenObject.Spawn(kitchenObjectScriptableObject, player);

        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
