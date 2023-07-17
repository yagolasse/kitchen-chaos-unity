using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectScriptableObject;
    [SerializeField]
    private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public void Interact(Player player)
    {
        if (kitchenObject == null)
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetClearCounter(this);
            }
            else
            {
                var kitchenObjectTransform = Instantiate(kitchenObjectScriptableObject.prefab, counterTopPoint);
                kitchenObjectTransform.GetComponent<KitchenObject>().SetClearCounter(this);
            }
        }
        else if (!player.HasKitchenObject())
        {
            kitchenObject.SetClearCounter(player);
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject value)
    {
        kitchenObject = value;
    }
}
