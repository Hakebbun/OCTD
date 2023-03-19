using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnPhaseChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelController.OnPhaseChange += OnPhaseChange;
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }

    private void OnPhaseChange(Phase phase) {
        Destroy(gameObject);
    }
}
