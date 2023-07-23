using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeScriptableObject : ScriptableObject
{
    public float fryingTimerMax;
    public KitchenObjectScriptableObject input;
    public KitchenObjectScriptableObject output;
}
