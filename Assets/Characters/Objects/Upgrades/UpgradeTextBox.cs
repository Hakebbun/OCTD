using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTextBox : MonoBehaviour
{

    public TMPro.TextMeshProUGUI tmp;

    public void setText(string text) {
        tmp.text = text;
    }
}
