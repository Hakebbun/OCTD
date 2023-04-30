using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SeedTower : MonoBehaviour, ITower, IBuildingBuffable, ILoadable
{

    public GameObject bulletType;
    public TMPro.TextMeshProUGUI tmp;
    public GameObject gunObject;

    private List<int> activeUpgradeIds; 

    private MoneySpender moneySpender;

    public int ammo = 0;
    public float aimSpeed = 1f;
    private bool isAoeUpgraded = false;
    public int damageUpgrades = 0;
    public int maxAmmo = 10;

    void Start()
    {
        activeUpgradeIds = new List<int>();
        LevelController.OnPhaseChange += OnPhaseChange;
        moneySpender = GetComponent<MoneySpender>();
        tmp.text = ammo.ToString();
    }

    public void OnFire()
    {
        if (ammo > 0) {
            GameObject newGameObject = Instantiate(bulletType, transform.position, transform.localRotation) as GameObject;
            SeedBullet bullet = newGameObject.GetComponent<SeedBullet>();
            if (bullet) {
                bullet.SetUpgrades(isAoeUpgraded, damageUpgrades);
                bullet.Fire(gunObject.transform.up);
                ammo --;
                tmp.text = ammo.ToString();
            }
        }
    }

    public void OnMove(Vector2 aimInput)
    {
        // if greater than 0, rotate right
        if (aimInput.x > 0) {
            gunObject.transform.Rotate(0, 0, aimSpeed * -1);
        } else if (aimInput.x < 0) {
            // less than 0 rotate left
            gunObject.transform.Rotate(0, 0, aimSpeed);
        } else {
            // equal to 0 do nothing
        }
    }

    public bool Load(IAmmo ammoToLoad)
    {
        // verify loadable
        if (ammo < maxAmmo && ammoToLoad.GetType().Equals(typeof(SeedAmmo))) {
            ammo ++;
            tmp.text = ammo.ToString();
            return true;
        } else {
            return false;
        }
    }

    private void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                ammo = 0;
                transform.rotation = Quaternion.identity;
                gameObject.layer = LayerMask.NameToLayer("Moveable");  
                break;
            case Phase.NIGHT:
                gameObject.layer = LayerMask.NameToLayer("TowerLayer");
                break;
        }
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }

    public bool OnBuff(IBuildingBuff buildingBuff) {
        if (buildingBuff.GetType().Equals(typeof(DamageBuffer))) {
            if (!activeUpgradeIds.Contains(buildingBuff.GetId())) {
                activeUpgradeIds.Add(buildingBuff.GetId());
                damageUpgrades += 1;
                return true;
            }
        }
        return false;
    }

    public bool OnDebuff(IBuildingBuff buildingBuff) {
        if (buildingBuff.GetType().Equals(typeof(DamageBuffer))) {
            if (activeUpgradeIds.Contains(buildingBuff.GetId())) {
                activeUpgradeIds.Remove(buildingBuff.GetId());
                damageUpgrades -= 1;
                return true;
            }
        }
        return false;
    }
}
