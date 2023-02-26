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
    private bool isHoldingBuy = false;
    private float buyTimer = 2.0f;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>(); 
        pickUpAction = GetComponent<PickUpAction>();
        useTowerAction = GetComponent<UseTowerAction>();
        doUpgradeAction = GetComponent<DoUpgradeAction>();
    }


    void FixedUpdate() {
        rb2d.MovePosition(rb2d.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        useTowerAction.controlTower(moveInput);
        UpdateHoldTimer();
    }

    void OnMove(InputValue inputValue) {
        moveInput = inputValue.Get<Vector2>();
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
        if(inputValue.isPressed) {
            StartHoldTimer();
        } else {
            CancelHoldTimer();
        }
    }

    private void StartHoldTimer() {
        isHoldingBuy = true;
        buyTimer = 2.0f;
    }

    private void CancelHoldTimer(){
        isHoldingBuy = false;
        buyTimer = 2.0f;
    }

    private void UpdateHoldTimer() {
        if (isHoldingBuy) {
            buyTimer -= Time.deltaTime;
        }

        if (buyTimer <= 0) {
            HoldTimerComplete();
            CancelHoldTimer();
        }
    }

    private void HoldTimerComplete(){
        Debug.Log("Hold timer complete");
    }

}
