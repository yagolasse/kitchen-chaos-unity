using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IKitchenObjectParent, IHasProgress
{

    public event EventHandler<OnStateChangeArgs> OnStateChange;
    public event EventHandler<IHasProgress.OnProgressChangeArgs> OnProgressChange;

    public class OnStateChangeArgs : EventArgs
    {
        public State state;
    }


    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField]
    private FryingRecipeScriptableObject[] fryingRecipes;
    [SerializeField]
    private BurningRecipeScriptableObject[] burningRecipes;

    private float fryingTimer;
    private float burningTimer;
    private State state;
    private FryingRecipeScriptableObject fryingRecipe;
    private BurningRecipeScriptableObject burningRecipe;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeArgs { progressNormalized = (float)fryingTimer / fryingRecipe.fryingTimerMax });

                    if (fryingTimer > fryingRecipe.fryingTimerMax)
                    {
                        fryingTimer = 0;

                        GetKitchenObject().DestroySelf();

                        KitchenObject.Spawn(fryingRecipe.output, this);

                        burningTimer = 0;
                        state = State.Fried;
                        OnStateChange?.Invoke(this, new OnStateChangeArgs { state = this.state });
                        Debug.Log("Fried");
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeArgs { progressNormalized = (float)burningTimer / burningRecipe.burningTimerMax });

                    if (burningTimer > burningRecipe.burningTimerMax)
                    {
                        burningTimer = 0;

                        GetKitchenObject().DestroySelf();

                        KitchenObject.Spawn(burningRecipe.output, this);

                        state = State.Burned;
                        OnStateChange?.Invoke(this, new OnStateChangeArgs { state = this.state });
                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeArgs { progressNormalized = 0 });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }

    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().ScriptableObject))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);

            fryingRecipe = GetFryingRecipeForInput(GetKitchenObject().ScriptableObject);
            burningRecipe = GetBurningRecipeForInput(fryingRecipe.output);

            Debug.Log("Started Frying");

            state = State.Frying;
            OnStateChange?.Invoke(this, new OnStateChangeArgs { state = this.state });
            OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeArgs { progressNormalized = 0 });
            fryingTimer = 0;
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

                        state = State.Idle;
                        OnStateChange?.Invoke(this, new OnStateChangeArgs { state = this.state });
                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeArgs { progressNormalized = 0 });
                    }
                }
                else
                {
                    if (GetKitchenObject().TryGetPlate(out PlateKitchenObject counterPlate))
                    {
                        if (counterPlate.TryAddIngredient(player.GetKitchenObject().ScriptableObject))
                        {

                        }
                    }
                }

            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChange?.Invoke(this, new OnStateChangeArgs { state = this.state });
                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeArgs { progressNormalized = 0 });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectScriptableObject scriptableObject)
    {
        var recipe = GetFryingRecipeForInput(scriptableObject);
        return recipe != null;
    }

    private KitchenObjectScriptableObject GetOutputForInput(KitchenObjectScriptableObject scriptableObject)
    {
        var recipe = GetFryingRecipeForInput(scriptableObject);

        if (recipe != null) return recipe.output;

        return null;
    }

    private FryingRecipeScriptableObject GetFryingRecipeForInput(KitchenObjectScriptableObject scriptableObject)
    {
        foreach (var recipe in fryingRecipes)
        {
            if (recipe.input == scriptableObject)
            {
                return recipe;
            }
        }

        return null;
    }

    private BurningRecipeScriptableObject GetBurningRecipeForInput(KitchenObjectScriptableObject scriptableObject)
    {
        foreach (var recipe in burningRecipes)
        {
            if (recipe.input == scriptableObject)
            {
                return recipe;
            }
        }

        return null;
    }
}
