using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPhase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelController.OnPhaseChange += OnPhaseChange;
    }

    private void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                GetComponent<TMPro.TextMeshProUGUI>().text = "DAY";
                break;
            case Phase.NIGHT:
                GetComponent<TMPro.TextMeshProUGUI>().text = "NIGHT";
                break;
        }
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }
}
