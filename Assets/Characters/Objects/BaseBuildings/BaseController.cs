using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour, IHittable, IUpgradeable
{

    public static event Action OnBaseDestroyed;
    public GameObject HealthBar;
    private HealthBarController healthBarController;
    private MoneySpender moneySpender;
    private BaseDamageRenderer damageRenderer;
    public float maxHealth = 100;

    public float health;

    void Start()
    {
        healthBarController = HealthBar.GetComponent<HealthBarController>();
        moneySpender = GetComponent<MoneySpender>();
        damageRenderer = GetComponent<BaseDamageRenderer>();
    }

    public void onDamage(float damage) {
        health -= damage;
        healthBarController.SetHealth(health);
        damageRenderer.updateSprite(health/maxHealth);
        if (health <=0) {
            Destroy(gameObject);
            OnBaseDestroyed?.Invoke();
        }
    }

    public bool OnUpgrade(IUpgrade upgrade) {
        if (upgrade.GetType().Equals(typeof(RepairUpgrade))){
            RepairUpgrade repairUpgrade = upgrade as RepairUpgrade;
            health = Mathf.Min(maxHealth, health + (maxHealth * repairUpgrade.repairPercentage));
            moneySpender.SpendMoney(repairUpgrade.getCost());
            healthBarController.SetHealth(health);
            return true;
        }
        return false;
    }
}
