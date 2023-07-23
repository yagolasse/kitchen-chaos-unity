using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeScriptableObject : ScriptableObject
{
    public int cuttingProgressMax;
    public KitchenObjectScriptableObject input;
    public KitchenObjectScriptableObject output;
}
