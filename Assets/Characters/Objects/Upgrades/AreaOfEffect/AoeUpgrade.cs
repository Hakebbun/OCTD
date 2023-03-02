using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeUpgrade : MonoBehaviour, IUpgrade
{
    public GameObject descriptionUiPrefab;
    private GameObject descriptionUiInstance;

    public int getCost() {
        return 20;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            descriptionUiInstance = Instantiate(descriptionUiPrefab, gameObject.transform.position, gameObject.transform.localRotation);
            descriptionUiInstance.transform.parent = transform;
            descriptionUiInstance.transform.position = transform.position + new Vector3(12f,10f, 0);
            descriptionUiInstance.GetComponent<UpgradeTextBox>().setText(string.Format("AoE upgrade; Cost {0}; Hit more baddies!", getCost()));
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
