using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase {
    DAY, NIGHT
}

public class LevelController : MonoBehaviour
{
    private static GameObject levelControllerInstance;
    private static Vector3 SPAWN_TIMER_LOCATION = new Vector3(0f, 30f, 0f);
    public float difficulty = 3f;

    public static event Action OnGameOver;
    public static event Action<Phase> OnPhaseChange;

    public GameObject gameOverUi;
    public SpawnTimer spawnTimerPrefab;
    public static bool isDay;

    private SpawnTimer activeSpawnTimer;

    private void Awake() {
        DontDestroyOnLoad (gameObject);
         
        if (levelControllerInstance == null) {
            levelControllerInstance = gameObject;
        } else {
            DestroyObject(gameObject);
        }

        BaseController.OnBaseDestroyed += EndLevelDefeat;
        SpawnTimer.OnSpawnsEmpty += EndLevelVictory;
        PlayerController.TestEvent += togglePhase;
        OnPhaseChange += PhaseChange;
        isDay = true;
    }

    void OnDestroy() {
        BaseController.OnBaseDestroyed -= EndLevelDefeat;
        SpawnTimer.OnSpawnsEmpty -= EndLevelVictory;
        PlayerController.TestEvent -= togglePhase;
        OnPhaseChange -= PhaseChange;
    }

    private void EndLevelDefeat() {
        GameObject newGameObject = Instantiate(gameOverUi, transform.position, transform.localRotation) as GameObject;
    }

    private void EndLevelVictory() {
        isDay = true;
        difficulty ++;
        OnPhaseChange(Phase.DAY);
    }

    private void createBaddieSpawner() {
        activeSpawnTimer = Instantiate(spawnTimerPrefab, SPAWN_TIMER_LOCATION, transform.localRotation) as SpawnTimer;
        activeSpawnTimer.init(difficulty);
    }

    private void togglePhase() {
        if (isDay) {
            OnPhaseChange(Phase.NIGHT);
            isDay = false;
        } else {
            OnPhaseChange(Phase.DAY);
            isDay = true;
        }
    }

    private void PhaseChange(Phase phase) {
        if (phase == Phase.NIGHT) {
            StartNightPhase();
        } else if (phase == Phase.DAY) {
            StartDayPhase();
        }
    }

    private void StartNightPhase() {
        isDay = false;
        createBaddieSpawner();
    }

    private void StartDayPhase() {
        isDay = true;
    }

}
