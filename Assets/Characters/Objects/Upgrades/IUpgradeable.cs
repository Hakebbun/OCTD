using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IUpgradeable
{
    bool OnUpgrade(IUpgrade upgrade);
}
