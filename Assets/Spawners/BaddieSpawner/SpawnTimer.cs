using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    public static event Action OnSpawnsEmpty;

    public float MAX_TIME_TO_SPAWN = 10;
    public float MIN_TIME_TO_SPAWN = 4;

    public float spawnsLeft = 3;
    public float spawnsInPlay = 0;

    public int corpsesInPlay = 0;

    public float timeUntilSpawn; 
    public GameObject spawner0;
    public GameObject spawner1;
    public GameObject spawner2;

    private int spawnIndex = 0;
    private List<GameObject> spawnBaddiesList;
    private List<int> spawnLocationList;

    private bool isInitialised;

    private List<GameObject> spawners = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        isInitialised = false;

        BasicBaddieCorpse.OnCorpseSpawn += OnCorpseSpawn;
        BasicBaddieCorpse.OnCorpseDespawn += OnCorpseDespawn;
        BaddieUtils.OnBaddieKilled += OnBaddieKilled;

        spawners.Add(spawner0);
        spawners.Add(spawner1);
        spawners.Add(spawner2);

        timeUntilSpawn = MAX_TIME_TO_SPAWN;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialised || 
            spawnsLeft <= 0 || 
            spawnIndex >= spawnBaddiesList.Count ||
            spawnIndex >= spawnLocationList.Count) {
            return;
        }
        
        timeUntilSpawn -= Time.deltaTime;
        if ( timeUntilSpawn < 0 )
        {
            spawnNext();
            timeUntilSpawn = UnityEngine.Random.Range(MIN_TIME_TO_SPAWN, MAX_TIME_TO_SPAWN);
        }
    }

    public void init(List<GameObject> incomingSpawnBaddiesList, List<int> incomingSpawnLocationList) {
        spawnBaddiesList = incomingSpawnBaddiesList;
        spawnLocationList = incomingSpawnLocationList;
        spawnsLeft = spawnBaddiesList.Count;
        spawnIndex = 0;
        isInitialised = true;
    }

    private void spawnNext() {
        GameObject toSpawnOn = spawners[spawnLocationList[spawnIndex]];

        IBaddieSpawner spawner = toSpawnOn.GetComponent<IBaddieSpawner>();
        spawner.spawnBaddie(spawnBaddiesList[spawnIndex]);
        spawnsLeft --;
        spawnsInPlay ++;
        spawnIndex ++;
    }

    private void OnBaddieKilled() {
        spawnsInPlay --;
    }

    private void OnCorpseSpawn() {
        corpsesInPlay += 1;
    }

    private void OnCorpseDespawn(int value) {
        corpsesInPlay -= 1;
        CheckVictory();
    }

    private void CheckVictory () {
        if (spawnsLeft <= 0 && spawnsInPlay <= 0 && corpsesInPlay <=0) {
            OnSpawnsEmpty?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                Destroy(gameObject);
                break;
            case Phase.NIGHT:
                gameObject.layer = LayerMask.NameToLayer("TowerLayer");
                break;
        }
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
        BaddieUtils.OnBaddieKilled -= OnBaddieKilled;
        BasicBaddieCorpse.OnCorpseSpawn -= OnCorpseSpawn;
        BasicBaddieCorpse.OnCorpseDespawn -= OnCorpseDespawn;
    }
}
