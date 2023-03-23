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
    public static int difficulty = 1;

    public static event Action<Phase> OnPhaseChange;

    public GameObject gameOverUi;
    
    public static bool isDay;

    private void Awake() {
        DontDestroyOnLoad (gameObject);
         
        if (levelControllerInstance == null) {
            levelControllerInstance = gameObject;
        } else {
            Destroy(gameObject);
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
        difficulty += 2;
        OnPhaseChange(Phase.DAY);
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
    }

    private void StartDayPhase() {
        isDay = true;
    }

}
