using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpawner : MonoBehaviour
{
    public List<GameObject> possibleUpgrades;

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
            Destroy(gameObject);
        }

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
            GameObject upgradeTypeToInstantiate = possibleUpgrades[Random.Range(0, possibleUpgrades.Count)];
            Instantiate(upgradeTypeToInstantiate, 
                        new Vector2(Random.Range(MIN_POS_X, MAX_POS_X), Random.Range(MIN_POS_Y, MAX_POS_Y)), 
                        Quaternion.identity);
        
        }
    }
}