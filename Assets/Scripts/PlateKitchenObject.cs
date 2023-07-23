using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField]
    private KitchenObjectScriptableObject[] validKitchenObjectScriptableObjects;

    private List<KitchenObjectScriptableObject> kitchenObjects;

    private void Awake()
    {
        kitchenObjects = new List<KitchenObjectScriptableObject>();
    }

    public bool TryAddIngredient(KitchenObjectScriptableObject scriptableObject)
    {
        if (kitchenObjects.Contains(scriptableObject) || !validKitchenObjectScriptableObjects.Contains(scriptableObject)) return false;

        kitchenObjects.Add(scriptableObject);
        return true;
    }
}
