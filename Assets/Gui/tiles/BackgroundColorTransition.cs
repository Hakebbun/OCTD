using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundColorTransition : MonoBehaviour
{

    private Tilemap Tilemap;
    private bool isAnimating;
    private Color currentColor = Color.white;
    private Color DAY = Color.white;
    private Color NIGHT = new Color(0.59f, 0.66f, 1f, 1f);
    
    private Color startingColor;
    private Color targetColor;

    private float lerpTime = 0;

    // Start is called before the first frame update
    void Awake()
    {
        startingColor = NIGHT;
        targetColor = DAY;

        Tilemap = GetComponent<Tilemap>();
        LevelController.OnPhaseChange += OnPhaseChange;
    }

    void Update() {
        if (!currentColor.Equals(targetColor)) {
            lerpTime += Time.deltaTime / 2.0f; // Divided by 5 to make it 5 seconds.
            currentColor = Color.Lerp(startingColor, targetColor, lerpTime);
            Tilemap.color = currentColor;
        }
    }

    private void OnPhaseChange(Phase phase) {
        if (phase == Phase.NIGHT) {
            targetColor = NIGHT;
            startingColor = DAY;
            lerpTime = 0;
        } else if (phase == Phase.DAY) {
            targetColor = DAY;
            startingColor = NIGHT;
            lerpTime = 0;
        }
    }
}
