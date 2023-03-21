using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBaddieSpawner : MonoBehaviour, IBaddieSpawner
{
    // Start is called before the first frame update
    public void spawnBaddie(GameObject baddieToSpawn) {
        GameObject newBaddie = Instantiate(baddieToSpawn, transform.position, transform.localRotation) as GameObject;
        IBaddie baddie = newBaddie.GetComponent<IBaddie>();

        if (baddie != null) {
            baddie.init();
        }
    }

}
