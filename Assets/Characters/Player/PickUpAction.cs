using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAction : MonoBehaviour
{

    public LayerMask pickUpMask;
    public Transform holdSpot;
    private GameObject itemHolding = null;

    void Start() {
        pickUpMask = LayerMask.GetMask("Moveable", "Upgrade");
        LevelController.OnPhaseChange += OnPhaseChange;
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
            // Pickup item if holding nothing
            Collider2D toPickUp = Physics2D.OverlapCircle(transform.position, .4f, pickUpMask);
            if (toPickUp) {
                itemHolding = toPickUp.gameObject;
                itemHolding.transform.position = holdSpot.position;
                itemHolding.transform.parent = transform;

                if (itemHolding.GetComponent<Rigidbody2D>()) {
                    itemHolding.GetComponent<Rigidbody2D>().simulated = false;
                    return true;
                }
            }
            return false;
    }

    private bool pickupFromContainer() {
            // Pickup item if holding nothing
            Collider2D toPickUpFrom = Physics2D.OverlapCircle(transform.position, .4f, LayerMask.GetMask("Container"));
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
        Vector3 currentScale = objectToDrop.transform.localScale;
        currentScale.x = Mathf.Abs(currentScale.x);
        objectToDrop.transform.localScale = currentScale;
    }
}
