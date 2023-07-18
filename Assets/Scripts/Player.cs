using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterEventArgs> OnSelectedCounterChange;

    public class OnSelectedCounterEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField]
    private float moveSpeed = 10;
    [SerializeField]
    private float rotationSpeed = 10;
    [SerializeField]
    private float radius = 0.7f;
    [SerializeField]
    private float height = 2;
    [SerializeField]
    private float interactionRange = 2;

    [SerializeField]
    private Transform playerHandPoint;
    [SerializeField]
    private GameInput gameInput;
    [SerializeField]
    private LayerMask countersLayersMask;

    private Vector3 lastInteractionDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    public bool IsWalking { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void Update()
    {
        HandleInteractions();
        HandleMovement();
    }
    private void SetSelectedCounter(BaseCounter counter)
    {
        selectedCounter = counter;
        var args = new OnSelectedCounterEventArgs { selectedCounter = counter };
        OnSelectedCounterChange?.Invoke(this, args);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void HandleInteractions()
    {
        if (Physics.Raycast(transform.position, lastInteractionDirection, out RaycastHit raycastHit, interactionRange))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter))
            {
                if (counter != selectedCounter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        var inputVector = gameInput.GetInputVectorNormalized();

        IsWalking = inputVector != Vector2.zero;

        var moveDistance = moveSpeed * Time.deltaTime;
        var moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDirection != Vector3.zero)
        {
            lastInteractionDirection = moveDirection;
        }

        var canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * height, radius, moveDirection, moveDistance);

        if (!canMove)
        {
            var moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;

            var canMoveSideways = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * height, radius, moveDirectionX, moveDistance);

            if (canMoveSideways)
            {
                transform.position += moveDistance * moveDirectionX;
            }
            else
            {
                var moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;

                var canMoveForwardOrBackwards = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * height, radius, moveDirectionZ, moveDistance);

                if (canMoveForwardOrBackwards)
                {
                    transform.position += moveDistance * moveDirectionZ;
                }
            }
        }
        else
        {
            transform.position += moveDistance * moveDirection;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return playerHandPoint;
    }

    public void SetKitchenObject(KitchenObject newKitchenObject)
    {
        kitchenObject = newKitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
