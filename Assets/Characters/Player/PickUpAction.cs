using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAction : MonoBehaviour
{

    public LayerMask pickUpMask;
    public Transform holdSpot;
    public Transform towerHoldSpot;
    public GameObject itemHolding = null;
    public GameObject pickupSpot;
    public float pickupObjectScale;
    private Vector3 objectOriginalScale;

    public GameObject placementCursor;
    public float cursorDistanceFromPlayer;
    private Vector2 cursorPosition;

    private PlayerController playerController;

    void Start() {
        pickUpMask = LayerMask.GetMask("Moveable", "Upgrade");
        playerController = GetComponent<PlayerController>();
        LevelController.OnPhaseChange += OnPhaseChange;
        placementCursor = Instantiate(placementCursor);
        placementCursor.SetActive(false);
    }

    private void Update()
    {
        if (itemHolding)
        {
            cursorPosition = this.transform.position;
            cursorPosition += (playerController.PrevMoveDirection * GridHelper.gridSize * cursorDistanceFromPlayer);
            cursorPosition = GridHelper.ClosestGridPoint(cursorPosition);
            placementCursor.transform.position = cursorPosition;
        }
    }

    public void pickUpObject() {
        // Release item if holding something
        if (itemHolding) {
             dropItem();
        } else {
            if (pickupFromContainer()) {return;}
            if (pickupLooseItem()) {return;}
        }
    }

    public GameObject getHoldingObject() {
        return itemHolding;
    }

    public void dropItem() {
        if (itemHolding != null) {
            itemHolding.transform.parent = null;
            if (itemHolding.GetComponent<Rigidbody2D>()) {
                    itemHolding.GetComponent<Rigidbody2D>().simulated = true;
                }
            itemHolding.transform.position = GridHelper.ClosestGridPoint(cursorPosition);
            placementCursor.SetActive(false);
            ResetScaleOnDrop(itemHolding);
            itemHolding = null;
        }
    }

    public void destroyItem() {
        if (itemHolding != null) {
            Destroy(itemHolding);
            itemHolding = null;
        }
    }

    private bool pickupLooseItem() {
            int xOffset;
            if (playerController.facingLeft) {
                xOffset = -3;
            } else {
                xOffset = 3;
            }

            Vector2 positionToPickup = transform.position;
            positionToPickup.x = positionToPickup.x + xOffset;

            // Pickup item if holding nothing
            Collider2D toPickUp = Physics2D.OverlapCircle(positionToPickup, .8f, pickUpMask);
            
            if (toPickUp) {
                itemHolding = toPickUp.gameObject;
                objectOriginalScale = toPickUp.gameObject.transform.localScale;
                itemHolding.transform.localScale = itemHolding.transform.localScale * pickupObjectScale;
                itemHolding.transform.position = towerHoldSpot.position;
                itemHolding.transform.parent = transform;

            if (itemHolding.GetComponent<Rigidbody2D>()) {
                    itemHolding.GetComponent<Rigidbody2D>().simulated = false;
                    placementCursor.SetActive(true);
                return true;
                }
            }
            return false;
    }

    private bool pickupFromContainer() {
            int xOffset;
            if (playerController.facingLeft) {
                xOffset = -3;
            } else {
                xOffset = 3;
            }

            Vector2 positionToPickup = transform.position;
            positionToPickup.x = positionToPickup.x + xOffset;

            // Pickup item if holding nothing
            Collider2D toPickUpFrom = Physics2D.OverlapCircle(positionToPickup, .8f, LayerMask.GetMask("Container"));
            if (toPickUpFrom) {
                Container container = toPickUpFrom.GetComponent<Container>();
                if (container) {
                    GameObject newGameObject = Instantiate(container.produces, transform.position, transform.localRotation) as GameObject;
                    itemHolding = newGameObject;
                    itemHolding.transform.position = holdSpot.position;
                    itemHolding.transform.parent = transform;

                    if (itemHolding.GetComponent<Rigidbody2D>()) {
                        itemHolding.GetComponent<Rigidbody2D>().simulated = false;
                        return true;
                    }
                }
            }
            return false;
    }

    private void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                pickUpMask = LayerMask.GetMask("Moveable", "Upgrade");
                break;
            case Phase.NIGHT:
                pickUpMask = LayerMask.GetMask("PickUp");
                break;
        }
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }

    private void ResetScaleOnDrop(GameObject objectToDrop) {
        //Vector3 currentScale = objectToDrop.transform.localScale;
        //currentScale.x = Mathf.Abs(currentScale.x);
        //objectToDrop.transform.localScale = currentScale;
        objectToDrop.transform.localScale = objectOriginalScale;
    }
}
