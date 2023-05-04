using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static event Action TestEvent;

    public static float BASE_MOVE_SPEED = 30f;

    public float moveSpeed = 30f;
    private Vector2 moveInput;
    private Rigidbody2D rb2d;
    private PickUpAction pickUpAction;
    private UseTowerAction useTowerAction;
    private DoUpgradeAction doUpgradeAction;
    private BuyBuildingAction buyBuildingAction;
    public Animator animator;

    [Header("Grid Movement")]
    public float inputThreshold;
    public float closeDistance;
    private bool closeToGridPoint;
    private bool lockNonParallelMovement;
    private Vector2 moveToPosition;
    private Vector2 prevMoveToPosition;
    private Vector2 prevMoveDirection;

    public bool facingLeft = true;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>(); 
        pickUpAction = GetComponent<PickUpAction>();
        useTowerAction = GetComponent<UseTowerAction>();
        doUpgradeAction = GetComponent<DoUpgradeAction>();
        buyBuildingAction = GetComponent<BuyBuildingAction>();

        moveToPosition = rb2d.position;
    }

    void FixedUpdate()
    {
        // If no movement, finish moving to previously set point
        if (moveInput == Vector2.zero)
        {
            moveToPosition = prevMoveToPosition;
        }
        // Allow moving back and forth but NOT any other directions until a grid point is reached
        else if (lockNonParallelMovement)   // The code within this 'else if' and the following 'else' are the same, but combining them felt MESSY
        {
            if (moveInput == prevMoveDirection || moveInput == -prevMoveDirection)
            {
                moveToPosition = rb2d.position + (moveInput * GridHelper.gridSize);
                moveToPosition = GridHelper.ClosestGridPoint(moveToPosition);
                prevMoveDirection = moveInput;
            }
        }
        // Allow movement in any direction
        else
        {
            moveToPosition = rb2d.position + (moveInput * GridHelper.gridSize);
            moveToPosition = GridHelper.ClosestGridPoint(moveToPosition);
            prevMoveDirection = moveInput;
        }
        prevMoveToPosition = moveToPosition;

        // Snap to point when close enough, to avoid jitters/errors in position
        closeToGridPoint = (rb2d.position - moveToPosition).magnitude < closeDistance ? true : false;
        if (!closeToGridPoint)
        {
            rb2d.MovePosition(rb2d.position + (moveToPosition - rb2d.position).normalized * moveSpeed * Time.fixedDeltaTime);
            lockNonParallelMovement = true;
        }
        else
        {
            rb2d.MovePosition(moveToPosition);
            lockNonParallelMovement = false;
        }

        useTowerAction.controlTower(moveInput);
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = InputValueMapping(inputValue.Get<Vector2>());

        animator.SetFloat("speed", Mathf.Abs(moveInput.x * moveSpeed));
        
        // If there's a a move input and a direction change, flip
        if ((moveSpeed > 0) &&
            (moveInput.x > 0 && facingLeft) ||
            (moveInput.x < 0 && !facingLeft)
        ) {
            Flip();
        } 
    }

    void OnPickup() {
        // try to load tower
        if (!LevelController.isDay &&
            pickUpAction?.getHoldingObject() != null &&
            pickUpAction?.getHoldingObject().GetComponent<IAmmo>() != null) {
                bool isLoadSuccessful = useTowerAction.loadTower(pickUpAction.getHoldingObject().GetComponent<IAmmo>());
                if (isLoadSuccessful) {
                    pickUpAction.destroyItem();
                    return;
                }
        }

        // try to upgrade
        if (LevelController.isDay &&
            pickUpAction?.getHoldingObject() != null &&
            pickUpAction?.getHoldingObject().GetComponent<IUpgrade>() != null &&
            pickUpAction?.getHoldingObject().GetComponent<IUpgrade>().getCost() <= EconomyController.money) {
            bool isUpgradeSuccessful = doUpgradeAction.upgrade(pickUpAction.getHoldingObject().GetComponent<IUpgrade>());
            if (isUpgradeSuccessful) {
                pickUpAction.destroyItem();
                return;
            }
        }
        
        pickUpAction?.pickUpObject();

        // Fire tower does nothing unless hooked into a tower
        useTowerAction?.fireTower();
    }

    void OnUse() {
        useTowerAction.useTower();
        buyBuildingAction.buyBuilding();
    }

    void OnDebug() {
        Debug.Log(LevelController.isDay);
        if (LevelController.isDay) {
            TestEvent();
        }
    }

    void OnBuy(InputValue inputValue) {
    }

    private void Flip() {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        // holding object gets flipped because of the parent. We flip it again to maintain it
        FlipHolding();

        facingLeft = !facingLeft;
    }

    private void FlipHolding() {
        if (pickUpAction != null && pickUpAction.getHoldingObject() != null) {
            Vector3 currentScale = pickUpAction.getHoldingObject().transform.localScale;
            currentScale.x *= -1;
            pickUpAction.getHoldingObject().transform.localScale = currentScale;
        }
    }

    private Vector2 InputValueMapping(Vector2 inputValue)
    {
        Vector2 finalInput = Vector2.zero;

        float absX = Mathf.Abs(inputValue.x);
        float absY = Mathf.Abs(inputValue.y);

        if (absX > inputThreshold)
            finalInput.x = (inputValue.x > 0) ? 1f : -1f;

        if (absY > inputThreshold)
            finalInput.y = (inputValue.y > 0) ? 1f : -1f;

        return finalInput;
    }

}
