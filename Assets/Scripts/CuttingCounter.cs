using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField]
    private CuttingRecipeScriptableObject[] recipes;

    private int cuttingProgress;
    private int cuttingProgressMax;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().ScriptableObject))
            {
                player.GetKitchenObject().SetClearCounter(this);
                cuttingProgress = 0;
                cuttingProgressMax = GetCuttingMaxForInput(GetKitchenObject().ScriptableObject);
            }
        }
        else if (!player.HasKitchenObject() && cuttingProgress >= cuttingProgressMax)
        {
            GetKitchenObject().SetClearCounter(player);
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().ScriptableObject))
        {
            var output = GetOutputForInput(GetKitchenObject().ScriptableObject);

            if (output == null) return;

            cuttingProgress++;

            if (cuttingProgress >= cuttingProgressMax)
            {
                GetKitchenObject().DestroySelf();

                KitchenObject.Spawn(output, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectScriptableObject scriptableObject)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.input == scriptableObject)
            {
                return true;
            }
        }

        return false;
    }

    private KitchenObjectScriptableObject GetOutputForInput(KitchenObjectScriptableObject scriptableObject)
    {
        foreach(var recipe in recipes)
        {
            if (recipe.input == scriptableObject)
            {
                return recipe.output;
            }
        }

        return null;
    }

    private int GetCuttingMaxForInput(KitchenObjectScriptableObject scriptableObject)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.input == scriptableObject)
            {
                return recipe.cuttingProgressMax;
            }
        }

        return 0;
    }
}
