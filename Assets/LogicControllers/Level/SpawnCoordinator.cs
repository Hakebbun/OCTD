using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoordinator : MonoBehaviour
{
    public static event Action<List<int>> OnSpawnsCoordinated;

    private SpawnIntent spawnIntent;
    private static GameObject spawnCoordinatorInstance;

    public SpawnTimer spawnTimerPrefab;
    private static Vector3 SPAWN_TIMER_LOCATION = new Vector3(0f, 30f, 0f);
    private SpawnTimer activeSpawnTimer;

    public List<GameObject> spawnBaddiesList;
    public List<int> spawnLocationList;

    void Awake() {
        DontDestroyOnLoad (gameObject);
         
        if (spawnCoordinatorInstance == null) {
            spawnCoordinatorInstance = gameObject;
        } else {
            DestroyObject(gameObject);
        }

        spawnIntent = GetComponent<SpawnIntent>();
        LevelController.OnPhaseChange += OnPhaseChange;
        GetAndDisplayIntent();
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }

    private void OnPhaseChange(Phase phase) {
        if (phase == Phase.NIGHT) {
            CreateAndStartSpawner();
        } else if (phase == Phase.DAY) {
            GetAndDisplayIntent();
        }
    }

    private void CreateAndStartSpawner() {
        activeSpawnTimer = Instantiate(spawnTimerPrefab, SPAWN_TIMER_LOCATION, transform.localRotation) as SpawnTimer;
        activeSpawnTimer.init(spawnBaddiesList, spawnLocationList);
    }

    private void GetAndDisplayIntent() {
        spawnBaddiesList = spawnIntent.generateSpawnBaddiesList(LevelController.difficulty);
        spawnLocationList = spawnIntent.generateSpawnLocationList(
            spawnBaddiesList.Count,
            3
        );
        DisplayIntent();
    }

    private void DisplayIntent() {
        OnSpawnsCoordinated?.Invoke(spawnLocationList);
    }


}