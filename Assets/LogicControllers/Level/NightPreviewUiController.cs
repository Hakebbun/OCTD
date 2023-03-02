using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightPreviewUiController : MonoBehaviour
{

    public TMPro.TextMeshProUGUI tmp;
    private SpawnCoordinator spawnCoordinator;
    

    // Start is called before the first frame update
    void Awake()
    {
        spawnCoordinator = gameObject.GetComponent<SpawnCoordinator>();
        LevelController.OnPhaseChange += OnPhaseChange;
        SpawnCoordinator.OnSpawnsCoordinated += OnSpawnsCoordinated;
    }

    private void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
        SpawnCoordinator.OnSpawnsCoordinated -= OnSpawnsCoordinated;
    }

    private void OnPhaseChange(Phase phase) {
        if (phase == Phase.NIGHT) {
            HidePreview();
        } else if (phase == Phase.DAY) {
            ShowPreview();
        }
    }

    private void HidePreview() {
        tmp.gameObject.SetActive(false);
    }

    private void ShowPreview() {
        tmp.gameObject.SetActive(true);
    }

    private void OnSpawnsCoordinated(List<int> spawnLocationList) {
        tmp.text = CalculatePreviewText(spawnLocationList);
    }

    private string CalculatePreviewText(List<int> spawnLocationList) {
        int left = 0;
        int middle = 0;
        int right = 0;

        for (int i = 0; i < spawnLocationList.Count; i++) {
            if (spawnLocationList[i] == 0) {left ++;}
            if (spawnLocationList[i] == 1) {middle ++;}
            if (spawnLocationList[i] == 2) {right ++;}
        }

        return string.Format("Left: {0}, Middle: {1}, Right: {2}", left, middle, right);
    }


}
