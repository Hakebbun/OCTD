using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpotController : MonoBehaviour
{
    public GameObject toPickUp = null;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(transform.parent.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (transform.parent.gameObject.GetComponent<PickUpAction>().itemHolding != null) {
            return;
        }                
        other.gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Thickness", 0.02f);
    }

    void OnTriggerExit2D(Collider2D other){
        if (transform.parent.gameObject.GetComponent<PickUpAction>().itemHolding != null) {
        }
        other.gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Thickness", 0f);
    }
}
