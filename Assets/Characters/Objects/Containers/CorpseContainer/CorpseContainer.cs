using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseContainer : MonoBehaviour, ILoadable
{
    // Start is called before the first frame update
    void Start()
    {
        LevelController.OnPhaseChange += OnPhaseChange;
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }

    public bool Load(IAmmo ammoToLoad)
    {
        Debug.Log(ammoToLoad.GetType().Equals(typeof(BasicBaddieCorpse)));
        return ammoToLoad.GetType().Equals(typeof(BasicBaddieCorpse));
    }

    private void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                gameObject.layer = LayerMask.NameToLayer("Moveable");  
                break;
            case Phase.NIGHT:
                gameObject.layer = LayerMask.NameToLayer("Processor");
                break;
        }
    }
}
