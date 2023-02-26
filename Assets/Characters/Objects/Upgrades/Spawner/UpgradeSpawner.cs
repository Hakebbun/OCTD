using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpawner : MonoBehaviour
{

    public RepairUpgrade repairUpgrade;
    public AoeUpgrade aoeUpgrade;
    public DamageUpgrade damageUpgrade;

    private List<Upgrades> possibleUpgrades = new List<Upgrades>();

    private static GameObject upgradeSpawnerInstance;

    private static int MAX_POS_X = 55;
    private static int MIN_POS_X = -55;
    private static int MAX_POS_Y = -15;
    private static int MIN_POS_Y = -30;

    public int upgradesToSpawn;


    void Start() {
        DontDestroyOnLoad (gameObject);
         
        if (upgradeSpawnerInstance == null) {
            upgradeSpawnerInstance = gameObject;
        } else {
            DestroyObject(gameObject);
        }

        possibleUpgrades.Add(Upgrades.REPAIR);
        possibleUpgrades.Add(Upgrades.AOE);
        possibleUpgrades.Add(Upgrades.DAMAGE);

        upgradesToSpawn = 2;

        LevelController.OnPhaseChange += OnPhaseChange;
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }

    void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                spawnUpgrades();
                break;
            case Phase.NIGHT:
                break;
        }
    }

    public void spawnUpgrades() {
        for (int i = 0; i < upgradesToSpawn; i++) {
            Upgrades upgradeTypeToInstantiate = possibleUpgrades[Random.Range(0, possibleUpgrades.Count)];
            switch(upgradeTypeToInstantiate) {
                case Upgrades.REPAIR:
                    Instantiate(repairUpgrade, 
                        new Vector2(Random.Range(MIN_POS_X, MAX_POS_X), Random.Range(MIN_POS_Y, MAX_POS_Y)), 
                        Quaternion.identity);
                    break;
                case Upgrades.AOE:
                    Instantiate(aoeUpgrade, 
                        new Vector2(Random.Range(MIN_POS_X, MAX_POS_X), Random.Range(MIN_POS_Y, MAX_POS_Y)),
                        Quaternion.identity);
                    break;
                case Upgrades.DAMAGE:
                    Instantiate(damageUpgrade,
                        new Vector2(Random.Range(MIN_POS_X, MAX_POS_X), Random.Range(MIN_POS_Y, MAX_POS_Y)),
                        Quaternion.identity);
                    break;
            }
        }
    }
}

public enum Upgrades {
    REPAIR,
    AOE,
    DAMAGE
}