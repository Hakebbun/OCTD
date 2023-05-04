using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeUpgrade : MonoBehaviour, IBuildUpgrade
{
    public GameObject descriptionUiPrefab;
    private GameObject descriptionUiInstance;

    public GameObject aoeBufferPrefab;
    public int cost;
    public string descriptionBuildingName;

    public int getCost() {
        return cost;
    }

    public void buildBuilding() {
        Instantiate(aoeBufferPrefab, transform.position, transform.localRotation);
        Destroy(gameObject);
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
