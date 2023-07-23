using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IKitchenObjectParent, IHasProgress
{
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangeArgs> OnProgressChange;

    [SerializeField]
    private CuttingRecipeScriptableObject[] recipes;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().ScriptableObject))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            cuttingProgress = 0;
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObject().ScriptableObject))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeArgs { progressNormalized = 0 });
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().ScriptableObject))
        {
            var recipe = GetCuttingRecipeForInput(GetKitchenObject().ScriptableObject);

            if (recipe == null) return;

            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeArgs { progressNormalized = (float)cuttingProgress / recipe.cuttingProgressMax });
            
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
