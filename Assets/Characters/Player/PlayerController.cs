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
    public Animator animator;

    private bool facingLeft = true;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>(); 
        pickUpAction = GetComponent<PickUpAction>();
        useTowerAction = GetComponent<UseTowerAction>();
        doUpgradeAction = GetComponent<DoUpgradeAction>();
    }


    void FixedUpdate() {
        rb2d.MovePosition(rb2d.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        useTowerAction.controlTower(moveInput);
    }

    void OnMove(InputValue inputValue) {
        moveInput = inputValue.Get<Vector2>();
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
        facingLeft = !facingLeft;
    }

}
