using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieUtils : MonoBehaviour
{
    public static event Action OnBaddieKilled;

    public void EmitBaddieKilledEvent() {
        OnBaddieKilled?.Invoke();
    }
}
