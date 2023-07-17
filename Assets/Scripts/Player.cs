using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
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
    private GameInput gameInput;
    [SerializeField]
    private LayerMask countersLayersMask;

    private Vector3 lastInteractionDirection;

    public bool IsWalking { get; private set; }

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnInteractAction;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void GameInputOnInteractAction(object sender, System.EventArgs e)
    {
        if (Physics.Raycast(transform.position, lastInteractionDirection, out RaycastHit raycastHit, interactionRange))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
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
}
