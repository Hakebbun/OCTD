using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIntent : MonoBehaviour
{
    public List<GameObject> baddiePrefabs = new List<GameObject>();

    public List<GameObject> generateSpawnBaddiesList(int pointsToSpend) {
        List<GameObject> result = new List<GameObject>();

        // spend all the points
        while (pointsToSpend > 0) {

            int maxRange = baddiePrefabs.Count;
            GameObject baddieToAdd = baddiePrefabs[Random.Range(0, baddiePrefabs.Count)];

            while (baddieToAdd.GetComponent<IBaddie>().GetCost() > pointsToSpend) {
                maxRange = (int) Mathf.Floor(maxRange * 0.66f);
                baddieToAdd = baddiePrefabs[Random.Range(0, maxRange)];
            }

            result.Add(baddieToAdd);
            pointsToSpend -= baddieToAdd.GetComponent<IBaddie>().GetCost();
        }
        return result;
    }

    public List<int> generateSpawnLocationList(int spawns, int numSpawnLocations) {
        List<int> result = new List<int>();
        for (int i = 0; i < spawns; i++) {
            result.Add(Random.Range(0, numSpawnLocations));
        }

        return result;
    }

}