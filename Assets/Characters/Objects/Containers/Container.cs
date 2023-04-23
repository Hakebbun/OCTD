using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public GameObject produces;
    
    // Start is called before the first frame update
    void Start()
    {
        LevelController.OnPhaseChange += OnPhaseChange;
    }

    private void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                GetComponent<Renderer>().sharedMaterial.SetFloat("_Thickness", 0f);
                gameObject.layer = LayerMask.NameToLayer("Moveable");   
                break;
            case Phase.NIGHT:
                GetComponent<Renderer>().sharedMaterial.SetFloat("_Thickness", 0.02f);
                gameObject.layer = LayerMask.NameToLayer("Container");
                break;
        }
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }
}
