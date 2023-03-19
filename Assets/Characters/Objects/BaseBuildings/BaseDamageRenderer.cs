using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageRenderer : MonoBehaviour
{

    public Sprite[] spriteArray;
    private int currentSpriteIndex;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void updateSprite(float healthPercent) {
        if (healthPercent >= 0.83f && currentSpriteIndex != 0) {
            spriteRenderer.sprite = spriteArray[0];
            currentSpriteIndex = 0;
        } else if (healthPercent <= 0.83f && healthPercent >= 0.66f && currentSpriteIndex != 1) {
            spriteRenderer.sprite = spriteArray[1];
            currentSpriteIndex = 1;
        } else if (healthPercent <= 0.66f && healthPercent >= 0.5f && currentSpriteIndex != 2) {
            spriteRenderer.sprite = spriteArray[2];
            currentSpriteIndex = 2;
        } else if (healthPercent <= 0.5f && healthPercent >= 0.33f && currentSpriteIndex != 3) {
            spriteRenderer.sprite = spriteArray[3];
            currentSpriteIndex = 3;
        } else if (healthPercent <= 0.33f && healthPercent >= 0.17f && currentSpriteIndex != 4) {
            spriteRenderer.sprite = spriteArray[4];
            currentSpriteIndex = 4;
        } else if (healthPercent <= 0.17f && currentSpriteIndex != 5){
            spriteRenderer.sprite = spriteArray[5];
            currentSpriteIndex = 5;
        }
    }
}
