using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetClearCounter(this);
            }
        }
        else if (!player.HasKitchenObject())
        {
            GetKitchenObject().SetClearCounter(player);
        }
    }
}
