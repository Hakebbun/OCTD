using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    
    void OnMove(Vector2 input);
    void OnFire();
}
