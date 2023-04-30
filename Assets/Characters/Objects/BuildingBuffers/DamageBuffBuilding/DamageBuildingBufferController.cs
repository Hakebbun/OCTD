using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuildingBufferController : MonoBehaviour
{
    public GameObject northBuffArm;
    public GameObject eastBuffArm;
    public GameObject southBuffArm;
    public GameObject westBuffArm;

    public int rollNeededFor1Arm;
    public int rollNeededFor2Arm;
    public int rollNeededFor3Arm;
    public int rollNeededFor4Arm;

    // Start is called before the first frame update
    void Awake()
    {
        List<int> possibleArms = new List<int>();
        possibleArms.Add(0);
        possibleArms.Add(1);
        possibleArms.Add(2);
        possibleArms.Add(3);

        int armsToDelete = generateNumArmsToDelete();
        deleteRandomArms(armsToDelete);
    }

    public int generateNumArmsToDelete() {
        int armSeed =  Random.Range(0, 100);
        if (armSeed >= rollNeededFor4Arm) {
            return 0;
        } else if (armSeed < rollNeededFor4Arm && armSeed >= rollNeededFor3Arm) {
            return 1;
        } else if (armSeed < rollNeededFor3Arm && armSeed >= rollNeededFor2Arm) {
            return 2;
        } else {
            return 3;
        }
    }

    public void deleteRandomArms(int numToDelete) {
        List<int> possibleArms = new List<int>();
        possibleArms.Add(0);
        possibleArms.Add(1);
        possibleArms.Add(2);
        possibleArms.Add(3);

        for (int i = 0; i < numToDelete; i++ ) {
            int toDeleteIndex = Random.Range(0, possibleArms.Count);
            int toDelete = possibleArms[toDeleteIndex];

            if (toDelete == 0) {
                Destroy(northBuffArm);
            } else if (toDelete == 1) {
                Destroy(eastBuffArm);
            } else if (toDelete == 2) {
                Destroy(southBuffArm);
            } else if (toDelete == 3) {
                Destroy(westBuffArm);
            }

            possibleArms.RemoveAt(toDeleteIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
