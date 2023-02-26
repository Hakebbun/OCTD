using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class DoUpgradeAction : MonoBehaviour
{
    public bool upgrade(IUpgrade upgrade) {
        Collider2D toUpgradeCollider = Physics2D.OverlapCircle(transform.position, .4f, LayerMask.GetMask("Base", "Moveable"));
        if (toUpgradeCollider) {
            IUpgradeable upgradeable = toUpgradeCollider.gameObject.GetComponent<IUpgradeable>();
            if (upgradeable != null) {
                return upgradeable.OnUpgrade(upgrade);
            }
        }
        return false;
    }
}