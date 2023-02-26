using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBaddieSpawner : MonoBehaviour, IBaddieSpawner
{

    public GameObject baddieToSpawn;
    // Start is called before the first frame update

    public void spawnBaddie() {
        GameObject newBaddie = Instantiate(baddieToSpawn, transform.position, transform.localRotation) as GameObject;
    }

}
