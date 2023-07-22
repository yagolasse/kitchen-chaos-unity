using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IKitchenObjectParent
{
    public event EventHandler<OnProgressChangeArgs> OnProgressChange;

    public class OnProgressChangeArgs : EventArgs
    {
        public float progressNormalized;
    }

    [SerializeField]
    private CuttingRecipeScriptableObject[] recipes;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().ScriptableObject))
        {
            player.GetKitchenObject().SetClearCounter(this);
            cuttingProgress = 0;
            OnProgressChange?.Invoke(this, new OnProgressChangeArgs { progressNormalized = 0 });
        }
        else if (!player.HasKitchenObject())
        {
            GetKitchenObject().SetClearCounter(player);
            OnProgressChange?.Invoke(this, new OnProgressChangeArgs { progressNormalized = 0 });
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().ScriptableObject))
        {
            var recipe = GetCuttingRecipeForInput(GetKitchenObject().ScriptableObject);

            if (recipe == null) return;

            cuttingProgress++;

            OnProgressChange?.Invoke(this, new OnProgressChangeArgs { progressNormalized = (float)cuttingProgress / recipe.cuttingProgressMax });

            if (cuttingProgress >= recipe.cuttingProgressMax)
            {
                GetKitchenObject().DestroySelf();

                KitchenObject.Spawn(recipe.output, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectScriptableObject scriptableObject)
    {
        var recipe = GetCuttingRecipeForInput(scriptableObject);
        return recipe != null;
    }

    private KitchenObjectScriptableObject GetOutputForInput(KitchenObjectScriptableObject scriptableObject)
    {
        var recipe = GetCuttingRecipeForInput(scriptableObject);

        if (recipe != null) return recipe.output;

        return null;
    }

    private CuttingRecipeScriptableObject GetCuttingRecipeForInput(KitchenObjectScriptableObject scriptableObject)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.input == scriptableObject)
            {
                return recipe;
            }
        }

        return null;
    }
}
