using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeBuffer : MonoBehaviour, IBuildingBuff
{
    void OnTriggerEnter2D(Collider2D other) {
        IBuildingBuffable buffable = other.gameObject.GetComponent<IBuildingBuffable>();
        if (buffable != null) {
            buffable.OnBuff(this);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        IBuildingBuffable buffable = other.gameObject.GetComponent<IBuildingBuffable>();
        if (buffable != null) {
            buffable.OnDebuff(this);
        }
    }

    public int GetId() {
        return gameObject.GetInstanceID();
    }
}
