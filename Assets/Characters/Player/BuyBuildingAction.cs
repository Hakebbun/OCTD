using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyBuildingAction : MonoBehaviour
{
    private PickUpAction pickUpAction;

    void Awake() {
        pickUpAction = GetComponent<PickUpAction>();
    }

    public void buyBuilding() {
        if (pickUpAction?.getHoldingObject() != null &&
            pickUpAction?.getHoldingObject().GetComponent<IBuildUpgrade>() != null &&
            pickUpAction?.getHoldingObject().GetComponent<IBuildUpgrade>().getCost() <= EconomyController.money) {
                pickUpAction?.getHoldingObject().GetComponent<IBuildUpgrade>().buildBuilding();
        }
        
    }
}
