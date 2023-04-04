using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBuildingUpgrade : MonoBehaviour, IBuildUpgrade
{
    public GameObject seedTowerPrefab;
    public int cost = 20;
    public GameObject descriptionUiPrefab;
    private GameObject descriptionUiInstance;
    public string descriptionBuildingName;

    public void buildBuilding() {
        Instantiate(seedTowerPrefab, transform.position, transform.localRotation);
        Destroy(gameObject);
    }

    public int getCost() {
        return cost;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            descriptionUiInstance = Instantiate(descriptionUiPrefab, gameObject.transform.position, gameObject.transform.localRotation);
            descriptionUiInstance.transform.parent = transform;
            descriptionUiInstance.transform.position = transform.position + new Vector3(12f,10f, 0);
            descriptionUiInstance.GetComponent<UpgradeTextBox>().setText(string.Format("Buy a new {1}!; Cost {0}", getCost(), descriptionBuildingName));
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
            descriptionUiInstance != null) {
                Destroy(descriptionUiInstance);
                descriptionUiInstance = null;
        }
    }
}
