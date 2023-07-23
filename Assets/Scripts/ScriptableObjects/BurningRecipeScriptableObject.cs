using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeScriptableObject : ScriptableObject
{
    public float burningTimerMax;
    public KitchenObjectScriptableObject input;
    public KitchenObjectScriptableObject output;  
}
