using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField]
    private KitchenObjectScriptableObject scriptableObject;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectScriptableObject ScriptableObject { get => scriptableObject; }

    public static KitchenObject Spawn(KitchenObjectScriptableObject scriptableObject, IKitchenObjectParent parent)
    {
        var kitchenObjectTransform = Instantiate(scriptableObject.prefab, parent.GetKitchenObjectFollowTransform());
        var kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(parent);
        return kitchenObject;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent newKitchenObjectParent)
    {
        if (kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenObject();
        }

        kitchenObjectParent = newKitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }

        plateKitchenObject = null;
        return false;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }
}
