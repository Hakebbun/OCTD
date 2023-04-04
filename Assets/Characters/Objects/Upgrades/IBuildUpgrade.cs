using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildUpgrade
{
    int getCost();
    void buildBuilding();
}