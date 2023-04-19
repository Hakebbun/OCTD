using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{

    private PlayerController playerController;
    private PickUpAction pickupController;
    private UseTowerAction useTowerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        pickupController = GetComponent<PickUpAction>();
        useTowerController = GetComponent<UseTowerAction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pickupController.itemHolding == null && 
            useTowerController.towerToUse == null
        ) {
            int xOffset;
            if (playerController.facingLeft) {
                xOffset = -3;
            } else {
                xOffset = 3;
            }

            Vector2 positionToHighlight = transform.position;
            positionToHighlight.x = positionToHighlight.x + xOffset;

            // Pickup item if holding nothing
            Collider2D toPickUpFrom = Physics2D.OverlapCircle(positionToHighlight, .8f, LayerMask.GetMask("Container"));
            
            
        }
    }
}
